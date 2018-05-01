using OdraIDE.QueryPlan.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdraIDE.QueryPlan.Services
{
    interface IQueryPlanService
    {
        void ShowQueryPlan(List<TreeTest> queryTree);
        void Clear();
    }
}
