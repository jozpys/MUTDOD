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
    public class NewObject : AbstractComposite
    {
        public NewObject() : base(ElementType.NEW_OBJECT) { }

        public override QueryDTO Execute(QueryParameters parameters)
        {
            IQueryElement classNameElement = Element(ElementType.CLASS_NAME);
            QueryDTO classResult = classNameElement.Execute(parameters);
            if (classResult.Result != null)
            {
                return classResult;
            }
            var objectClass = classResult.QueryClass;
            if (objectClass.Interface)
            {
                return new QueryDTO
                {
                    Result = new DTOQueryResult()
                    {
                        QueryResultType = ResultType.StringResult,
                        StringOutput = "Can't create object from the interface!"
                    }
                };
            }
            List<Property> propeteries = parameters.Database.Schema.ClassProperties(objectClass);

            var oid = new Oid(Guid.NewGuid(), parameters.Database.DatabaseId.Dli);
            var toStore = new Storable { Oid = oid };
            parameters.Subquery = new QueryDTO { Value = toStore, AdditionalValue = propeteries };

            foreach (var attrToSet in AllElements(ElementType.OBJECT_INITIALIZATION_ELEMENT))
            {
                var attributeResult = attrToSet.Execute(parameters);
                if(attributeResult.Result != null)
                {
                    return attributeResult;
                }
            }

            parameters.Storage.Save(parameters.Database.DatabaseId, toStore);
            parameters.Log("new object saved with id: " + oid, MessageLevel.QueryExecution);
            return new QueryDTO()
            {
                Result = new DTOQueryResult
                {
                    NextResult = null,
                    QueryResultType = ResultType.StringResult,
                    StringOutput = "new object saved with id: " + oid
                }
            };
        }
    }
}
