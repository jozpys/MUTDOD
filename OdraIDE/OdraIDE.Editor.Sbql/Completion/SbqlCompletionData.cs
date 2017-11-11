using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using System.ComponentModel.Composition;

namespace OdraIDE.Editor.Sbql
{
    class Snippets
    {
        [Export(OdraIDE.Editor.ExtensionPoints.SourceEditor.CompletionData, typeof(ICompletionData))]
        private CodeSnippetCompletionData ifSnipped = new CodeSnippetCompletionData(
            "if", //nazwa szablonu
            "if true then\n") //zawartosc szablonu
            {
                //zakres zaznaczenia (początek, długość)
                RelativeSelection = new RelativeSelection(3, 4),
            };

        [Export(OdraIDE.Editor.ExtensionPoints.SourceEditor.CompletionData, typeof(ICompletionData))]
        private CodeSnippetCompletionData forEachSnipped = new CodeSnippetCompletionData(
            "for each", //nazwa szablonu
            "for each collection as item do\n{\n\t\n}") //zawartość szablonu
            {
                //zakres zaznaczenia (początek, długość)
                RelativeSelection = new RelativeSelection(9, 10),
            };
        

        [Export(OdraIDE.Editor.ExtensionPoints.SourceEditor.CompletionData, typeof(ICompletionData))]
        private CodeSnippetCompletionData selectSnipped = new CodeSnippetCompletionData(
            "select",
            "select * from Object\nwhere");

        [Export(OdraIDE.Editor.ExtensionPoints.SourceEditor.CompletionData, typeof(ICompletionData))]
        private CodeSnippetCompletionData procedureSnipped = new CodeSnippetCompletionData(
            "procedure",
            "procedure proc()\nbegin\n\t\nend;", -5);
    }

}
