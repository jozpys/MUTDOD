using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MUTDOD.Common;
using MUTDOD.Common.Communication;
using MUTDOD.Common.ModuleBase.Communication;
using MUTDOD.Common.Types;

namespace MUTDOD.Server.Common.QueryTree
{
    public class OperationComperision : AbstractComposite
    {
        public OperationComperision() : base(ElementType.COMPERISION){}
        public override QueryDTO Execute(QueryParameters parameters)
        {
            IQueryElement leftElement = elements[ElementType.LEFT_OPERAND];
            IQueryElement rightElement = elements[ElementType.RIGHT_OPERAND];
            IQueryElement operation = elements[ElementType.OPERATOR];
            Func<IStorable, bool> expression = delegate (IStorable databaseObject)
            {
                QueryDTO subquery = new QueryDTO { QueryObjects = new List<IStorable> { databaseObject } };
                QueryParameters singleParameter = new QueryParameters { Subquery = subquery };
                var left = leftElement.Execute(singleParameter).Value;
                var right = rightElement.Execute(singleParameter).Value;
                QueryDTO comparisionSubquery = new QueryDTO { Value = left, AdditionalValue = right };
                QueryParameters comparisionParameter = new QueryParameters { Subquery = comparisionSubquery };
                return (Boolean)operation.Execute(comparisionParameter).Value;
            };

            try
            {
                QueryDTO query = parameters.Subquery;
                IEnumerable<IStorable> objects = query.QueryObjects;
                query.QueryObjects = objects.Where(obj => expression(obj)).ToList();
                return query;
            }
            catch(NoClassPropertyException exc)
            {
                DTOQueryResult errorResult = new DTOQueryResult
                {
                    NextResult = null,
                    QueryResultType = ResultType.StringResult,
                    StringOutput = "Unknown propertyName: " + exc.PropertyName
                };
                return new QueryDTO { Result = errorResult };
            }
        }
    }
}
