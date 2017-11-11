using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CSharpTest.Net.Collections;
using MutDood.Storage.Strategies.Mmf.Serializers;
using MUTDOD.Common.ModuleBase.Storage.Core;
using MUTDOD.Common.ModuleBase.Storage.Core.Metadata;
using MUTDOD.Common.ModuleBase.Storage.Exceptions;
using MUTDOD.Common.ModuleBase.Storage.Strategy;
using MUTDOD.Common.Types;
using MUTDOD.Server.Common.Storage.Strategies.Mmf.Persistence;
using MUTDOD.Server.Common.Storage.Strategies.Mmf.Serializers;

namespace MUTDOD.Server.Common.Storage.Strategies.Mmf
{
    public class Engine : IEngine
    {
        private readonly Dictionary<Did, BPlusTree<Oid, byte[]>> _storages = new Dictionary<Did, BPlusTree<Oid, byte[]>>();

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
            _storages[parameters.DatabaseId].Dispose();
            _storages[parameters.DatabaseId] = null;
            _storages.Remove(parameters.DatabaseId);

            if (File.Exists(parameters.DataFileFullPath))
            {
                File.Delete(parameters.DataFileFullPath);

            }
        }

        public void Save(Did did, SerializedStorable storable)
        {
            _storages[did].Add(storable.Oid, storable.Data);
        }

        public void Save(Did did, List<SerializedStorable> storables)
        {
            foreach (var serializedStorable in storables)
            {

                _storages[did].Add(serializedStorable.Oid, serializedStorable.Data);

            }
        }

        public byte[] Read(Did did, Oid oid)
        {
            if (!_storages[did].ContainsKey(oid))
            {
                throw new ObjectNotFoundException(String.Format("Nie odnaleziono obiektu: '{0}'", oid));
            }
            return _storages[did][oid];
        }

        public Dictionary<Oid, byte[]> ReadAll(Did did)
        {
            return _storages[did].ToDictionary(pair => pair.Key, pair => pair.Value);
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

            var options = new BPlusTree<Oid, byte[]>.Options(new KeySerializer(), new ValueSerializer());
            options.FileName = parameters.DataFileFullPath;
            options.CreateFile = CreatePolicy.IfNeeded;
            options.StorageSystem = new MmfStorageStream(parameters);
            options.StorageType = StorageType.Custom;

            if (!File.Exists(parameters.DataFileFullPath))
            {
                options.FileGrowthRate = parameters.IncreaseFactor;
                options.CachePolicy = CachePolicy.Recent;
                options.ConcurrentWriters = 4;
                options.FileBlockSize = 4096;
                options.FileOpenOptions = FileOptions.RandomAccess;
                options.MaximumValueNodes = 128;
                options.MinimumValueNodes = 64;
                options.MaximumChildNodes = 128;
                options.MinimumChildNodes = 32;

            }

            _storages.Add(parameters.DatabaseId, new BPlusTree<Oid, byte[]>(options));
        }

        public void Dispose()
        {
            if (_storages != null)
            {
                foreach (var storage in _storages)
                {
                    if (storage.Value != null)
                    {
                        storage.Value.Dispose();
                    }
                }
            }
        }
    }
}