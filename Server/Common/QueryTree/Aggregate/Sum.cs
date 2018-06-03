using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUTDOD.Common;
using MUTDOD.Common.Communication;
using MUTDOD.Common.ModuleBase.Communication;
using MUTDOD.Common.Types;

namespace MUTDOD.Server.Common.QueryTree.Aggregate
{
    public class Sum : AbstractComposite
    {
        public Sum() : base(ElementType.AGGREGATE_FUNCTION) { }

        public override QueryDTO Execute(QueryParameters parameters)
        {
            var selectStatement = SingleElement();
            var selectedObjects = selectStatement.Execute(parameters);

            if (selectedObjects.Value == null)
            {
                return new QueryDTO
                {
                    Result = new DTOQueryResult
                    {
                        NextResult = null,
                        QueryResultType = ResultType.StringResult,
                        StringOutput = "Can't aggregate over objects."
                    }
                };
            }

            Property property = selectedObjects.AdditionalValue;
            if (!property.IsNumericType)
            {
                return new QueryDTO
                {
                    Result = new DTOQueryResult
                    {
                        NextResult = null,
                        QueryResultType = ResultType.StringResult,
                        StringOutput = "Can't sum non numeric type."
                    }
                };
            }

            var typeOfValue = selectedObjects.Value.GetType();
            if (!typeof(IEnumerable<Object>).IsAssignableFrom(typeOfValue))
            {
                return new QueryDTO
                {
                    Value = selectedObjects.Value,
                    Result = new DTOQueryResult
                    {
                        NextResult = null,
                        QueryResultType = ResultType.StringResult,
                        StringOutput = "Sum value: " + selectedObjects.Value
                    }
                };
            }

            IEnumerable<Object> values = selectedObjects.Value;
            var sumValue = values.Select(s => Convert.ToDouble(s.ToString())).Sum();

            return new QueryDTO
            {
                Value = sumValue,
                Result = new DTOQueryResult
                {
                    NextResult = null,
                    QueryResultType = ResultType.StringResult,
                    StringOutput = "Sum value: " + sumValue
                }
            };
        }
    }
}
