using System;
using System.ServiceModel;
using MUTDOD.Common;
using MUTDOD.Common.Communication;
using MUTDOD.Common.ModuleBase.Communication;

namespace MUTDOD.Server.DataServer.DSODBC
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class DataServerConnector : IDataServerContract
    {
        private readonly ILogger _logger;

        public DataServerConnector(ILogger logger)
        {
            _logger = logger;
        }

        public DTOQueryResult ExecuteQuery(DatabaseInfo dbName, DTOQueryTree queryTree)
        {
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
