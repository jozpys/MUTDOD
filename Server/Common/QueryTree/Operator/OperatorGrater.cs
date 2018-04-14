using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using MUTDOD.Common.ModuleBase.Communication;

namespace MUTDOD.Server.Common.QueryTree.Operator
{
    [DataContract]
    public class OperatorGrater : AbstractComposite
    {
        public OperatorGrater() : base(ElementType.OPERATOR) { }

        public override QueryDTO Execute(QueryParameters parameters)
        {
            var leftValue = parameters.Subquery.Value;
            var rightValue = parameters.Subquery.AdditionalValue;
            if (leftValue == null || rightValue == null)
            {
                return new QueryDTO { Value = false };
            }
            var left = Expression.Constant(leftValue);
            var right = Expression.Constant(rightValue);
            var greaterThanExpr = Expression.GreaterThan(left, right);
            Boolean result = Expression.Lambda<Func<bool>>(greaterThanExpr).Compile()();
            return new QueryDTO { Value = result };
        }
    }
}
