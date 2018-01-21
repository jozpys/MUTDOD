using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUTDOD.Common.ModuleBase.Communication;

namespace MUTDOD.Server.Common.QueryTree
{
    public class WhereStatement : AbstractComposite
    {
        public WhereStatement() : base(ElementType.WHERE){}
        public override QueryDTO Execute(QueryParameters parameters)
        {
            return elements.Single().Value.Execute(parameters);
        }
    }
}
