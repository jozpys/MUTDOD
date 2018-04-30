using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUTDOD.Common.Communication
{
    public class DTOQueryPlanResult : IQueryPlanReslult
    {
        public ResultType QueryResultType { get; set; }
        public string StringOutput { get; set; }
        public QueryPlan queryPlan { get; set; }
    }
}
