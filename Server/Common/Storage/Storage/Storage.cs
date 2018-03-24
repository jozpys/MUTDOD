using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using MutDood.Storage.Core.MetadataElements;
using MUTDOD.Common;
using MUTDOD.Common.ModuleBase;
using MUTDOD.Common.ModuleBase.Storage;
using MUTDOD.Common.ModuleBase.Storage.Core;
using MUTDOD.Common.ModuleBase.Storage.Core.Metadata;
using MUTDOD.Common.ModuleBase.Storage.Exceptions;
using MUTDOD.Common.ModuleBase.Storage.Strategy;
using MUTDOD.Common.Settings;
using MUTDOD.Common.Types;
using MUTDOD.Server.Common.Storage.MetadataElements;
using MUTDOD.Server.Common.Storage.Serialization;
using MUTDOD.Server.Common.Storage.Strategies.Speed;
using System.IO;

namespace MUTDOD.Server.Common.Storage
{
    public class Storage : Module, IStorage, IMetadataStorage, IStorageManagement, IDisposable
    {
        private readonly IEngine _engine;
        private readonly ISerializer _serializer;
        private readonly StorageMetadata _metadata;
        private readonly ILogger _logger;

        private Storage(IEngine engine,
            ISerializer serializer,
            StorageMetadata metadata,
            ILogger logger)
        {
            _engine = engine;
            _metadata = metadata;
            _logger = logger;
            _serializer = serializer;
        }

        public Storage(ISettingsManager settingsManager,
            ILogger logger)
        {
            _logger = logger;
            switch (settingsManager.StorageStrategy)
            {
                case StorageStrategy.Speed:
                    _engine = Engine.Create();
                    break;
                case StorageStrategy.Esent:
                    _engine = Strategies.Esent.Engine.Create();
                    break;
                case StorageStrategy.MemoryMappedFiles:
                    _engine = Strategies.Mmf.Engine.Create();
                    break;
                default:
                    _engine = Engine.Create();
                    break;
            }
            _metadata = StorageMetadata.ReadMetadata(_logger);
            foreach (var databaseParameters in _metadata.Databases)
            {
                _engine.OpenDatabase(databaseParameters.Value);
            }
            _serializer = DefaultSerializer.Create(_metadata);
        }

        public Oid Save(Did dbId, IStorable toStore)
        {
            var serialized = _serializer.Serialize(toStore);

            foreach (var serializedStorable in serialized)
            {
                _engine.Save(dbId, serializedStorable);
            }

            return serialized.Count == 0 ? null : serialized[0].Oid;
        }

        public IEnumerable<Oid> Save(Did dbId, IEnumerable<IStorable> toStore)
        {
            List<SerializedStorable> storables = new List<SerializedStorable>();
            var oids = new List<Oid>();
            foreach (var storable in toStore)
            {
                var tmp = _serializer.Serialize(storable);
                oids.Add(tmp[0].Oid);
                storables.AddRange(tmp);
            }
            _engine.Save(dbId, storables);
            return oids;
        }

        public IStorable Get(Did dbId, Oid oid)
        {
            var data = _engine.Read(dbId, oid);
            return _serializer.Deserialize(dbId, new SerializedStorable {Oid = oid, Data = data});
        }

        public IEnumerable<IStorable> GetAll(Did dbId)
        {
            foreach (var read in _engine.ReadAll(dbId))
                yield return _serializer.Deserialize(dbId, new SerializedStorable { Oid = read.Key, Data = read.Value });
        }

        public IStorable[] Find(Did dbId, ISearchCriteria searchCriteria)
        {
            throw new NotImplementedException();
        }


        public void SaveSchema(IDatabaseSchema schema)
        {
            if (!_metadata.Databases.ContainsKey(schema.DatabaseId))
            {
                throw new DatabaseNotFoundException(String.Format("Nie odnaleziono bazy danych o id: '{0}'",
                    schema.DatabaseId));
            }
            _metadata.Databases[schema.DatabaseId].Schema = schema;
            StorageMetadata.SaveMetadata(_metadata, _logger);
        }

