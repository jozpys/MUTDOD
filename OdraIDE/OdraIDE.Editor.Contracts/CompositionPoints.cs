using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OdraIDE.Editor.CompositionPoints
{
    public static class Workbench
    {
        public static class Documents
        {
            public const string SourceEditor = "Editor.CompositionPoints.Workbench.Documents.SourceEditor";
        }

        public static class Painters
        {
            public const string ErrorPainter = "Editor.CompositionPoints.Workbench.Painters.ErrorPainter";
        }

        public static class Options
        {
            public const string SourceEditorGeneralOptionsPad = "Editor.CompositionPoints.Options.SourceEditorGeneralOptionsPad";
        }

        public static class StatusBar
        {
            public const string LineCounterHeading = "Editor.CompositionPoints.StatusBar.LineCounterHeading";
            public const string LineCounterText = "Editor.CompositionPoints.StatusBar.LineCounterText";
            public const string ColumnCounterHeading = "Editor.CompositionPoints.StatusBar.ColumnCounterHeading";
            public const string ColumnCounterText = "Editor.CompositionPoints.StatusBar.ColumnCounterText";
        }
    }
}
