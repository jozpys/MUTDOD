using OdraIDE.QueryPlan.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdraIDE.QueryPlan
{
    public class QueryPlanTreeModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public QueryPlanTreeModel()
        {
            TreeTest t1 = new TreeTest();
            t1.id = 1;
            t1.name = "pierwszy";

            TreeTest t2 = new TreeTest();
            t2.id = 2;
            t2.name = "drugi";
            t1.Children = new List<TreeTest>();
            t1.Children.Add(t2);
            OnPropertyChanged("QueryTree");

            treeTest = new List<TreeTest>();
            treeTest.Add(t1);
            

        }
        private List<TreeTest> treeTest;
        public List<TreeTest> QueryTree
        {
            get
            {
                return treeTest;
            }
            set
            {   
                treeTest = value;
                OnPropertyChanged("QueryTree");
            }
        }
        virtual protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
