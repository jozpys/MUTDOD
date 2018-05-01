using OdraIDE.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdraIDE.QueryPlan.Commands
{
    [Export(OdraIDE.Core.ExtensionPoints.Host.StartupCommands, typeof(IExecutableCommand))]
    public class StartupCommand : AbstractExtension, IExecutableCommand
    {
        [Import(OdraIDE.Core.Services.Layout.LayoutManager, typeof(ILayoutManager))]
        private Lazy<ILayoutManager> layoutManager { get; set; }

        [Import(CompositionPoints.Workbench.Pads.TreeResultPad, typeof(TreeResultPad))]
        private Lazy<TreeResultPad> treeResultPad { get; set; }

        public void Run(params object[] args)
        {
            layoutManager.Value.ShowPad(treeResultPad.Value);
        }
    }
}
