using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUTDOD.Common
{
    public interface IQueryPlanReslult
    {
        ResultType QueryResultType { get; }
        string StringOutput { get; }
        QueryPlan QueryPlan { get; }
    }
}
