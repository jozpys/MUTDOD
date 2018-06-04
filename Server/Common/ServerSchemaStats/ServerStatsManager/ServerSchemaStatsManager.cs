using MUTDOD.Common.ModuleBase;
using MUTDOD.Common.ModuleBase.Storage.Core.Metadata;
using MUTDOD.Common.Types;
using MUTDOD.Server.Common.ServerStats.StatsStorage;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MUTDOD.Server.Common.ServerStats
{
    class ServerSchemaStatsManager
    {
        private static ServerSchemaStatsManager _instance = null;
        private static List<SchemaStats> schemasStats = null;

        private string _serverStatsStoragePath
        {
            get
            {
                if (!Directory.Exists(Settings.GetInstance().ServerStatisticDataStorageDirectory))
                    Directory.CreateDirectory(Settings.GetInstance().ServerStatisticDataStorageDirectory);
                return Settings.GetInstance().ServerStatisticDataStorageDirectory;
            }
        }

        public static ServerSchemaStatsManager GetInstance(IStorage storage)
        {
            return _instance ?? (_instance = new ServerSchemaStatsManager(storage));
        }

        private ServerSchemaStatsManager(IStorage storage)
        {
            schemasStats = new List<SchemaStats>();
            RecalculateStats(storage);
            ServerSchemaStatsStorageManager.GetStatisticDataAllSchemas().ForEach(p => schemasStats.Add((SchemaStats)p));
        }
        internal int GetClassObjectNumber(Did databaseId, ClassId classId)
        {
            var schemaStats = schemasStats.SingleOrDefault(p => p.DatabaseId.Equals(databaseId));
            if (schemaStats != null)
            {
                return schemaStats.ClassStats.objectNumbers.TryGetValue(classId, out int number) ? number : 0;
            }
            else
                return 0;

        }

        internal void UpdateStatisticSchemaData(Did DatabaseId, SchemaStats schemaStats)
        {
            ServerSchemaStatsStorageManager.UpdateStatisticSchemaData(DatabaseId, schemaStats);
        }

        internal bool RecalculateStats(IStorage storage)
        {
            try{
                storage.GetDatabases()?.ToList().ForEach(p => RecalculateSchemaStats(storage, p));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void RecalculateSchemaStats(IStorage storage, IDatabaseParameters databaseParameters)
        {
            var schemasStat = new SchemaStats();
            databaseParameters.Schema.Classes.ToList()
                                             .ForEach(p =>
                                                           schemasStat.ClassStats.objectNumbers.TryAdd(p.Key, GetObjectClassNumber(storage, databaseParameters, p.Value)));
            schemasStat.DatabaseId = databaseParameters.DatabaseId;
            UpdateStatisticSchemaData(databaseParameters.DatabaseId, schemasStat);
        }

        private int GetObjectClassNumber(IStorage storage, IDatabaseParameters databaseParameters, Class classStats)
        {
            var classParameter = databaseParameters.Schema.ClassProperties(classStats);
            var objs = storage.GetAll(databaseParameters.DatabaseId);
            return objs.Where(s => s.Properties.All(p => classParameter.Any(cp => cp.PropertyId.Id == p.Key.PropertyId.Id))).Count();
        }
    }
}
