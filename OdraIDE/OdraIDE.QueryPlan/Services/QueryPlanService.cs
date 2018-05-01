using OdraIDE.Core;
using OdraIDE.QueryPlan.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdraIDE.QueryPlan.Services
{
    [Export(OdraIDE.Core.Services.QueryPlan.QueryPlanService, typeof(IQueryPlanService))]
    class QueryPlanService : IQueryPlanService
    {
        [Import(CompositionPoints.Workbench.Pads.TreeResultPad, typeof(TreeResultPad))]
        private TreeResultPad treeResultPad { get; set; }


        public void ShowQueryPlan(List<TreeTest> lista)
        {
            treeResultPad.ShowResult(lista);
        }

        public void Clear()
        {
            treeResultPad.Clear();
        }
    }
}
