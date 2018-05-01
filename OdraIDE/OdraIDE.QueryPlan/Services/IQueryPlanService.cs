using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUTDOD.Common;
namespace OdraIDE.QueryPlan.Services
{
    interface IQueryPlanService
    {
        void ShowQueryPlan(List<MUTDOD.Common.QueryPlan> queryPlan);
        void Clear();
        void ShowErrorMessage(string errorMessage);
    }
}
