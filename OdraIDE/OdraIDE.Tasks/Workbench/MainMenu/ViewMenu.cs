using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OdraIDE.Core;
using System.ComponentModel.Composition;

namespace OdraIDE.Tasks
{
    [Export(OdraIDE.Core.ExtensionPoints.Workbench.MainMenu.ViewMenu, typeof(IMenuItem))]
    public class ViewMenuGridTasksPad : AbstractMenuItem
    {
        [Import(OdraIDE.Core.Services.Layout.LayoutManager, typeof(ILayoutManager))]
        private Lazy<ILayoutManager> layoutManager { get; set; }

        [Import(CompositionPoints.Workbench.Pads.GridTasksPad, typeof(GridTasksPad))]
        private Lazy<GridTasksPad> tasksPad { get; set; }

        public ViewMenuGridTasksPad()
        {
            ID = "ViewMenuGridTasksPad";
            Header = "Tasks";
            ToolTip = "Show Tasks";
            BeforeOrAfter = RelativeDirection.After;
            InsertRelativeToID = "ViewMenuGridResultsPad";
            SetIconFromBitmap(Resources.Images.Tasks);
        }

        protected override void Run()
        {
           layoutManager.Value.ShowPad(tasksPad.Value);
        }
    }
}
