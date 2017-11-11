using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

namespace OdraIDE.Core
{
    public interface ILayoutItemMeta
    {
        string Name { get; }
    }

    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class LayoutItemAttribute : ExportAttribute
    {
        public LayoutItemAttribute() : base(typeof(ILayoutItem)) { }
        public LayoutItemAttribute(Type contractType) : base(contractType) { }
        public string Name { get; set; }
    }
}
