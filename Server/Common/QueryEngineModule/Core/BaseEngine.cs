using System;
using System.Linq;
using MUTDOD.Common;
using MUTDOD.Common.Communication;
using MUTDOD.Common.ModuleBase;

namespace MUTDOD.Server.Common.QueryEngineModule.Core
{
    public abstract class BaseEngine : Module
    {
        public BaseEngine(IExecutionPlanner executionPlanner, IStorage storage, ILogger logger)
        {
            _executionPlanner = executionPlanner;
            _storage = storage;
            _logger = logger;
        }

        protected readonly IExecutionPlanner _executionPlanner;
        protected readonly IStorage _storage;
        protected readonly ILogger _logger;

        public IQueryResult Execute(string dbName, IQueryTree queryTree)
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
                /*
                                if (query.QueryText == InternalQueries.SystemInfoQuery.QueryText)
                                {
                                    StringBuilder sb = new StringBuilder();
                                    StringWriter sw = new StringWriter(sb);
                                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(SystemInfo));
                                    xmlSerializer.Serialize(sw, systemInfo);
                                    return new DTOQueryResult()
                                    {
                                        NextResult = null,
                                        QueryResults = null,
                                        QueryResultType = ResultType.SystemInfo,
                                        StringOutput = sb.ToString()
                                    };
                                }
                                if (query.QueryText == InternalQueries.CreateDatabaseQuery.QueryText)
                                {
                                    try
                                    {
                                        var did = _storage.CreateDatabase(new DatabaseParameters(dbName, _settingsManager));
                                        _logger.Log(Name,string.Format("new database created as {0}", did),MessageLevel.Info);
                                        StringBuilder sb = new StringBuilder();
                                        StringWriter sw = new StringWriter(sb);
                                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(DatabaseInfo));
                                        var db = _storage.GetDatabase(did);
                                        xmlSerializer.Serialize(sw,
                                            new DatabaseInfo()
                                            {
                                                Name = db.Name,
                                                Classes = db.Schema.Classes.Select(c => new DatabaseClass
                                                {
                                                    Name = c.Value.Name,
                                                    Fields = c.Value.Fields,
                                                    Methods = c.Value.Methods
                                                }).ToList()
                                            });
                                        return new DTOQueryResult()
                                        {
                                            NextResult = null,
                                            QueryResults = null,
                                            QueryResultType = ResultType.DatabaseInfo,
                                            StringOutput = sb.ToString()
                                        };

                                    }
                                    catch (Exception ex)
                                    {
                                        return new DTOQueryResult()
                                        {
                                            NextResult = null,
                                            QueryResults = null,
                                            QueryResultType = ResultType.StringResult,
                                            StringOutput = "Error during database creation: " + ex.ToString()
                                        };
                                    }
                                }
                                */
                var executionPlan = _executionPlanner.GenerateQueryPlan(queryTree);
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
