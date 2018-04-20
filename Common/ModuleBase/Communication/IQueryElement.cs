using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUTDOD.Common.ModuleBase.Communication
{
    public interface IQueryElement
    {
        QueryDTO Execute(QueryParameters parameters);
        IQueryCompositeElement GetComposite();
        ElementType ElementType { get; }
        int GetCost();
        int GetCardinality();
    }

    public interface IQueryCompositeElement : IQueryElement
    {
        void Add(IQueryElement element);
        IQueryElement Remove(IQueryElement element);
        IDictionary<ElementType, ISet<IQueryElement>> GetElements();
    }

    public interface IQueryLeafElement : IQueryElement
    {
        String GetValue();
    }
}
