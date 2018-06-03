using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUTDOD.Common;
using MUTDOD.Common.Communication;
using MUTDOD.Common.ModuleBase.Communication;

namespace MUTDOD.Server.Common.QueryTree.Aggregate
{
    public class Minimum : AbstractComposite
    {
        public Minimum() : base(ElementType.AGGREGATE_FUNCTION) { }

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
                        StringOutput = "Minimum value: " + selectedObjects.Value
                    }
                };
            }

            IEnumerable<Object> values = selectedObjects.Value;
            var minValue = values.Min();

            return new QueryDTO
            {
                Value = minValue,
                Result = new DTOQueryResult
                {
                    NextResult = null,
                    QueryResultType = ResultType.StringResult,
                    StringOutput = "Minimum value: " + minValue
                }
            };
        }
    }
}
