using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUTDOD.Common.Communication
{
    [Serializable]
    public class DTOQueryPlanResult : IQueryPlanReslult
    {

        public ResultType QueryResultType { get; set; }
        public string StringOutput { get; set; }
        public QueryPlan QueryPlan { get; set; }

        public DTOQueryPlanResult()
        {
        }

        public DTOQueryPlanResult(IQueryPlanReslult query)
        {
            QueryResultType = query.QueryResultType;
            StringOutput = query.StringOutput;
            QueryPlan = query.QueryPlan;

        }

    }
}
