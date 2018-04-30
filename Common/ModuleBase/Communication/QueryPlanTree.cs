using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUTDOD.Common.ModuleBase.Communication
{
    public class QueryPlanTree
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public int Cost { get; set; }
        public int Cardinality { get; set; }
        public string Value { get; set; }
        public ElementType ElementType { get; set; }
        public IQueryElement QueryElement { get; set; }
        public QueryPlanTree Parent { get; set; }
        public AccessType AccessType { get; set; }

        public QueryPlanTree(int Id, int ParentId, IQueryElement QueryElement, ElementType ElementType, QueryPlanTree Parent, string Value, int Cost, int Cardinality, AccessType accessType)
        {
            this.Id = Id;
            this.ParentId = ParentId;
            this.QueryElement = QueryElement;
            this.Parent = Parent;
            this.ElementType = ElementType;
            this.QueryElement = QueryElement;
            this.Value = Value;
            this.Cost = Cost;
            this.Cardinality = Cardinality;
            this.AccessType = accessType;
        }
        public QueryPlanTree(QueryPlanTree old)
        {
            this.Id = old.Id;
            this.ParentId = old.ParentId;
            this.QueryElement = old.QueryElement;
            this.Parent = old.Parent;
            this.ElementType = old.ElementType;
            this.QueryElement = old.QueryElement;
            this.Value = old.Value;
            this.Cost = old.Cost;
            this.Cardinality = old.Cardinality;
        }
        public List<QueryPlanTree> Childrens { get; set; }

    }
}
