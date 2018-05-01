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
        AccessType AccessType { get; }
        string AccessObject { get; }
        int Cost { get; }
        int Cardinality(QueryParameters parameters);
        string Value { get; }
        Boolean IsOpenStackScope { get; }
        Boolean IsCloseStackScope { get; }
        Boolean Optimize(QueryParameters parameters, QueryStack queryStack);
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
