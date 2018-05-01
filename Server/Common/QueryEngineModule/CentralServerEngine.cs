using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUTDOD.Common;
using MUTDOD.Common.Communication;
using MUTDOD.Common.ModuleBase;
using MUTDOD.Common.ModuleBase.Communication;
using MUTDOD.Common.ModuleBase.Storage.Core.Metadata;
using MUTDOD.Common.Settings;
using MUTDOD.Common.Types;
using MUTDOD.Server.Common.IndexMechanism;
using MUTDOD.Server.Common.QueryEngineModule.Core;

namespace MUTDOD.Server.Common.QueryEngineModule
{
    public class CentralServerEngine : BaseEngine, ICentralServerEngine
    {
        private readonly IQueryAnalyzer _queryAnalyzer;
        private readonly ISettingsManager _settingsManager;
        private IIndexMechanism<string> _indexMechanism;
        private IServerSchemaStats _serverSchemaStats;

        public CentralServerEngine(IQueryAnalyzer queryAnalyzer, IQueryOptimizer queryOptimizer, IStorage storage,
            ISettingsManager settingsManager, ILogger logger, IIndexMechanism<string> indexMechanism, IServerSchemaStats serverSchemaStats) : base(queryOptimizer, storage, logger)
        {
            _queryAnalyzer = queryAnalyzer;
            _settingsManager = settingsManager;
            _indexMechanism = indexMechanism;
            _serverSchemaStats = serverSchemaStats;
        }

        public override string Name
        {
            get { return "CentralServerEngine"; }
        }

        public IQueryResult Execute(string dbName, IQueryElement queryTree)
        {
            throw new NotImplementedException();
        }

        public IQueryResult Execute(string dbName, IQuery query, SystemInfo systemInfo, Action<IQueryElement> doOnDataServers)
        {
            try
            {
                var db = _storage.GetDatabases().SingleOrDefault(d => d.Name == dbName);
                var schema = db == null ? new EmptyDatabaseSchema() : db.Schema;
                var queryTree = _queryAnalyzer.ParseQuery(query);
                Action<string, MessageLevel> logger = (s, level) => _logger.Log(Name, s, level);
                QueryParameters parameters = new QueryParameters { Database = db, SystemInfo = systemInfo, Storage = _storage, SettingsManager = _settingsManager, Log = logger, IndexMechanism = _indexMechanism, ServerSchemaStats = _serverSchemaStats };
                queryTree = _queryOptimizer.OptimizeQueryPlan(queryTree, parameters);
                var executer = new CentralServerExecuter(db, doOnDataServers, systemInfo, _storage, _settingsManager, logger, _indexMechanism);
                return executer.Execute(queryTree);
            }
            catch (QuerySyntaxException ex)
            {
                return new DTOQueryResult()
                {
                    QueryResultType = ResultType.StringResult,
                    StringOutput = "SYNTAX ERROR:\n" + ex.Message
                };
            }
            catch (QuerySemanticException ex)
            {
                var sb = new StringBuilder();
                sb.AppendLine("SEMANTIC ERROR:");
                foreach (var exceptionItem in ex.ExceptionList)
                    sb.AppendLine(exceptionItem.message);

                return new DTOQueryResult() { QueryResultType = ResultType.StringResult, StringOutput = sb.ToString() };
            }
            catch (Exception ex)
            {
                return new DTOQueryResult() { QueryResultType = ResultType.StringResult, StringOutput = ex.ToString() };
            }
        }

        public IQueryPlanReslult GetQueryPlan(string dbName, IQuery query)
        {
            try
            {
                var db = _storage.GetDatabases().SingleOrDefault(d => d.Name == dbName);
                var schema = db == null ? new EmptyDatabaseSchema() : db.Schema;
                var queryTree = _queryAnalyzer.ParseQuery(query);
                Action<string, MessageLevel> logger = (s, level) => _logger.Log(Name, s, level);
                QueryParameters parameters = new QueryParameters { Database = db, Storage = _storage, SettingsManager = _settingsManager, Log = logger, IndexMechanism = _indexMechanism, ServerSchemaStats = _serverSchemaStats };
                return new DTOQueryPlanResult() { QueryPlan = _queryOptimizer.GetQueryPlan(queryTree, parameters) };
            }
            catch (QuerySyntaxException ex)
            {
                return new DTOQueryPlanResult()
                {
                    QueryResultType = ResultType.StringResult,
                    StringOutput = "SYNTAX ERROR:\n" + ex.Message
                };
            }
            catch (QuerySemanticException ex)
            {
                var sb = new StringBuilder();
                sb.AppendLine("SEMANTIC ERROR:");
                foreach (var exceptionItem in ex.ExceptionList)
                    sb.AppendLine(exceptionItem.message);

                return new DTOQueryPlanResult() { QueryResultType = ResultType.StringResult, StringOutput = sb.ToString() };
            }
            catch (Exception ex)
            {
                return new DTOQueryPlanResult() { QueryResultType = ResultType.StringResult, StringOutput = ex.ToString() };
            }
        }

        private class EmptyDatabaseSchema : IDatabaseSchema
        {
            public EmptyDatabaseSchema()
            {
                DatabaseId = Did.CreateNew();
                Classes = new ConcurrentDictionary<ClassId, Class>();
                Properties = new ConcurrentDictionary<PropertyId, Property>();
                Methods = new ConcurrentDictionary<ClassId, List<string>>();
            }
            public Did DatabaseId { get; set; }
            public ConcurrentDictionary<ClassId, Class> Classes { get; set; }
            public ConcurrentDictionary<PropertyId, Property> Properties { get; set; }
            public ConcurrentDictionary<ClassId, List<string>> Methods { get; set; }

            public List<Property> ClassProperties(Class className)
            {
                return Enumerable.Empty<Property>().ToList();
            }
        }
    }
}
