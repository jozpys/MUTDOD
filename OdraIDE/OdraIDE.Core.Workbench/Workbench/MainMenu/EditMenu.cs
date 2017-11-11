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
    class EditMenu : AbstractMenuItem, IPartImportsSatisfiedNotification
    {
        public EditMenu()
        {
            ID = Extensions.Workbench.MainMenu.Edit;
            Header = Resources.Strings.Workbench_MainMenu_Edit;
            InsertRelativeToID = Extensions.Workbench.MainMenu.File;
            BeforeOrAfter = RelativeDirection.After;
        }

        [Import(Services.Host.ExtensionService)]
        private IExtensionService extensionService { get; set; }

        [ImportMany(ExtensionPoints.Workbench.MainMenu.EditMenu, typeof(IMenuItem), AllowRecomposition = true)]
        private IEnumerable<IMenuItem> menu { get; set; }

        public void OnImportsSatisfied()
        {
            Items = extensionService.Sort(menu);
        }
    }
}
