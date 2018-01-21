using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUTDOD.Common.ModuleBase.Communication;

namespace MUTDOD.Server.Common.QueryTree.Literal
{
    public class IntegerLiteral : AbstractLeaf
    {
        public IntegerLiteral() : base(ElementType.LITERAL){}
        public String Value { get; set; }
        public override QueryDTO Execute(QueryParameters parameters)
        {
            int integerValue = Int32.Parse(Value, NumberStyles.Number, CultureInfo.InvariantCulture);
            var literalDto = new QueryDTO { Value = integerValue };
            return literalDto;
        }
    }
}
