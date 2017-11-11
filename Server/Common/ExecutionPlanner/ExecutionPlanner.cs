using MUTDOD.Common;
using MUTDOD.Common.ModuleBase;

namespace MUTDOD.Server.Common.ExecutionPlanner
{
    public class ExecutionPlanner : Module, IExecutionPlanner
    {
        public string Name
        {
            get { return "ExecutionPlanner"; }
        }

        public IQueryPlan GenerateQueryPlan(IQueryTree queryTree)
        {
            return new DUMOPQueryPlan {QueryTree = queryTree};
        }

        internal class DUMOPQueryPlan : IQueryPlan
        {
            public IQueryTree QueryTree { get; internal set; }
        }
    }
}
