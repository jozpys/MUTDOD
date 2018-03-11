using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using MUTDOD.Common;
using MUTDOD.Common.ModuleBase.Communication;

namespace MUTDOD.Server.Common.QueryTree
{
    [DataContract]
    public abstract class AbstractLeaf : AbstractElement, IQueryLeafElement
    {
        public AbstractLeaf(ElementType elementType) : base(elementType) { }
        public override IQueryCompositeElement GetComposite()
        {
            return null;
        }

        public override string ToString()
        {
            return ElementType.ToString();
        }
    }
}
