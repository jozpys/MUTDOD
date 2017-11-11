using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OdraIDE.Editor.ExtensionPoints
{
    public static class Options
    {
        public const string SourceEditorItems = "ExtensionPoints.Options.SourceEditorItems";
    }

    public static class SourceEditor
    {
        public const string Highlighting = "ExtensionPoints.SourceEditor.Highlighting";
        public const string FoldingStrategy = "ExtensionPoints.SourceEditor.FoldingStrategy";
        public const string CompletionData = "ExtensionPoints.SourceEditor.CompletionData";
        public const string FileClosingCommands = "ExtensionPoints.SourceEditor.FileClosingCommands";
    }
}
