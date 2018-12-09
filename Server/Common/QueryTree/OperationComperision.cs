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
    public class OperationComperision : AbstractComposite
    {
        public OperationComperision() : base(ElementType.WHERE_OPERATION){}
        public override QueryDTO Execute(QueryParameters parameters)
        {
            IQueryElement leftElement = Element(ElementType.LEFT_OPERAND);
            IQueryElement rightElement = Element(ElementType.RIGHT_OPERAND);
            IQueryElement operation = Element(ElementType.OPERATOR);
            Func<IStorable, bool> expression = delegate (IStorable databaseObject)
            {
                QueryDTO subquery = new QueryDTO { QueryClass = parameters.Subquery.QueryClass, QueryObjects = new List<IStorable> { databaseObject } };
                QueryParameters singleParameter = new QueryParameters { Database = parameters.Database, Storage = parameters.Storage, Subquery = subquery };
                QueryDTO leftResult = leftElement.Execute(singleParameter);
                if(leftResult.Result?.IsError == true)
                {
                    throw new SelectExecutionException(leftResult);
                }
                QueryDTO rightResult = rightElement.Execute(singleParameter);
                if(rightResult.Result?.IsError == true)
                {
                    throw new SelectExecutionException(rightResult);
                }
                if(leftResult.Result?.QueryResultType == ResultType.ReferencesOnly && rightResult.Result?.QueryResultType == ResultType.ReferencesOnly)
                {
                    bool containsCommonObject = leftResult.QueryObjects.Any(x => rightResult.QueryObjects.Contains(x));
                    return containsCommonObject;
                }
                QueryDTO comparisionSubquery = new QueryDTO { Value = leftResult.Value, AdditionalValue = rightResult.Value };
                QueryParameters comparisionParameter = new QueryParameters { Subquery = comparisionSubquery };
                return (Boolean)operation.Execute(comparisionParameter).Value;
            };

            try
            {
                IEnumerable<IStorable> objects = parameters.Subquery.QueryObjects;

                QueryDTO query = new QueryDTO()
                {
                    QueryClass = parameters.Subquery.QueryClass,
                    QueryObjects = objects.Where(obj => expression(obj)).ToList()
                };

                return query;
            }
            catch(NoClassPropertyException exc)
            {
                DTOQueryResult errorResult = new DTOQueryResult
                {
                    NextResult = null,
                    QueryResultType = ResultType.StringResult,
                    IsError = true,
                    StringOutput = "Unknown propertyName: " + exc.PropertyName
                };
                return new QueryDTO { Result = errorResult };
            }
            catch(SelectExecutionException exc)
            {
                var errorDto = exc.ErrorDto;
                return errorDto;
            }
            catch (InvalidOperationException exc)
            {
                DTOQueryResult errorResult = new DTOQueryResult
                {
                    NextResult = null,
                    QueryResultType = ResultType.StringResult,
                    StringOutput = "Can't compare diferent types!"
                };
                return new QueryDTO { Result = errorResult };
            }
        }
    }
}
