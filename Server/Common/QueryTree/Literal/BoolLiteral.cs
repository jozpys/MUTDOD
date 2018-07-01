using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using MUTDOD.Common.ModuleBase.Communication;
using MUTDOD.Common.Types;

namespace MUTDOD.Server.Common.QueryTree.Literal
{
    [DataContract]
    public class BoolLiteral : AbstractLeaf
    {
        public BoolLiteral() : base(ElementType.LITERAL){}
        [DataMember]
        public String Value { get; set; }
        public override QueryDTO Execute(QueryParameters parameters)
        {
            Boolean convertedValue = ConvertValue(Value);
            var literalDto = new QueryDTO { Value = convertedValue, AdditionalValue = Property.BOOL };
            return literalDto;
        }

        private Boolean ConvertValue(String value)
        {
            if (value.ToUpper() == "TRUE")
                return true;
            else if (value.ToUpper() == "FALSE")
                return false;
            else
                throw new ApplicationException("Unknown BOOL_VALUE token value: " + value);
        }
    }
}
