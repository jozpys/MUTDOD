using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using OdraIDE.Core;
using OdraIDE.QueryPlan;

namespace OdraIDE.QueryPlan.Workbench.ViewMenu
{
    [Export(OdraIDE.Core.ExtensionPoints.Workbench.MainMenu.ViewMenu, typeof(IMenuItem))]
    public class ViewMenuQueryPlan : AbstractMenuItem
    {
        [Import(OdraIDE.Core.Services.Layout.LayoutManager, typeof(ILayoutManager))]
        private Lazy<ILayoutManager> layoutManager { get; set; }

        [Import(OdraIDE.QueryPlan.CompositionPoints.Workbench.Pads.TreeResultPad, typeof(TreeResultPad))]
        private Lazy<TreeResultPad> resultsPad { get; set; }

        public ViewMenuQueryPlan()
        {
            ID = "ViewMenuStringResultsPad";
            Header = "Results1";
            ToolTip = "Show Results";
            BeforeOrAfter = RelativeDirection.After;
            InsertRelativeToID = "ViewMenuGridTasksPad";
            SetIconFromBitmap(Resources.Images.tree);
        }

        protected override void Run()
        {
            layoutManager.Value.ShowPad(resultsPad.Value);
        }
    }
}
