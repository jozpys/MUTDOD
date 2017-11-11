using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using OdraIDE.Core;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;

namespace OdraIDE.Core.Workbench
{
    [Export(ExtensionPoints.Workbench.MainMenu.Self, typeof(IMenuItem))]
    class FileMenu : AbstractMenuItem, IPartImportsSatisfiedNotification
    {
        public FileMenu()
        {
            ID = Extensions.Workbench.MainMenu.File;
            Header = Resources.Strings.Workbench_MainMenu_File;
        }

        [Import(Services.Host.ExtensionService)]
        private IExtensionService extensionService { get; set; }

        [ImportMany(ExtensionPoints.Workbench.MainMenu.FileMenu.Self, typeof(IMenuItem), AllowRecomposition = true)]
        private IEnumerable<IMenuItem> menu { get; set; }

        public void OnImportsSatisfied()
        {
            Items = extensionService.Sort(menu);
        }
    }

    [Export(OdraIDE.Core.ExtensionPoints.Workbench.MainMenu.FileMenu.Self, typeof(IMenuItem))]
    class FileMenuNew : AbstractMenuItem, IPartImportsSatisfiedNotification
    {
        public FileMenuNew()
        {
            ID = Extensions.Workbench.MainMenu.FileMenu.New;
            Header = Resources.Strings.Workbench_MainMenu_File_New;
        }

        [Import(Services.Host.ExtensionService)]
        private IExtensionService extensionService { get; set; }

        [ImportMany(ExtensionPoints.Workbench.MainMenu.FileMenu.NewMenu, typeof(IMenuItem), AllowRecomposition = true)]
        private IEnumerable<IMenuItem> menu { get; set; }

        public void OnImportsSatisfied()
        {
            Items = extensionService.Sort(menu);
        }
    }

    [Export(OdraIDE.Core.ExtensionPoints.Workbench.MainMenu.FileMenu.Self, typeof(IMenuItem))]
    class FileMenuOpen : AbstractMenuItem, IPartImportsSatisfiedNotification
    {
        public FileMenuOpen()
        {
            ID = Extensions.Workbench.MainMenu.FileMenu.Open;
            InsertRelativeToID = Extensions.Workbench.MainMenu.FileMenu.New;
            BeforeOrAfter = RelativeDirection.After;
            Header = Resources.Strings.Workbench_MainMenu_File_Open;
        }

        [Import(Services.Host.ExtensionService)]
        private IExtensionService extensionService { get; set; }

        [ImportMany(ExtensionPoints.Workbench.MainMenu.FileMenu.OpenMenu, typeof(IMenuItem), AllowRecomposition = true)]
        private IEnumerable<IMenuItem> menu { get; set; }

        public void OnImportsSatisfied()
        {
            Items = extensionService.Sort(menu);
        }
    }

    [Export(ExtensionPoints.Workbench.MainMenu.FileMenu.Self, typeof(IMenuItem))]
    class FileMenuExit : AbstractMenuItem
    {
        public FileMenuExit()
        {
            ID = Extensions.Workbench.MainMenu.FileMenu.Exit;
            InsertRelativeToID = Extensions.Workbench.MainMenu.FileMenu.Open;
            BeforeOrAfter = RelativeDirection.After;
            Header = Resources.Strings.Workbench_MainMenu_File_Exit;
            ToolTip = Resources.Strings.Workbench_Command_Tooltip_Exit;
            SetIconFromBitmap(Resources.Images.Workbench_Command_Exit);
        }

        // This has to be a Lazy import because we're instantiated 
        // when the Workbench itself is instantiated, so the Workbench
        // isn't finished being constructed yet.
        [Import(CompositionPoints.Host.MainWindow, typeof(Window))]
        private Lazy<Window> mainWindowExport { get; set; }

        protected override void Run()
        {
            Window mainWindow = mainWindowExport.Value;
            mainWindow.Close();
        }
    }
}
