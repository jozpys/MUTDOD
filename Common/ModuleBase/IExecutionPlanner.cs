namespace MUTDOD.Common.ModuleBase
{
    public interface IExecutionPlanner : IModule
    {
        IQueryPlan GenerateQueryPlan(IQueryTree queryTree);
    }
}