        public IDatabaseSchema GetSchema(Did databaseId)
        {
            if (!_metadata.Databases.ContainsKey(databaseId))
            {
                throw new DatabaseNotFoundException(String.Format("Nie odnaleziono bazy danych o id: '{0}'",
                    databaseId));
            }
            return _metadata.Databases[databaseId].Schema;
        }


        public Did CreateDatabase(IDatabaseParameters databaseParameters)
        {
            if (_metadata.Databases.Any(d => d.Value.Name.Equals(databaseParameters.Name)))
            {
                throw new DatabaseAlreadyExistException(String.Format("Baza danych o nazwie '{0}' już istnieje",
                    databaseParameters.Name));
            }
            var newDid = Did.CreateNew();
            if (databaseParameters.Schema == null)
                databaseParameters.Schema = new DatabaseSchema()
                {
                    DatabaseId = newDid,
                    Classes = new ConcurrentDictionary<ClassId, Class>(),
                    Properties = new ConcurrentDictionary<PropertyId, Property>(),
                    Methods = new ConcurrentDictionary<ClassId, List<string>>()
                };
            newDid.Duid = _metadata.Databases.Count == 0 ? 0 : _metadata.Databases.Max(d => d.Key.Duid) + 1;
            databaseParameters.DatabaseId = newDid;
            if (!_metadata.Databases.TryAdd(newDid, databaseParameters))
                throw  new ApplicationException("Database creation internal storage error...");
            _engine.OpenDatabase(databaseParameters);

            StorageMetadata.SaveMetadata(_metadata, _logger);
            return newDid;
        }

        public Did RenameDatabase(IDatabaseParameters databaseParameters, string databaseNweName, ISettingsManager settingsManager)
        {
            DatabaseParameters renamedParameters = new DatabaseParameters(databaseNweName, settingsManager);
            renamedParameters.DatabaseId = databaseParameters.DatabaseId;
            renamedParameters.Schema = databaseParameters.Schema;
            renamedParameters.PageSize = databaseParameters.PageSize;
            renamedParameters.OptimizeSize = databaseParameters.OptimizeSize;
            renamedParameters.StartupSize = databaseParameters.StartupSize;
            renamedParameters.IncreaseFactor = databaseParameters.IncreaseFactor;

            _metadata.Databases[databaseParameters.DatabaseId] = renamedParameters;

            StorageMetadata.SaveMetadata(_metadata, _logger);
            //_engine.Dispose();
            //File.Move(databaseParameters.DataFileFullPath, renamedParameters.DataFileFullPath);
            _engine.OpenDatabase(renamedParameters);
            return renamedParameters.DatabaseId;
        }

        public DeleteDatabaseStatus RemoveDatabase(IDatabaseRemoveParameters databaseRemoveParameters)
        {
            if (!_metadata.Databases.ContainsKey(databaseRemoveParameters.DatabaseToRemove))
            {
                throw new DatabaseNotFoundException(String.Format("Nie odnaleziono bazy danych o id: '{0}'",
                    databaseRemoveParameters.DatabaseToRemove));
            }

            IDatabaseParameters parameters = null;
            _metadata.Databases.TryRemove(databaseRemoveParameters.DatabaseToRemove, out parameters);

            StorageMetadata.SaveMetadata(_metadata, _logger);
            return new DeleteDatabaseStatus();
        }

        public IDatabaseParameters[] GetDatabases()
        {
            return _metadata.Databases.Values.ToArray();
        }

        public IDatabaseParameters GetDatabase(Did did)
        {
            if (!_metadata.Databases.ContainsKey(did))
            {
                throw new DatabaseNotFoundException(String.Format("Nie odnaleziono bazy danych o id: '{0}'",
                    did));
            }
            return _metadata.Databases[did];
        }

        public void Dispose()
        {
            if (_engine != null)
            {
                _engine.Dispose();
            }
            if (_metadata != null)
            {
                StorageMetadata.SaveMetadata(_metadata, _logger);
            }
        }

        public string Name
        {
            get
            {
                return
                    "Storage";
            }
        }
    }
}