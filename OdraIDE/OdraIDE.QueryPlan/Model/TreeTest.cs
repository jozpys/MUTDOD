using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdraIDE.QueryPlan.Model
{
    public class TreeTest
    {
        public int id { set; get; }
        public string name { set; get; }
        public List<TreeTest> Children { set; get; }
    }
}
