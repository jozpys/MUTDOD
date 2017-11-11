using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OdraIDE.Editor.Sbql.CompositionPoints
{
    public static class Workbench
    {
        public static class Commands
        {
            public const string NewSbqlFile = "Editor.Sbql.CompositionPoints.Workbench.Commands.NewSbqlFile";
            public const string OpenSbqlFile = "Editor.Sbql.CompositionPoints.Workbench.Commands.OpenSbqlFile";
            public const string SaveSbqlFile = "Editor.Sbql.CompositionPoints.Workbench.Commands.SaveSbqlFile";
            public const string SaveSbqlFileAs = "Editor.Sbql.CompositionPoints.Workbench.Commands.SaveSbqlFileAs";
            public const string SaveAllSbqlFiles = "Editor.Sbql.CompositionPoints.Workbench.Commands.SaveAllSbqlFiles";

            public const string ExecuteQuery = "Editor.Sbql.CompositionPoints.Workbench.Commands.ExecuteQuery";
            public const string ExecuteSelectedQuery = "Editor.Sbql.CompositionPoints.Workbench.Commands.ExecuteSelectedQuery";
            public const string CancelExecuteQuery = "Editor.Sbql.CompositionPoints.Workbench.Commands.CancelExecuteQuery";

            public const string CommentLines = "Editor.Sbql.CompositionPoints.Workbench.Commands.CommentLines";
        }

        public static class ExecuteToolbar
        {
            public const string DatabasesComboBox = "Editor.Sbql.CompositionPoints.ExecuteToolbar.DatabasesComboBox";
        }
    }
}
