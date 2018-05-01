using OdraIDE.QueryPlan;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUTDOD.Common;

namespace OdraIDE.QueryPlan.Model
{
    public class QueryPlanTreeModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public QueryPlanTreeModel()
        {
            

        }

        private List<MUTDOD.Common.QueryPlan> queryPlan;
        private string errorMessage;

        public string ErrorMessage
        {
            get
            {
                return errorMessage;
            }
            set
            {
                errorMessage = value;
                OnPropertyChanged("ErrorMessage");
            }
        }

        public List<MUTDOD.Common.QueryPlan> QueryPlan
        {
            get
            {
                return queryPlan;
            }
            set
            {
                queryPlan = value;
                OnPropertyChanged("QueryPlan");
            }
        }

        virtual protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
