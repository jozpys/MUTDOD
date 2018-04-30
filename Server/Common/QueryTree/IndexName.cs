using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using MUTDOD.Common;
using MUTDOD.Common.Communication;
using MUTDOD.Common.ModuleBase.Communication;
using MUTDOD.Server.Common.Tools.Logger;

namespace MUTDOD.Server.Common.QueryTree
{
    public class IndexName : AbstractLeaf, ILogger
    {
        [DataMember]
        public String Name { get; set; }

        private IndexMechanism.IndexMechanism im;

        public IndexName() : base(ElementType.INDEX_NAME){
            im = new IndexMechanism.IndexMechanism(this);
        }

        public override QueryDTO Execute(QueryParameters parameters)
        {
            if (!im.GetIndexes().Any(p => p.Value.Equals(Name))){
                DTOQueryResult errorResult = new DTOQueryResult
                    {
                        NextResult = null,
                        QueryResultType = ResultType.StringResult,
                        StringOutput = "Unknown index: " + Name
                    };
                return new QueryDTO { Result = errorResult};
            }
            return new QueryDTO { Value = Name };
        }

        public void Log(string senderName, string message, MessageLevel messageLevel)
        {
            Console.WriteLine("Log TODO");
        }
    }
}
