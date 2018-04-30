﻿using MUTDOD.Common.ModuleBase.Communication;

namespace MUTDOD.Common.ModuleBase
{
    public interface IQueryOptimizer : IModule
    {
        IQueryElement OptimizeQueryPlan(IQueryElement queryTree, QueryParameters queryParameters);
        QueryPlan GetQueryPlan(IQueryElement queryTree, QueryParameters queryParameters);
    }
}
