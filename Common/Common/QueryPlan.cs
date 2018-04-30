using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUTDOD.Common
{
    public class QueryPlan
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public int Cost { get; set; }
        public int Cardinality { get; set; }
        public string Value { get; set; }
        public string ElementType { get; set; }
        public string AccessType { get; set; }
        public List<QueryPlan> Childrens { get; set; }

        public QueryPlan(int Id, int ParentId, string ElementType, string Value, int Cost, int Cardinality)
        {
            this.Id = Id;
            this.ParentId = ParentId;
            this.ElementType = ElementType;
            this.Value = Value;
            this.Cost = Cost;
            this.Cardinality = Cardinality;
        }
    }
}
