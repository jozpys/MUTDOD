using MUTDOD.Common.ModuleBase.Storage.Core.Metadata;
using MUTDOD.Common.Types;
using MUTDOD.Server.Common.ServerStats.StatsStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static ServerSchemaStatsManager GetInstance()
        {
            return _instance ?? (_instance = new ServerSchemaStatsManager());
        }

        private ServerSchemaStatsManager()
        {
            schemasStats = new List<SchemaStats>();
            ServerSchemaStatsStorageManager.GetStatisticDataAllSchemas().ForEach(p => schemasStats.Add((SchemaStats)p));
        }
        internal int GetClassObjectNumber(Did databaseId, ClassId classId)
        {
            var schemaStats = schemasStats.Find(p => p.DatabaseId.Equals(databaseId));
            if (schemaStats != null)
                return schemaStats.ClassStats.objectNumbers.TryGetValue(classId, out int number) ? number : 0;
            else
                return 0;

        }
        internal void UpdateStatisticSchemaData(Did DatabaseId, SchemaStats schemaStats)
        {
            ServerSchemaStatsStorageManager.UpdateStatisticSchemaData(DatabaseId, schemaStats);
        }
    }
}
