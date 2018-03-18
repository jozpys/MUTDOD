using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OdraIDE.SolutionExplorer.Connections.CompositionPoints
{
    public static class Workbench
    {
        public static class Commands
        {
            public const string Connect = "OdraIDE.SolutionExplorer.Connections.CompositionPoints.Workbench.Commands.Connect";
            public const string Disconnect = "OdraIDE.SolutionExplorer.Connections.CompositionPoints.Workbench.Commands.Disconnect";
            public const string NewDatabase = "OdraIDE.SolutionExplorer.Connections.CompositionPoints.Workbench.Commands.NewDatabase";
            public const string RenameDatabase = "OdraIDE.SolutionExplorer.Connections.CompositionPoints.Workbench.Commands.RenameDatabase";
        }

        public const string NewDatabaseDialog = "OdraIDE.SolutionExplorer.Connections.CompositionPoints.Workbench.NewDatabaseDialog";
        public const string RenameDatabaseDialog = "OdraIDE.SolutionExplorer.Connections.CompositionPoints.Workbench.RenameDatabaseDialog";
        public const string TreeLoader = "OdraIDE.SolutionExplorer.Connections.CompositionPoints.Workbench.TreeLoader";
    }
}
