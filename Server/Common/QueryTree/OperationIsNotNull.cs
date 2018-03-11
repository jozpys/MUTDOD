using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using MUTDOD.Common;
using MUTDOD.Common.Communication;
using MUTDOD.Common.ModuleBase.Communication;
using MUTDOD.Common.Types;

namespace MUTDOD.Server.Common.QueryTree
{
    [DataContract]
    public class OperationIsNotNull : AbstractComposite
    {
        public OperationIsNotNull() : base(ElementType.IS_NOT_NULL){}
        public override QueryDTO Execute(QueryParameters parameters)
        {
            IQueryElement valueElement = elements.Single().Value;
            Func<IStorable, bool> expression = delegate (IStorable databaseObject)
            {
                QueryDTO subquery = new QueryDTO { QueryObjects = new List<IStorable> { databaseObject } };
                QueryParameters singleParameter = new QueryParameters { Subquery = subquery };
                return valueElement.Execute(singleParameter).Value != null;
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
