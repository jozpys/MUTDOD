using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using MUTDOD.Common.ModuleBase.Communication;

namespace MUTDOD.Server.Common.QueryTree
{
    [DataContract]
    public class WhereStatement : AbstractComposite
    {
        public WhereStatement() : base(ElementType.WHERE){}

        public override QueryDTO Execute(QueryParameters parameters)
        {
            return SingleElement().Execute(parameters);
        }
    }
}
