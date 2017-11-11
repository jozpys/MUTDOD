using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using OdraIDE.Core;
using System.Collections.ObjectModel;

namespace OdraIDE.Core.Workbench
{
    [Export(ExtensionPoints.Workbench.ToolBars.Self, typeof(IToolBar))]
    class StandardToolbar : AbstractToolBar, IPartImportsSatisfiedNotification
    {
        public StandardToolbar()
        {
            Name = Resources.Strings.Workbench_StandardToolBar;
        }

        [Import(Services.Host.ExtensionService)]
        private IExtensionService extensionService { get; set; }

        [ImportMany(ExtensionPoints.Workbench.ToolBars.Standard, typeof(IToolBarItem), AllowRecomposition = true)]
        private IEnumerable<IToolBarItem>  importedItems { get; set; }

        public void OnImportsSatisfied()
        {
            Items = extensionService.Sort(importedItems);
        }
    }
}
