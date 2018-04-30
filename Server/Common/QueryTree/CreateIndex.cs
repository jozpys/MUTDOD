using MUTDOD.Common.ModuleBase.Communication;
using MUTDOD.Common.ModuleBase.Storage.Core.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using MUTDOD.Server.Common.IndexMechanism;
using MUTDOD.Common;
using MUTDOD.Common.Types;
using MUTDOD.Common.Communication;

namespace MUTDOD.Server.Common.QueryTree
{
    [DataContract]
    public class CreateIndex : AbstractComposite, ILogger
    {
        public CreateIndex() : base(ElementType.CREATE_INDEX) {
        }

        public override QueryDTO Execute(QueryParameters parameters)
        {
            Class indexedClass = Element(ElementType.CLASS_NAME).Execute(parameters).QueryClass;
            parameters.Subquery = new QueryDTO { QueryClass = indexedClass};
            var attributes = AllElements(ElementType.INDEX_ATTRIBUTE)
                             .Select(p =>(string) p.Execute(parameters).Value.Name);

            var classParameter = parameters.Database.Schema.ClassProperties(indexedClass);
            var objs = parameters.Storage.GetAll(parameters.Database.DatabaseId);
            var oids = objs.Where(s => s.Properties.All(p => classParameter.Any(cp => cp.PropertyId.Id == p.Key.PropertyId.Id)))
                           .Select(obj => obj.Oid);

            var indexName = (string)Element(ElementType.INDEX_NAME).Execute(parameters).Value;
            var indexId = parameters.IndexMechanism.GetIndexes().Where(p => p.Value.Equals(indexName)).Single().Key;

            parameters.IndexMechanism.IndexObjects(indexId, oids.ToArray(), attributes.ToArray(), parameters);

            var result = new DTOQueryResult()
            {
                QueryResultType = ResultType.StringResult,
                StringOutput = "Class " + indexedClass.Name + " indexed"
            };
            return new QueryDTO() { Result = result };
        }

        public void Log(string senderName, string message, MessageLevel messageLevel)
        {
            throw new NotImplementedException();
        }
    }
}
