using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUTDOD.Common;
using MUTDOD.Common.ModuleBase.Communication;
using MUTDOD.Common.Types;

namespace MUTDOD.Server.Common.QueryTree.Operator
{
    public class LogicalOperatorAnd : AbstractComposite
    {
        public LogicalOperatorAnd() : base(ElementType.WHERE_OPERATION) { }

        public override QueryDTO Execute(QueryParameters parameters)
        {
            ISet<IStorable> resultObjects = new HashSet<IStorable>();
            var operations = AllElements(ElementType.WHERE_OPERATION);
            var left = operations.ElementAt(0);
            var right = operations.ElementAt(1);

            var leftResult = left.Execute(parameters);
            if (leftResult.Result?.QueryResultType == ResultType.StringResult)
            {
                return leftResult;
            }

            var rightResult = right.Execute(parameters);
            if (rightResult.Result?.QueryResultType == ResultType.StringResult)
            {
                return rightResult;
            }

            QueryDTO query = new QueryDTO()
            {
                QueryClass = parameters.Subquery.QueryClass,
                QueryObjects = leftResult.QueryObjects.Intersect(rightResult.QueryObjects)
            };

            return query;
        }
    }
}
