using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using MUTDOD.Common;
using MUTDOD.Common.Communication;
using MUTDOD.Common.ModuleBase.Communication;
using MUTDOD.Common.ModuleBase.Storage;
using MUTDOD.Common.Types;

namespace MUTDOD.Server.Common.QueryTree
{
    [DataContract]
    public class DeleteObject : AbstractComposite
    {
        public DeleteObject() : base(ElementType.DELETE_OBJECT) { }

        public override QueryDTO Execute(QueryParameters parameters)
        {
            IQueryElement selectElement = Element(ElementType.SELECT);
            QueryDTO selectResult = selectElement.Execute(parameters);
            if (selectResult.Result.QueryResultType == ResultType.StringResult)
            {
                return selectResult;
            }

            IEnumerable<IStorable> objectsToDelete = selectResult.QueryObjects;

            parameters.Storage.Delete(parameters.Database.DatabaseId, objectsToDelete);
            parameters.Log("Objects deleted with ids: " + String.Join(", ", objectsToDelete.Select( obj => obj.Oid )), MessageLevel.QueryExecution);
            return new QueryDTO()
            {
                Result = new DTOQueryResult
                {
                    NextResult = null,
                    QueryResultType = ResultType.StringResult,
                    StringOutput = "Objects deleted with ids: " + String.Join(", ", objectsToDelete.Select(obj => obj.Oid))
                }
            };
        }
    }
}
