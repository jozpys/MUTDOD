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
        private IDictionary<ElementType, ISet<IQueryElement>> elements = new Dictionary<ElementType, ISet<IQueryElement>>();


        public override IQueryCompositeElement GetComposite()
        {
            return this;
        }

        public IEnumerable<IQueryElement> AllElements(ElementType elementType)
        {
            if (!elements.ContainsKey(elementType))
            {
                return Enumerable.Empty<IQueryElement>();
            }
            return elements[elementType];
        }

        public IQueryElement Element(ElementType elementType)
        {
            return elements[elementType].Single();
        }

        public IQueryElement SingleElement()
        {
            return elements.Single().Value.Single();
        }

        public Boolean TryGetElement(ElementType elementType, out IQueryElement searchedElement)
        {
            if (elements.ContainsKey(elementType))
            {
                var typeElements = elements[elementType];
                if(typeElements.Take(2).Count() == 1)
                {
                    searchedElement = typeElements.Single();
                    return true;
                }
            }
            searchedElement = null;
            return false;
        }

        public void Add(IQueryElement element)
        {
            if (!elements.ContainsKey(element.ElementType))
            {
                elements.Add(element.ElementType, new HashSet<IQueryElement>());
            }
            elements[element.ElementType].Add(element);
        }

        public IQueryElement Remove(IQueryElement element)
        {
            if (elements.ContainsKey(element.ElementType))
            {
                Boolean exists = elements[element.ElementType].Remove(element);
                if (exists)
                {
                    return element;
                }
            }
            return null;
        }

        public IDictionary<ElementType, ISet<IQueryElement>> GetElements()
        {
            return elements;
        }
        
        public override string ToString()
        {
            return ElementType + " (" + String.Join(", ", elements.Values) + ")";
        }

    }
}
