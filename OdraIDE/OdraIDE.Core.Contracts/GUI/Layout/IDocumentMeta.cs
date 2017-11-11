using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

namespace OdraIDE.Core
{
    public interface IDocumentMeta : ILayoutItemMeta
    {
    }

    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DocumentAttribute : LayoutItemAttribute
    {
        public DocumentAttribute() : base(typeof(IDocument)) { }
    }
}
