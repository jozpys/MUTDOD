namespace MUTDOD.Server.Common.Storage.Strategies.Esent
{
    //internal class EsentStorage : INodeStorage
    //{
    //    static readonly ISerializer<IStorageHandle> StorageHandleSerializer = new HandleSerializer();
    //    private readonly IDatabaseParameters _databaseParameters;
    //    private JET_INSTANCE _instance;
    //    private JET_SESID _sesid;
    //    private JET_DBID _dbid;
    //    private JET_TABLEID _tableid;
        
    //    private JET_COLUMNID _dataColumnId;
    //    private JET_COLUMNID _idColumnId;

    //    private readonly RecordId _rootId = RecordId.FirstIdentity;

    //    public EsentStorage(IDatabaseParameters databaseParameters)
    //    {
    //        _databaseParameters = databaseParameters;
    //        CreateInstance();
    //        OpenDatabase();
    //    }

    //    private void OpenDatabase()
    //    {
    //        if (IsNewDatabase())
    //        {
    //            Api.JetCreateDatabase(_sesid, _databaseParameters.DataFileFullPath, null, out _dbid, CreateDatabaseGrbit.None);

    //            Api.JetBeginTransaction(_sesid);
    //            Api.JetCreateTable(_sesid, _dbid, "nodes", 100, 100, out _tableid);

    //            var idColumn = new JET_COLUMNDEF { coltyp = JET_coltyp.Long, cp = JET_CP.None };
    //            Api.JetAddColumn(_sesid, _tableid, "id", idColumn, null, 0, out _idColumnId);

    //            var dataColumn = new JET_COLUMNDEF {coltyp = JET_coltyp.LongBinary, cp = JET_CP.None};
    //            Api.JetAddColumn(_sesid, _tableid, "data", dataColumn, null, 0, out _dataColumnId);

    //            const string indexDef = "+id\0\0";
    //            Api.JetCreateIndex(_sesid, _tableid, "primary", CreateIndexGrbit.IndexPrimary, indexDef, indexDef.Length, 100);

    //            Api.JetCommitTransaction(_sesid, CommitTransactionGrbit.LazyFlush);
    //            CreateRoot();
    //        }
    //        else
    //        {
    //            Api.JetAttachDatabase(_sesid, _databaseParameters.DataFileFullPath, AttachDatabaseGrbit.None);
    //            Api.JetOpenDatabase(_sesid, _databaseParameters.DataFileFullPath, "", out _dbid,
    //                                OpenDatabaseGrbit.None);

    //            Api.JetBeginTransaction(_sesid);
    //            Api.JetOpenTable(_sesid, _dbid, "nodes", null, 0, OpenTableGrbit.Preread, out _tableid);
    //            var columns = Api.GetColumnDictionary(_sesid, _tableid);
    //            _idColumnId = columns["id"];
    //            _dataColumnId = columns["data"];
    //            Api.JetCommitTransaction(_sesid, CommitTransactionGrbit.LazyFlush);
    //        }
    //    }

    //    private void CreateInstance()
    //    {
    //        Api.JetCreateInstance(out _instance, "instance");
           
    //        Api.JetSetSystemParameter(_instance, JET_SESID.Nil, JET_param.DbExtensionSize, _databaseParameters.IncreaseFactor, null);

    //        Api.JetSetSystemParameter(_instance, JET_SESID.Nil, JET_param.CircularLog, 1, null);
            
    //        Api.JetInit(ref _instance);
    //        Api.JetBeginSession(_instance, out _sesid, null, null);
    //    }

    //    public void WriteTo(IStorageHandle value, Stream stream)
    //    {
    //        StorageHandleSerializer.WriteTo(value, stream);
    //    }

    //    public IStorageHandle ReadFrom(Stream stream)
    //    {
    //        return StorageHandleSerializer.ReadFrom(stream);
    //    }

    //    public void Dispose()
    //    {
    //        Api.JetCloseTable(_sesid, _tableid);
    //        Api.JetEndSession(_sesid, EndSessionGrbit.None);
    //        Api.JetTerm(_instance);
    //    }

    //    public IStorageHandle OpenRoot(out bool isNew)
    //    {
    //        byte[] record = ReadRecord(_rootId);
    //        isNew = (record == null || record.Length == 0);
    //        return _rootId;
    //    }

    //    private byte[] ReadRecord(RecordId recordId)
    //    {
            
    //        SeekToKey(recordId);

    //        var buffer = Api.RetrieveColumn(_sesid, _tableid, _dataColumnId);
            
    //        return buffer;
    //    }
    //    private void WriteRecord(RecordId recordId, byte[] data)
    //    {
    //        Api.JetBeginTransaction(_sesid);
    //        Api.JetPrepareUpdate(_sesid, _tableid, JET_prep.Insert);
    //        Api.SetColumn(_sesid, _tableid, _idColumnId, recordId.Id);
    //        Api.SetColumn(_sesid, _tableid, _dataColumnId, data);
    //        Api.JetUpdate(_sesid,_tableid);
    //        Api.JetCommitTransaction(_sesid, CommitTransactionGrbit.LazyFlush);
    //    }
    //    private void UpdateRecord(RecordId recordId, byte[] data)
    //    {
    //        Api.JetBeginTransaction(_sesid);
    //        if(!SeekToKey(recordId))
    //        {
    //            Api.JetRollback(_sesid, RollbackTransactionGrbit.RollbackAll);
    //            WriteRecord(recordId, data);
    //            return;
    //        }
    //        Api.JetPrepareUpdate(_sesid, _tableid, JET_prep.Replace);
    //        Api.SetColumn(_sesid, _tableid, _dataColumnId, data);
    //        Api.JetUpdate(_sesid, _tableid);
    //        Api.JetCommitTransaction(_sesid, CommitTransactionGrbit.LazyFlush);
    //    }
    //    private void DeleteRecord(RecordId recordId)
    //    {
    //        Api.JetBeginTransaction(_sesid);
    //        SeekToKey(recordId);
    //        Api.JetDelete(_sesid, _tableid);
    //        Api.JetCommitTransaction(_sesid, CommitTransactionGrbit.LazyFlush);
    //    }
    //    private void CreateRoot()
    //    {
    //        WriteRecord(_rootId, new byte[]{});
    //    }
    //    private bool SeekToKey(RecordId recordId)
    //    {
    //        // We need to be on the primary index (which indexes the 'symbol' column).
    //        Api.JetSetCurrentIndex(_sesid, _tableid, null);
    //        Api.MakeKey(_sesid, _tableid, recordId.Id, MakeKeyGrbit.NewKey);

    //        // This seek expects the record to be present. To test for a record
    //        // use TrySeek(), which won't throw an exception if the record isn't
    //        // found.
    //        return Api.TrySeek(_sesid, _tableid, SeekGrbit.SeekEQ);
    //    }
    //    public void Reset()
    //    {
    //        Api.JetBeginTransaction(_sesid);
    //        if (Api.TryMoveFirst(_sesid, _tableid))
    //        {
    //            do
    //            {
    //                Api.JetDelete(_sesid, _tableid);        
    //            } while (Api.TryMoveNext(_sesid, _tableid));
    //        }
    //        Api.JetCommitTransaction(_sesid, CommitTransactionGrbit.LazyFlush);
    //        CreateRoot();
    //    }

    //    public bool TryGetNode<TNode>(IStorageHandle handle, out TNode node, ISerializer<TNode> serializer)
    //    {
    //        var buffer = ReadRecord(handle as RecordId);
    //        if (buffer == null)
    //        {
    //            node = default(TNode);
    //            return false;
    //        }
    //        using (var stream = new MemoryStream(buffer))
    //        {
    //            node = serializer.ReadFrom(stream);
    //            return true;
    //        }
    //    }

    //    public IStorageHandle Create()
    //    {
    //        var recordId = new RecordId();
    //       // WriteRecord(recordId, new byte[]{});
    //        return recordId;
    //    }

    //    private bool IsNewDatabase()
    //    {
    //        if (File.Exists(_databaseParameters.DataFileFullPath))
    //        {
    //            return false;
    //        }
    //        return true;
    //    }

    //    public void Destroy(IStorageHandle handle)
    //    {
    //        DeleteRecord(handle as RecordId);
    //    }

    //    public void Update<TNode>(IStorageHandle handle, ISerializer<TNode> serializer, TNode node)
    //    {
    //        using (var stream = new MemoryStream())
    //        {
    //            serializer.WriteTo(node, stream);
    //            var data = stream.ToArray();
    //            UpdateRecord(handle as RecordId, data);
    //        }
            
    //    }
    //}

    
}