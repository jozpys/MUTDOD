using System;
using System.ServiceModel;
using MUTDOD.Common;
using MUTDOD.Common.Communication;
using MUTDOD.Common.ModuleBase;
using System.Linq;
using MUTDOD.Common.ModuleBase.Communication;
using MUTDOD.Server.Common.CoreModule.Communication;
using MUTDOD.Common.Settings;

namespace MUTDOD.Server.DataServer.DSODBC
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class DataServerConnector : IDataServerContract
    {
        private readonly ISettingsManager _settingsManager;
        protected readonly IStorage _storage;
        private readonly ILogger _logger;

        public DataServerConnector(ISettingsManager settingsManager, IStorage storage, ILogger logger)
        {
            _settingsManager = settingsManager;
            _storage = storage;
            _logger = logger;
        }

        public DTOQueryResult ExecuteQuery(DatabaseInfo dbName, DTOQueryTree queryTree)
        {
           /*
            var db = _storage.GetDatabases().SingleOrDefault(d => d.Name == dbName.Name);
            QueryParameters parameters = new QueryParameters { Database = db, Storage = _storage, SettingsManager = _settingsManager, Log = (s, level) => _logger.Log(dbName.Name, s, level) };
            QueryDTO result = queryTree.QueryTree.Execute(parameters);
            return result.Result;
            */
            _logger.Log("DataServerConnector", dbName + " " + queryTree, MessageLevel.QueryExecution);
            return new DTOQueryResult
            {
                StringOutput = string.Format("Not implemented, but I get your query: {0}", queryTree),
                QueryResultType = ResultType.StringResult
            };
            
        }

        public bool IsAvailable()
        {
            return true;
        }
    }
}
