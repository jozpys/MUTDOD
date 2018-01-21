using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUTDOD.Common;
using MUTDOD.Common.ModuleBase.Communication;

namespace MUTDOD.Server.Common.QueryTree
{
    public abstract class AbstractElement : IQueryElement
    {
        public AbstractElement(ElementType elementType)
        {
            this.elementType = elementType;
        }
        public abstract QueryDTO Execute(QueryParameters parameters);
        public abstract IQueryCompositeElement GetComposite();

        private readonly ElementType elementType;
        public ElementType ElementType => elementType;
    }

}
