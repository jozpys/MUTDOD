using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using OdraIDE.Core;
using System.Collections.ObjectModel;

namespace OdraIDE.Core.Workbench
{
    [Export(ExtensionPoints.Workbench.MainMenu.Self, typeof(IMenuItem))]
    class ToolsMenu : AbstractMenuItem, IPartImportsSatisfiedNotification
    {
        public ToolsMenu()
        {
            ID = Extensions.Workbench.MainMenu.Tools;
            Header = Resources.Strings.Workbench_MainMenu_Tools;
            InsertRelativeToID = Extensions.Workbench.MainMenu.View;
            BeforeOrAfter = RelativeDirection.After;
        }

        [Import(Services.Host.ExtensionService)]
        private IExtensionService extensionService { get; set; }

        [ImportMany(ExtensionPoints.Workbench.MainMenu.ToolsMenu, typeof(IMenuItem), AllowRecomposition = true)]
        private IEnumerable<IMenuItem> menu { get; set; }

        public void OnImportsSatisfied()
        {
            Items = extensionService.Sort(menu);
        }
    }
}
