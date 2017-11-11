using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OdraIDE.Core;
using System.ComponentModel.Composition;

namespace OdraIDE.SolutionExplorer
{
    [Export(OdraIDE.Core.ExtensionPoints.Host.StartupCommands, typeof(IExecutableCommand))]
    public class StartupCommand : AbstractExtension, IExecutableCommand
    {
        [Import(OdraIDE.Core.Services.Layout.LayoutManager, typeof(ILayoutManager))]
        private Lazy<ILayoutManager> layoutManager { get; set; }

        [Import(CompositionPoints.Workbench.Pads.SolutionExplorer, typeof(OdraIDE.SolutionExplorer.SolutionExplorer))]
        private Lazy<OdraIDE.SolutionExplorer.SolutionExplorer> solutionExplorer { get; set; }

        [Import(CompositionPoints.Workbench.Pads.PropertyGrid, typeof(OdraIDE.SolutionExplorer.PropertyGrid))]
        private Lazy<OdraIDE.SolutionExplorer.PropertyGrid> propertyGrid { get; set; }

        public void Run(params object[] args)
        {
            layoutManager.Value.ShowPad(solutionExplorer.Value);
            layoutManager.Value.ShowPad(propertyGrid.Value);
            layoutManager.Value.ShowPad(solutionExplorer.Value);
        }
    }
}
