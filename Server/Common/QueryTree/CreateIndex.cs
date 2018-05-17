using MUTDOD.Common;
using MUTDOD.Common.Communication;
using MUTDOD.Common.ModuleBase.Communication;
using MUTDOD.Common.ModuleBase.Storage.Core.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace MUTDOD.Server.Common.QueryTree
{
    [DataContract]
    public class CreateIndex : AbstractComposite
    {
        public CreateIndex() : base(ElementType.CREATE_INDEX)
        {
        }

        public override QueryDTO Execute(QueryParameters parameters)
        {
            var indexedClassReslut = Element(ElementType.CLASS_NAME).Execute(parameters);

            if (indexedClassReslut.Result != null)
            {
                return indexedClassReslut;
            }
            parameters.Subquery = new QueryDTO { QueryClass = indexedClassReslut.QueryClass };

            var indexNameReslut = Element(ElementType.INDEX_NAME).Execute(parameters);

            if (indexNameReslut.Result != null)
            {
                return indexNameReslut;
            }

            var index = parameters.IndexMechanism.GetIndexes().
                        Where(p => p.Value.Equals(indexNameReslut.Value)).Single();

            var attributes = AllElements(ElementType.INDEX_ATTRIBUTE)
                             .Select(p => (string)p.Execute(parameters).Value.Name);
            var indexedAttributes = parameters.IndexMechanism.GetIndexedAttribiutesForType(index.Key, indexedClassReslut.QueryClass.Name);
            if (indexedAttributes != null && indexedAttributes.All(p => attributes.Contains(p)))
            {
                var errorResult = new DTOQueryResult()
                {
                    QueryResultType = ResultType.StringResult,
                    StringOutput = "That attributes has already been indexed"

                };
                return new QueryDTO() { Result = errorResult };
            }

            var classParameter = parameters.Database.Schema.ClassProperties(indexedClassReslut.QueryClass);
            var objs = parameters.Storage.GetAll(parameters.Database.DatabaseId);

            var oids = objs?.Where(s => s.Properties.All(p => classParameter.Any(cp => cp.PropertyId.Id == p.Key.PropertyId.Id)))
                            .Select(obj => obj.Oid);

            parameters.IndexMechanism.IndexObjects(index.Key, oids.ToArray(), attributes.ToArray(), parameters);

            var result = new DTOQueryResult()
            {
                QueryResultType = ResultType.StringResult,
                StringOutput = "Class " + indexedClassReslut.QueryClass.Name + " indexed"
            };
            return new QueryDTO() { Result = result };
        }
    }
}
