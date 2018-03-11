using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using MUTDOD.Common;
using MUTDOD.Common.Communication;
using MUTDOD.Common.ModuleBase.Communication;
using MUTDOD.Common.ModuleBase.Storage.Core.Metadata;

namespace MUTDOD.Server.Common.QueryTree
{
    [DataContract]
    public class ClassName : AbstractLeaf
    {
        public ClassName() : base(ElementType.CLASS_NAME) { }
        [DataMember]
        public String Name {get; set;}

        public override QueryDTO Execute(QueryParameters parameters)
        {
            if (parameters.Database.Schema == null)
                throw new ApplicationException("Schema can not be null!");
            Class selectedClass = parameters.Database.Schema.Classes.Values.SingleOrDefault(c => c.Name == Name);
            if (selectedClass == null) {
                DTOQueryResult errorResult = new DTOQueryResult
                {
                    NextResult = null,
                    QueryResultType = ResultType.StringResult,
                    StringOutput = "Unknown class: " + Name
                };
                return new QueryDTO { Result = errorResult };
            }
            return new QueryDTO { QueryClass = selectedClass };
        }
    }
}
