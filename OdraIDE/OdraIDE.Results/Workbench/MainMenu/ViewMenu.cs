using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OdraIDE.Core;
using System.ComponentModel.Composition;

namespace OdraIDE.Results
{
    [Export(OdraIDE.Core.ExtensionPoints.Workbench.MainMenu.ViewMenu, typeof(IMenuItem))]
    public class ViewMenuStringResultsPad : AbstractMenuItem
    {
        [Import(OdraIDE.Core.Services.Layout.LayoutManager, typeof(ILayoutManager))]
        private Lazy<ILayoutManager> layoutManager { get; set; }

        [Import(CompositionPoints.Workbench.Pads.StringResultsPad, typeof(StringResultsPad))]
        private Lazy<StringResultsPad> resultsPad { get; set; }

        public ViewMenuStringResultsPad()
        {
            ID = "ViewMenuStringResultsPad";
            Header = "Results";
            ToolTip = "Show Results";
            BeforeOrAfter = RelativeDirection.After;
            InsertRelativeToID = "ViewMenuSolutionExplorer";
            SetIconFromBitmap(Resources.Images.StringResults);
        }

        protected override void Run()
        {
            layoutManager.Value.ShowPad(resultsPad.Value);
        }
    }

    [Export(OdraIDE.Core.ExtensionPoints.Workbench.MainMenu.ViewMenu, typeof(IMenuItem))]
    public class ViewMenuGridResultsPad : AbstractMenuItem
    {
        [Import(OdraIDE.Core.Services.Layout.LayoutManager, typeof(ILayoutManager))]
        private Lazy<ILayoutManager> layoutManager { get; set; }

        [Import(CompositionPoints.Workbench.Pads.GridResultsPad, typeof(GridResultsPad))]
        private Lazy<GridResultsPad> resultsPad { get; set; }

        public ViewMenuGridResultsPad()
        {
            ID = "ViewMenuGridResultsPad";
            Header = "Data results";
            ToolTip = "Show Data Results";
            BeforeOrAfter = RelativeDirection.After;
            InsertRelativeToID = "ViewMenuStringResultsPad";
            SetIconFromBitmap(Resources.Images.Results);
        }

        protected override void Run()
        {
           layoutManager.Value.ShowPad(resultsPad.Value);
        }
    }
}
