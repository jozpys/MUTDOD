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
    public abstract class AbstractElement : IQueryElement
    {
        public AbstractElement(ElementType elementType)
        {
            this.elementType = elementType;
        }
        public abstract QueryDTO Execute(QueryParameters parameters);
        public abstract IQueryCompositeElement GetComposite();

        [DataMember]
        private readonly ElementType elementType;
        public ElementType ElementType => elementType;

        public virtual int GetCost()
        {
            return 0;
        }
        public virtual int GetCardinality()
        {
            return 0;
        }
    }

}
