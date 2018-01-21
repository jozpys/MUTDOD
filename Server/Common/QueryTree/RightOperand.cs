using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUTDOD.Common.ModuleBase.Communication;

namespace MUTDOD.Server.Common.QueryTree
{
    public class RightOperand : AbstractComposite
    {
        public RightOperand() : base(ElementType.RIGHT_OPERAND){}

        public override QueryDTO Execute(QueryParameters parameters)
        {
            return elements.Single().Value.Execute(parameters);
        }
    }
}
