using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OdraIDE.Core;
using System.ComponentModel.Composition;
using OdraIDE.Utilities;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using System.Globalization;
using OdraIDE.QueryPlan.Model;

namespace OdraIDE.QueryPlan
{
    [Export(OdraIDE.Core.ExtensionPoints.Workbench.Pads, typeof(IPad))]
    [Export(OdraIDE.QueryPlan.CompositionPoints.Workbench.Pads.TreeResultPad, typeof(TreeResultPad))]
    [Pad(Name = TreeResultPad.QP_NAME)]
    public class TreeResultPad : AbstractPad
    {
        public const string QP_NAME = "QueryPlanPad";

        [Import(OdraIDE.Core.Services.Layout.LayoutManager, typeof(ILayoutManager))]
        private Lazy<ILayoutManager> layoutManager { get; set; }

        private QueryPlanTreeModel queryPlanTreeModel;

        public TreeResultPad()
        {
            Name = QP_NAME;
            Title = "Query plan";
            Location = PadLocation.Bottom;
            Icon = ImageHelper.GetImageFromResources(Resources.Images.tree);
            queryPlanTreeModel = new QueryPlanTreeModel();
        }

        public QueryPlanTreeModel Model
        {
            get
            {
                return queryPlanTreeModel;
            }
        }

        public void ShowResult(List<MUTDOD.Common.QueryPlan> queryPlan)
        {
            layoutManager.Value.ShowPad(this);
            queryPlanTreeModel.QueryPlan = queryPlan;
        }
        public void ShowErrorMessage(string errorMessage)
        {
            layoutManager.Value.ShowPad(this);
            queryPlanTreeModel.ErrorMessage = errorMessage;
        }
        public void Clear()
        {
            queryPlanTreeModel.QueryPlan = null;
            queryPlanTreeModel.ErrorMessage = null;
        }

    }
}
