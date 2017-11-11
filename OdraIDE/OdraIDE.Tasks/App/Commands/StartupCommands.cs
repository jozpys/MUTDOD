using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OdraIDE.Core;
using System.ComponentModel.Composition;
using AvalonDock;

namespace OdraIDE.Tasks
{
    [Export(OdraIDE.Core.ExtensionPoints.Host.StartupCommands, typeof(IExecutableCommand))]
    public class StartupCommand : AbstractExtension, IExecutableCommand
    {
        [Import(OdraIDE.Core.Services.Layout.LayoutManager, typeof(ILayoutManager))]
        private Lazy<ILayoutManager> layoutManager { get; set; }

        [Import(CompositionPoints.Workbench.Pads.GridTasksPad, typeof(GridTasksPad))]
        private Lazy<GridTasksPad> gridTasksPad { get; set; }

        public void Run(params object[] args)
        {
            layoutManager.Value.ShowPad(gridTasksPad.Value);
        }
    }
}
