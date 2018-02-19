using MUTDOD.Common;
using MUTDOD.Common.ModuleBase;
using MUTDOD.Common.Settings;
using MUTDOD.Server.Common.QueryEngineModule.Core;

namespace MUTDOD.Server.Common.QueryEngineModule
{
    public class QueryEngine : BaseEngine, IQueryEngine
    {
        private readonly IQueryAnalyzer _queryAnalyzer;
        private readonly ISettingsManager _settingsManager;

        public QueryEngine(IQueryAnalyzer queryAnalyzer, IQueryOptimizer queryOptimizer,IStorage storage, ISettingsManager settingsManager, ILogger logger) 
            : base(queryOptimizer, storage,logger)
        {
            _queryAnalyzer = queryAnalyzer;
            _settingsManager = settingsManager;
        }

        public override string Name
        {
            get { return Constant.Name; }
        }

        private static class Constant
        {
            public const string Name = "QueryEngine";
        }
    }
}