using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using MUTDOD.Common;
using MUTDOD.Common.Communication;
using MUTDOD.Common.ModuleBase.Communication;

namespace MUTDOD.Server.Common.QueryTree
{
    public class IndexName : AbstractLeaf
    {
        [DataMember]
        public String Name { get; set; }

        public IndexName() : base(ElementType.INDEX_NAME)
        {
        }

        public override QueryDTO Execute(QueryParameters parameters)
        {
            if (!parameters.IndexMechanism.GetIndexes().Any(p => p.Value.Equals(Name)))
            {
                DTOQueryResult errorResult = new DTOQueryResult
                {
                    NextResult = null,
                    QueryResultType = ResultType.StringResult,
                    StringOutput = "Unknown index: " + Name
                };
                return new QueryDTO { Result = errorResult };
            }
            return new QueryDTO { Value = Name };
        }

    }
}
