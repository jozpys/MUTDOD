using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUTDOD.Common.ModuleBase.Communication;

namespace MUTDOD.Server.Common.QueryTree.Literal
{
    public class BoolLiteral : AbstractLeaf
    {
        public BoolLiteral() : base(ElementType.LITERAL){}
        public String Value { get; set; }
        public override QueryDTO Execute(QueryParameters parameters)
        {
            Boolean convertedValue = ConvertValue(Value);
            var literalDto = new QueryDTO { Value = convertedValue };
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
