using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OdraIDE.Core;
using System.ComponentModel.Composition;
using OdraIDE.Utilities;
using OdraIDE.Core.GUI.DynamicListView;
using OdraIDE.Core.GUI.TreeItem;
using System.Collections.ObjectModel;
using Telerik.Windows.Controls;
using System.Windows;
using System.Windows.Data;
using System.Globalization;
using OdraIDE.QueryPlan.Model;

namespace OdraIDE.QueryPlan
{
    [Export(OdraIDE.Core.ExtensionPoints.Workbench.Pads, typeof(IPad))]
    [Export(OdraIDE.QueryPlan.CompositionPoints.Workbench.Pads.TreeResultPad, typeof(TreeResultPad))]
    [Pad(Name = TreeResultPad.RP_NAME)]
    public class TreeResultPad : AbstractPad
    {
        public const string RP_NAME = "TreeResultPad";

        [Import(OdraIDE.Core.Services.Layout.LayoutManager, typeof(ILayoutManager))]
        private Lazy<ILayoutManager> layoutManager { get; set; }

        private QueryPlanTreeModel queryPlanTreeModel;

        public TreeResultPad()
        {
            Name = RP_NAME;
            Title = "Query plan";
            Location = PadLocation.Bottom;
            Icon = ImageHelper.GetImageFromResources(Resources.Images.tree);
            queryPlanTreeModel = new QueryPlanTreeModel(); 
        }

        public QueryPlanTreeModel Model1
        {
            get
            {
                return queryPlanTreeModel;
            }
        }

        public void ShowResult(List<TreeTest> lista)
        {
            layoutManager.Value.ShowPad(this);
            queryPlanTreeModel.QueryTree = lista;
        }

        public void Clear()
        {
           // m_TreeView.DataSource = new List<TreeNode>();
        }
       
    }
}
