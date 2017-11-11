using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using OdraIDE.Core;
using System.Collections.ObjectModel;
using System.Windows;

namespace OdraIDE.Core.Workbench
{
    [Export(ExtensionPoints.Workbench.MainMenu.Self, typeof(IMenuItem))]
    class HelpMenu : AbstractMenuItem, IPartImportsSatisfiedNotification
    {
        public HelpMenu()
        {
            ID = Extensions.Workbench.MainMenu.Help;
            Header = Resources.Strings.Workbench_MainMenu_Help;
            InsertRelativeToID = Extensions.Workbench.MainMenu.Window;
            BeforeOrAfter = RelativeDirection.After;
        }

        [Import(Services.Host.ExtensionService)]
        private IExtensionService extensionService { get; set; }

        [ImportMany(ExtensionPoints.Workbench.MainMenu.HelpMenu, typeof(IMenuItem), AllowRecomposition = true)]
        private IEnumerable<IMenuItem> menu { get; set; }

        public void OnImportsSatisfied()
        {
            Items = extensionService.Sort(menu);
        }
    }

    [Export(ExtensionPoints.Workbench.MainMenu.HelpMenu, typeof(IMenuItem))]
    class HelpMenuAbout : AbstractMenuItem
    {
        [Import(OdraIDE.Core.CompositionPoints.Help.AboutDialog, typeof(AboutWindow))]
        private AboutWindow aboutWindow { get; set; }

        public HelpMenuAbout()
        {
            ID = Extensions.Workbench.MainMenu.HelpMenu.About;
            Header = Resources.Strings.Workbench_MainMenu_Help_About;
        }

        protected override void Run()
        {
            aboutWindow.ShowWindow();
        }
    }
}
