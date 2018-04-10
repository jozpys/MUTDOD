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
    public class UpdateObject : AbstractComposite
    {
        public UpdateObject() : base(ElementType.UPDATE_OBJECT) { }

        public override QueryDTO Execute(QueryParameters parameters)
        {
            IQueryElement selectElement = Element(ElementType.SELECT);
            QueryDTO selectResult = selectElement.Execute(parameters);
            if (selectResult.Result.QueryResultType == ResultType.StringResult)
            {
                return selectResult;
            }

            List<Property> propeteries = parameters.Database.Schema.ClassProperties(selectResult.QueryClass);
            ISet<IStorable> updatedObjects = new HashSet<IStorable>();

            foreach ( var obj in selectResult.QueryObjects)
            {
                parameters.Subquery = new QueryDTO { Value = obj, AdditionalValue = propeteries };

                foreach (var attrToSet in AllElements(ElementType.OBJECT_UPDATE_ELEMENT))
                {
                    var attributeResult = attrToSet.Execute(parameters);
                    if (attributeResult.Result != null)
                    {
                        return attributeResult;
                    }
                }

                updatedObjects.Add(obj);
            }

            parameters.Storage.Save(parameters.Database.DatabaseId, updatedObjects);
            parameters.Log("Objects update with ids: " + String.Join(", ", updatedObjects.Select( obj => obj.Oid )), MessageLevel.QueryExecution);
            return new QueryDTO()
            {
                Result = new DTOQueryResult
                {
                    NextResult = null,
                    QueryResultType = ResultType.StringResult,
                    StringOutput = "Objects update with ids: " + String.Join(", ", updatedObjects.Select(obj => obj.Oid))
                }
            };
        }
    }
}
