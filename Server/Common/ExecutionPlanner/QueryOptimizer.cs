using MUTDOD.Common;
using MUTDOD.Common.ModuleBase;
using MUTDOD.Common.ModuleBase.Communication;

namespace MUTDOD.Server.Common.DummyQueryOptimizer
{
    public class QueryOptimizer : Module, IQueryOptimizer
    {
        public string Name
        {
            get { return "QueryOptimizer"; }
        }

        public IQueryElement OptimizeQueryPlan(IQueryElement queryTree)
        {
            return queryTree;
        }
    }
}
