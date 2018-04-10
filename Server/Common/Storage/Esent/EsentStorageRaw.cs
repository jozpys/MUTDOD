using System;
using System.IO;
using Microsoft.Isam.Esent.Interop;
using MUTDOD.Common.ModuleBase.Storage.Core.Metadata;
using MUTDOD.Common.Types;

namespace MUTDOD.Server.Common.Storage.Strategies.Esent
{
    public class EsentStorageRaw
    {
        private readonly IDatabaseParameters _databaseParameters;
        private JET_INSTANCE _instance;
        private JET_SESID _sesid;
        private JET_DBID _dbid;
        private JET_TABLEID _tableid;

        private JET_COLUMNID _dataColumnId;
        private JET_COLUMNID _idColumnId;

        public EsentStorageRaw(IDatabaseParameters databaseParameters)
        {
            _databaseParameters = databaseParameters;
            CreateInstance();
            OpenDatabase();
        }

        private void OpenDatabase()
        {
            if (IsNewDatabase())
            {
                Api.JetCreateDatabase(_sesid, _databaseParameters.DataFileFullPath, null, out _dbid,
                    CreateDatabaseGrbit.None);

                Api.JetBeginTransaction(_sesid);
                Api.JetCreateTable(_sesid, _dbid, "nodes", 100, 100, out _tableid);

                var idColumn = new JET_COLUMNDEF {coltyp = JET_coltyp.Binary, cp = JET_CP.None};
                Api.JetAddColumn(_sesid, _tableid, "id", idColumn, null, 0, out _idColumnId);

                var dataColumn = new JET_COLUMNDEF {coltyp = JET_coltyp.LongBinary, cp = JET_CP.None};
                Api.JetAddColumn(_sesid, _tableid, "data", dataColumn, null, 0, out _dataColumnId);

                const string indexDef = "+id\0\0";
                Api.JetCreateIndex(_sesid, _tableid, "primary", CreateIndexGrbit.IndexPrimary, indexDef, indexDef.Length,
                    100);

                Api.JetCommitTransaction(_sesid, CommitTransactionGrbit.LazyFlush);
            }
            else
            {
                Api.JetAttachDatabase(_sesid, _databaseParameters.DataFileFullPath, AttachDatabaseGrbit.None);
                Api.JetOpenDatabase(_sesid, _databaseParameters.DataFileFullPath, "", out _dbid,
                    OpenDatabaseGrbit.Exclusive);

                Api.JetBeginTransaction(_sesid);
                Api.JetOpenTable(_sesid, _dbid, "nodes", null, 0, OpenTableGrbit.Preread, out _tableid);
                var columns = Api.GetColumnDictionary(_sesid, _tableid);
                _idColumnId = columns["id"];
                _dataColumnId = columns["data"];
                Api.JetCommitTransaction(_sesid, CommitTransactionGrbit.LazyFlush);
            }
        }

        private void CreateInstance()
        {
            Api.JetCreateInstance(out _instance, "instance");

            Api.JetSetSystemParameter(_instance, JET_SESID.Nil, JET_param.DbExtensionSize,
                _databaseParameters.IncreaseFactor, null);
            Api.JetSetSystemParameter(_instance, JET_SESID.Nil, JET_param.LogFileSize, 16*1024, null);
            Api.JetSetSystemParameter(_instance, JET_SESID.Nil, JET_param.LogBuffers, 16*1024, null);

            Api.JetSetSystemParameter(_instance, JET_SESID.Nil, JET_param.CircularLog, 1, null);

            Api.JetInit(ref _instance);
            Api.JetBeginSession(_instance, out _sesid, null, null);
        }

        private bool IsNewDatabase()
        {
            if (File.Exists(_databaseParameters.DataFileFullPath))
            {
                return false;
            }
            return true;
        }

        private void WriteRecord(Oid oid, byte[] data)
        {
            Api.JetPrepareUpdate(_sesid, _tableid, JET_prep.Insert);
            Api.SetColumn(_sesid, _tableid, _idColumnId, (oid.Oli + BitConverter.ToInt64(oid.Id.ToByteArray(), 0)));
            Api.SetColumn(_sesid, _tableid, _dataColumnId, data);
            Api.JetUpdate(_sesid, _tableid);

        }

        private void UpdateRecord(Oid oid, byte[] data)
        {
            Api.JetPrepareUpdate(_sesid, _tableid, JET_prep.Replace);
            Api.SetColumn(_sesid, _tableid, _dataColumnId, data);
            Api.JetUpdate(_sesid, _tableid);
        }

        private byte[] ReadRecord(Oid oid)
        {

            SeekToKey(oid);

            var buffer = Api.RetrieveColumn(_sesid, _tableid, _dataColumnId);

            return buffer;
        }

        private bool SeekToKey(Oid oid)
        {

            Api.JetSetCurrentIndex(_sesid, _tableid, null);
            Api.MakeKey(_sesid, _tableid, (oid.Oli + +BitConverter.ToInt64(oid.Id.ToByteArray(), 0)),
                MakeKeyGrbit.NewKey);
            return Api.TrySeek(_sesid, _tableid, SeekGrbit.SeekEQ);
        }

        public void Save(Oid oid, byte[] data)
        {
            //if(SeekToKey(oid))
            //{
            //    UpdateRecord(oid, data);
            //}
            //else
            //{
            WriteRecord(oid, data);
            //}
        }

        public void Delete(Oid oid)
        {
            SeekToKey(oid);

            Api.JetDelete(_sesid, _tableid);
        }

        public byte[] Read(Oid oid)
        {
            return ReadRecord(oid);
        }

        public void BeginTransaction()
        {
            Api.JetBeginTransaction(_sesid);

        }

        public void CommitTransaction()
        {
            Api.JetCommitTransaction(_sesid, CommitTransactionGrbit.LazyFlush);
        }
    }
}