using System;
using System.Collections.Generic;


namespace MUTDOD.Common
{
    [Serializable]
    public class QueryPlan
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public int Cost { get; set; }
        public int Cardinality { get; set; }
        public string Value { get; set; }
        public string ElementType { get; set; }
        public string AccessType { get; set; }
        public string AccessObject { get; set; }

        public List<QueryPlan> Childrens { get; set; }

        public QueryPlan(int Id, int ParentId, string ElementType, string Value, int Cost, int Cardinality, string AccessType, string AccessObject)
        {
            this.Id = Id;
            this.ParentId = ParentId;
            this.ElementType = ElementType;
            this.Value = Value;
            this.Cost = Cost;
            this.Cardinality = Cardinality;
            this.AccessType = AccessType;
            this.AccessObject = AccessObject;
        }
    }
}
