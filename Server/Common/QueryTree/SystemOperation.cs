using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUTDOD.Common;
using MUTDOD.Common.ModuleBase.Communication;

namespace MUTDOD.Server.Common.EBNFQueryAnalyzer
{
    public class SystemOperation : AbstractComposite
    {
        public SystemOperation() : base(ElementType.SYSTEM_OPERATION) { }
        public override QueryDTO Execute(QueryParameters parameters)
        {
            return elements.Values.Single().Execute(parameters);
        }
    }
}
