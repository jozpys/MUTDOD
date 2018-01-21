using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MUTDOD.Common.ModuleBase.Communication;

namespace MUTDOD.Server.Common.QueryTree.Operator
{
    public class OperatorNotEqual : AbstractComposite
    {
        public OperatorNotEqual() : base(ElementType.OPERATOR) { }

        public override QueryDTO Execute(QueryParameters parameters)
        {
            var left = Expression.Constant(parameters.Subquery.Value);
            var right = Expression.Constant(parameters.Subquery.AdditionalValue);
            var greaterThanExpr = Expression.NotEqual(left, right);
            Boolean result = Expression.Lambda<Func<bool>>(greaterThanExpr).Compile()();
            return new QueryDTO { Value = result };
        }
    }
}
