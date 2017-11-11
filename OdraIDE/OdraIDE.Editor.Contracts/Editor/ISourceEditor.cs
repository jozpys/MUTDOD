using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.AvalonEdit;
using OdraIDE.Core;

namespace OdraIDE.Editor
{
    public interface ISourceEditor : IDocument
    {
        event EventHandler QuerySelectionChanged;
        string SourceCode { get; }
        string SelectedSourceCode { get; }
        TextEditor TextEditor { get; }
    }
}
