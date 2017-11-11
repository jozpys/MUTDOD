using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using OdraIDE.Core;

namespace OdraIDE.SolutionExplorer
{
    [Export(ExtensionPoints.ToolBars.Self, typeof(IToolBar))]
    class SolutionToolBar : AbstractToolBar, IPartImportsSatisfiedNotification
    {
        public SolutionToolBar()
        {
            Name = "Solution ToolBar";
        }

        [Import(OdraIDE.Core.Services.Host.ExtensionService)]
        private IExtensionService extensionService { get; set; }

        [ImportMany(ExtensionPoints.ToolBars.Solution, typeof(IToolBarItem), AllowRecomposition = true)]
        private IEnumerable<IToolBarItem> importedItems { get; set; }

        public void OnImportsSatisfied()
        {
            Items = extensionService.Sort(importedItems);
        }
    }
}
