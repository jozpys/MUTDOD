using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OdraIDE.Core;
using System.ComponentModel.Composition;

namespace OdraIDE.SolutionExplorer
{
    [Export(OdraIDE.Core.ExtensionPoints.Workbench.MainMenu.ViewMenu, typeof(IMenuItem))]
    public class ViewMenuSolutionExplorer : AbstractMenuItem
    {
        [Import(OdraIDE.Core.Services.Layout.LayoutManager, typeof(ILayoutManager))]
        private Lazy<ILayoutManager> layoutManager { get; set; }

        [Import(CompositionPoints.Workbench.Pads.SolutionExplorer, typeof(OdraIDE.SolutionExplorer.SolutionExplorer))]
        private Lazy<OdraIDE.SolutionExplorer.SolutionExplorer> solutionExplorer { get; set; }

        public ViewMenuSolutionExplorer()
        {
            ID = "ViewMenuSolutionExplorer";
            Header = "Solution Explorer";
            ToolTip = "Show Solution Explorer";
            SetIconFromBitmap(Resources.Images.SolutionExplorer);
        }

        protected override void Run()
        {
            layoutManager.Value.ShowPad(solutionExplorer.Value);
        }
    }

    [Export(OdraIDE.Core.ExtensionPoints.Workbench.MainMenu.ViewMenu, typeof(IMenuItem))]
    public class ViewMenuPropertyGrid : AbstractMenuItem
    {
        [Import(OdraIDE.Core.Services.Layout.LayoutManager, typeof(ILayoutManager))]
        private Lazy<ILayoutManager> layoutManager { get; set; }

        [Import(CompositionPoints.Workbench.Pads.PropertyGrid, typeof(OdraIDE.SolutionExplorer.PropertyGrid))]
        private Lazy<OdraIDE.SolutionExplorer.PropertyGrid> propertyGrid { get; set; }

        public ViewMenuPropertyGrid()
        {
            ID = "ViewMenuPropertyGrid";
            Header = "Properties";
            ToolTip = "Show Properties";
            SetIconFromBitmap(Resources.Images.Properties);
        }

        protected override void Run()
        {
            layoutManager.Value.ShowPad(propertyGrid.Value);
        }
    }
}
