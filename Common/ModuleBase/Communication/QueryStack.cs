using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUTDOD.Common.ModuleBase.Communication
{
    public class QueryStack : Stack<Scope>
    {
        public IQueryElement FindSingleElementOnPeek(ElementType elementType)
        {
            return Peek().Where(p => ElementType.CLASS_NAME.Equals(p.ElementType))
                                            .Single();
        }

        public Boolean HasElementOnPeek(ElementType elementType)
        {
            return Count > 0 ? Peek().Any(p => elementType.Equals(p.ElementType)) : false;
        }

        public IQueryElement FindLastAncestorOnPeekMatch(Predicate<IQueryElement> predicate)
        {
            return Peek().FindLast(predicate);
        }
        
        public IQueryElement FindLastAncestorOnPeekMatchType(ElementType elementType)
        {
            return Peek().FindLast(p => p.ElementType.Equals(elementType));
        }

        public void AddElement(IQueryElement element)
        {
            if (element.IsOpenStackScope)
                Push(new Scope { element });
            else if (Count > 0)
                Peek().Add(element);
            else
                Push(new Scope { element });
        }
    }
}
