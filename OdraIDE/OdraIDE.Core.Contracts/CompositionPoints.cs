namespace OdraIDE.Core.CompositionPoints
{
    public static class Host
    {
        public const string MainWindow = "Core.CompositionPoints.Host.MainWindow";
    }

    public static class Workbench
    {

        public static class Commands
        {
            public const string NewFile = "Core.CompositionPoints.Workbench.Commands.NewFile";

            public const string SaveFile = "Core.CompositionPoints.Workbench.Commands.SaveFile";
            public const string SaveFileAs = "Core.CompositionPoints.Workbench.Commands.SaveFileAs";
            public const string SaveAllFile = "Core.CompositionPoints.Workbench.Commands.SaveAllFile";
        }

        public static class StatusBar
        {
            public const string ApplicationStatus = "Core.CompositionPoints.StatusBar.ApplicationStatus";
            public const string ApplicationProgressBar = "Core.CompositionPoints.StatusBar.ApplicationProgressBar";
        }
        public const string ViewModel = "Core.CompositionPoints.Workbench";
    }

    public static class Options
    {
        public const string OptionsDialog = "Core.CompositionPoints.Options.OptionsDialog";
    }

    public static class Help
    {
        public const string AboutDialog = "Core.CompositionPoints.Help.AboutDialog";
    }

}
