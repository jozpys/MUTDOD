using System;
using System.Collections.Generic;
using System.IO;
using MUTDOD.Common.ModuleBase.Storage.Core;
using MUTDOD.Common.ModuleBase.Storage.Core.Metadata;
using MUTDOD.Common.ModuleBase.Storage.Exceptions;
using MUTDOD.Common.ModuleBase.Storage.Strategy;
using MUTDOD.Common.Types;

namespace MUTDOD.Server.Common.Storage.Strategies.Esent
{
    public class Engine : IEngine
    {
        private readonly Dictionary<Did, EsentStorageRaw> _storages = new Dictionary<Did, EsentStorageRaw>();

        private Engine()
        {

        }
        public static IEngine Create()
        {
            return new Engine();
        }

        public void DeleteDataFiles(IDatabaseParameters parameters)
        {
            OpenDatabase(parameters);
            _storages[parameters.DatabaseId] = null;
            _storages.Remove(parameters.DatabaseId);

            if (File.Exists(parameters.DataFileFullPath))
            {
                File.Delete(parameters.DataFileFullPath);

            }
        }

        public void Save(Did did, SerializedStorable storable)
        {
            _storages[did].BeginTransaction();
            _storages[did].Save(storable.Oid, storable.Data);
            _storages[did].CommitTransaction();
        }

        public void Save(Did did, List<SerializedStorable> storables)
        {
            _storages[did].BeginTransaction();
            foreach (var serializedStorable in storables)
            {
                _storages[did].Save(serializedStorable.Oid, serializedStorable.Data);
            }
            _storages[did].CommitTransaction();
        }


        public byte[] Read(Did did, Oid oid)
        {
            var ret = _storages[did].Read(oid);
            if (ret == null || ret.Length == 0)
            {
                throw new ObjectNotFoundException(String.Format("Nie odnaleziono obiektu: '{0}'", oid));
            }
            return ret;
        }

        public Dictionary<Oid, byte[]> ReadAll(Did did)
        {
            throw new NotImplementedException();
        }

        public void OpenDatabase(IDatabaseParameters parameters)
        {
            if (_storages.ContainsKey(parameters.DatabaseId))
            {
                return;
            }
            if (!Directory.Exists(parameters.BaseDirectory))
            {
                Directory.CreateDirectory(parameters.BaseDirectory);
            }

            _storages.Add(parameters.DatabaseId, new EsentStorageRaw(parameters));
        }

        public void Dispose()
        {
            if (_storages != null)
            {
                _storages.Clear();
            }
        }
    }
}