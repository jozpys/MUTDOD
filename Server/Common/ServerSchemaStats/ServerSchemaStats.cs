using MUTDOD.Common;
using MUTDOD.Common.ModuleBase;
using MUTDOD.Common.ModuleBase.Storage.Core.Metadata;
using MUTDOD.Common.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUTDOD.Server.Common.ServerStats
{
    public class ServerSchemaStats : Module, IServerSchemaStats
    {

        private static ILogger _logger = null;
        private static IStorage _storage = null;

        public ServerSchemaStats(ILogger logger, IStorage storage)
        {
            _logger = logger;
            _storage = storage;
        }

        internal static ILogger GetLoger()
        {
            return _logger;
        }

        public int GetClassObjectNumber(Did databaseId, ClassId classId)
        {
            return ServerSchemaStatsManager.GetInstance(_storage).GetClassObjectNumber(databaseId, classId);
        }

        public void UpdateStatisticSchemaData(Did databaseId, SchemaStats schemaStats)
        {
            ServerSchemaStatsManager.GetInstance(_storage).UpdateStatisticSchemaData(databaseId, schemaStats);
        }
        public void PropertyValue(Property property, object value)
        {
            throw new NotImplementedException();
        }

        public int PropertyValueNumber(Property property)
        {
            throw new NotImplementedException();
        }

        public bool RecalculateStats()
        {
            return ServerSchemaStatsManager.GetInstance(_storage).RecalculateStats(_storage);
        }

        public string Name
        {
            get
            {
                return "ServerSchemaStats";
            }
        }
    }
}
