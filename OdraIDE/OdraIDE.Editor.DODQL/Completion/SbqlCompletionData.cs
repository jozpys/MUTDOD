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
    internal class Snippets
    {
        [Export(OdraIDE.Editor.ExtensionPoints.SourceEditor.CompletionData, typeof (ICompletionData))] private
            CodeSnippetCompletionData ifSnipped = new CodeSnippetCompletionData(
                "if", //nazwa szablonu
                "if true then\n") //zawartosc szablonu
            {
                //zakres zaznaczenia (początek, długość)
                RelativeSelection = new RelativeSelection(3, 4),
            };

        [Export(OdraIDE.Editor.ExtensionPoints.SourceEditor.CompletionData, typeof (ICompletionData))] private
            CodeSnippetCompletionData forEachSnipped = new CodeSnippetCompletionData(
                "foreach", //nazwa szablonu
                "foreach e from Collection do\n\t\nendforeach") //zawartość szablonu
            {
                //zakres zaznaczenia (początek, długość)
                RelativeSelection = new RelativeSelection(15, 10),
            };


        [Export(OdraIDE.Editor.ExtensionPoints.SourceEditor.CompletionData, typeof (ICompletionData))] private
            CodeSnippetCompletionData selectSnipped = new CodeSnippetCompletionData(
                "select",
                "select * from Collection\nwhere")
            {
                //zakres zaznaczenia (początek, długość)
                RelativeSelection = new RelativeSelection(14, 10),
            };
    }

}
