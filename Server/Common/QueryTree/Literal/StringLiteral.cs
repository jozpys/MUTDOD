using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using MUTDOD.Common.ModuleBase.Communication;

namespace MUTDOD.Server.Common.QueryTree.Literal
{
    [DataContract]
    public class StringLiteral : AbstractLeaf
    {
        public StringLiteral() : base(ElementType.LITERAL){}
        [DataMember]
        public String Value { get; set; }
        public override QueryDTO Execute(QueryParameters parameters)
        {
            var literalDto = new QueryDTO { Value = Value };
            return literalDto;
        }
    }
}
