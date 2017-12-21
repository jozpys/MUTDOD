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
    }

    public interface IQueryCompositeElement : IQueryElement
    {
        void Add(IQueryElement element);
        IQueryElement Remove(IQueryElement element);
    }

    public interface IQueryLeafElement : IQueryElement
    {

    }
}
