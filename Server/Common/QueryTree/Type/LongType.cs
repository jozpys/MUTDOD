using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUTDOD.Common.ModuleBase.Communication;
using MUTDOD.Common.Types;

namespace MUTDOD.Server.Common.QueryTree.Type
{
    public class LongType : AbstractValueType
    {
        public override string PropertyType()
        {
            return Property.LONG;
        }
    }
}
