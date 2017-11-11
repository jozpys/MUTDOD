using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.AvalonEdit.Highlighting;
using OdraIDE.Core;

namespace OdraIDE.Editor
{
    public interface IHighlighting : IExtension
    {
        string Title { get; set; }
        string[] Extenstions { get; set; }
        IHighlightingDefinition Definition { get; set; }
    }
}
