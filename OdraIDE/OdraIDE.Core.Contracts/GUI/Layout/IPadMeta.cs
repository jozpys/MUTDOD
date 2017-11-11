using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

namespace OdraIDE.Core
{
    public interface IPadMeta : ILayoutItemMeta
    {
    }

    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class PadAttribute : LayoutItemAttribute
    {
        public PadAttribute() : base(typeof(IPad)) { }
    }
}
