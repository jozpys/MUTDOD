using System;
using System.Linq;
using MUTDOD.Common;
using MUTDOD.Common.Communication;
using MUTDOD.Common.ModuleBase;
using MUTDOD.Common.ModuleBase.Communication;

namespace MUTDOD.Server.Common.QueryEngineModule.Core
{
    public abstract class BaseEngine : Module
    {
        public BaseEngine(IQueryOptimizer queryOptimizer, IStorage storage, ILogger logger)
        {
            _queryOptimizer = queryOptimizer;
            _storage = storage;
            _logger = logger;
        }

        protected readonly IQueryOptimizer _queryOptimizer;
        protected readonly IStorage _storage;
        protected readonly ILogger _logger;

        public IQueryResult Execute(string dbName, IQueryElement queryTree)
        {
            try
            {
                var db = _storage.GetDatabases().SingleOrDefault(d => d.Name == dbName);
                if (db == null)
                    return new DTOQueryResult()
                    {
                        QueryResultType = ResultType.StringResult,
                        StringOutput = "Database named '" + dbName + "' not exists!"
                    };
                
                queryTree = _queryOptimizer.OptimizeQueryPlan(queryTree, new QueryParameters());
                var executer = new EngineExecuter(db, _storage,
                    (s, level) => _logger.Log(Name, s, level));
                return executer.Execute(queryTree);

            }
            catch (Exception ex)
            {
                return new DTOQueryResult() {QueryResultType = ResultType.StringResult, StringOutput = ex.ToString()};
            }
        }

        public abstract string Name { get; }
    }
}
