using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUTDOD.Common.ModuleBase.Communication;

namespace MUTDOD.Server.Common.QueryTree
{
    public class ArrayValues : AbstractComposite
    {
        public ArrayValues() : base(ElementType.ARRAY) { }

        public override QueryDTO Execute(QueryParameters parameters)
        {
            List<Object> elements = new List<Object>();
            foreach( var valueElement in AllElements(ElementType.LITERAL))
            {
                var literal = valueElement.Execute(parameters).Value;
                elements.Add(literal);
            }

            return new QueryDTO() { Value = elements };
        }
    }
}
