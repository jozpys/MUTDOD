using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OdraIDE.Core.ExtensionPoints
{
    public static class Host
    {
        public const string Styles = "Core.ExtensionPoints.Host.Styles";
        public const string Views = "Core.ExtensionPoints.Host.Views";
        public const string StartupCommands = "Core.ExtensionPoints.Host.StartupCommands";
        public const string ShutdownCommands = "Core.ExtensionPoints.Host.ShutdownCommands";
    }
    public static class Workbench
    {

        public const string StatusBar = "Core.ExtensionPoints.Workbench.StatusBar";
        public const string Pads = "Core.ExtensionPoints.Workbench.Pads";
        public const string Documents = "Core.ExtensionPoints.Workbench.Documents";
        public const string ClosingCommands = "Core.ExtensionPoints.Workbench.ClosingCommands";

        public static class ToolBars
        {
            public const string Self = "Core.ExtensionPoints.Workbench.ToolBars";
            public const string Standard = "Core.ExtensionPoints.Workbench.Toolbars.Standard";
        }

        public static class MainMenu
        {
            public const string Self = "Core.ExtensionPoints.Workbench.MainMenu";
            public const string EditMenu = "Core.ExtensionPoints.Workbench.MainMenu.EditMenu";
            public const string ViewMenu = "Core.ExtensionPoints.Workbench.MainMenu.ViewMenu";
            public const string ToolsMenu = "Core.ExtensionPoints.Workbench.MainMenu.ToolsMenu";
            public const string WindowMenu = "Core.ExtensionPoints.Workbench.MainMenu.WindowMenu";
            public const string HelpMenu = "Core.ExtensionPoints.Workbench.MainMenu.HelpMenu";

            public static class FileMenu
            {
                public const string Self = "Core.ExtensionPoints.Workbench.MainMenu.FileMenu";
                public const string NewMenu = "Core.ExtensionPoints.Workbench.MainMenu.FileMenu.NewMenu";
                public const string OpenMenu = "Core.ExtensionPoints.Workbench.MainMenu.FileMenu.OpenMenu";
            }
        }
    }

    public static class Options
    {
        public static class OptionsDialog
        {
            public const string OptionsItems = "ExtensionPoints.Options.OptionsDialog.OptionsItems";
        }
    }
}
