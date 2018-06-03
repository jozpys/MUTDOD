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
    public class Count : AbstractComposite
    {
        public Count() : base(ElementType.AGGREGATE_FUNCTION) { }

        public override QueryDTO Execute(QueryParameters parameters)
        {
            var selectStatement = SingleElement();
            var selectedObjects = selectStatement.Execute(parameters);

            int numberOFObjects = selectedObjects.QueryObjects.Count();

            return new QueryDTO
            {
                Value = numberOFObjects,
                Result = new DTOQueryResult
                {
                    NextResult = null,
                    QueryResultType = ResultType.StringResult,
                    StringOutput = "Number of objects: " + numberOFObjects
                }
            };
        }
    }
}
