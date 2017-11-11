using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OdraIDE.Tasks.CompositionPoints
{
    public static class Workbench
    {
        public static class Pads
        {
            public const string GridTasksPad = "OdraIDE.Tasks.CompositionPoints.Workbench.Pads.GridTasksPad";
        }

        public static class Commands
        {
            public const string ErrorTaskFilter = "OdraIDE.Tasks.Connections.CompositionPoints.Workbench.Commands.ErrorTaskFilter";
            public const string WarningTaskFilter = "OdraIDE.Tasks.Connections.CompositionPoints.Workbench.Commands.WarningTaskFilter";
            public const string MessageTaskFilter = "OdraIDE.Tasks.Connections.CompositionPoints.Workbench.Commands.MessageTaskFilter";
        }
    }
}
