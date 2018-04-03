using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUTDOD.Common.ModuleBase.Communication;

namespace MUTDOD.Server.Common.QueryTree.Type
{
    public abstract class AbstractValueType : AbstractLeaf
    {
        public AbstractValueType() : base(ElementType.DATA_TYPE) { }

        public override QueryDTO Execute(QueryParameters parameters)
        {
            var dataTypeDto = new QueryDTO { Value = PropertyType(), AdditionalValue = true };
            return dataTypeDto;
        }

        public abstract String PropertyType();
    }
}
