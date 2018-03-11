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
    public abstract class AbstractComposite : AbstractElement, IQueryCompositeElement
    {
        public AbstractComposite(ElementType ElementType) : base(ElementType) { }

        [DataMember]
        protected IDictionary<ElementType, IQueryElement> elements = new Dictionary<ElementType, IQueryElement>();


        public override IQueryCompositeElement GetComposite()
        {
            return this;
        }

        public void Add(IQueryElement element)
        {
            elements.Add(element.ElementType, element);
        }

        public IQueryElement Remove(IQueryElement element)
        {
            Boolean exists = elements.Remove(element.ElementType);
            if (exists)
            {
                return element;
            }
            return null;
        }

        public override string ToString()
        {
            return ElementType + " (" + String.Join(", ", elements.Values) + ")";
        }
    }
}
