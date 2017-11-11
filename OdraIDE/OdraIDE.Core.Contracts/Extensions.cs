using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OdraIDE.Core.Extensions
{
    public static class Workbench
    {
        public static class MainMenu
        {
            public const string File = "File";
            public const string Edit = "Edit";
            public const string View = "View";
            public const string Tools = "Tools";
            public const string Window = "Window";
            public const string Help = "Help";

            public static class FileMenu
            {
                public const string New = "New";
                public const string Open = "Open";
                public const string Exit = "Exit";
            }

            public static class ViewMenu
            {
                public const string ToolBars = "ToolBars";
            }

            public static class ToolsMenu
            {
                public const string PluginManager = "PluginManager";
                public const string Options = "Options";
            }

            public static class HelpMenu
            {
                public const string About = "About";
            }
        }
    }
}
