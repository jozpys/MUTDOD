using MUTDOD.Common.ModuleBase.Communication;
using MUTDOD.Common.Types;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MUTDOD.Server.Common.QueryTree.Literal
{
    [DataContract]
    public class FloatLiteral : AbstractLeaf
    {
        public FloatLiteral() : base(ElementType.LITERAL) { }
        [DataMember]
        public String Value { get; set; }
        public override QueryDTO Execute(QueryParameters parameters)
        {
            float floatValue = float.Parse(Value, CultureInfo.InvariantCulture);
            var literalDto = new QueryDTO { Value = floatValue, AdditionalValue = Property.FLOAT };
            return literalDto;
        }
    }
}
