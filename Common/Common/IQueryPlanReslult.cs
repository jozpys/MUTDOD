using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUTDOD.Common
{
    public class IQueryPlanReslult
    {
        public ResultType QueryResultType { get; }
        public string StringOutput { get; }
        public QueryPlan QueryPlan {get;}
    }
}
