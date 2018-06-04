using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using MUTDOD.Common;
using MUTDOD.Common.Communication;
using MUTDOD.Common.ModuleBase;
using MUTDOD.Common.ModuleBase.Communication;
using MUTDOD.Common.ModuleBase.Storage.Core.Metadata;

namespace MUTDOD.Server.Common.QueryTree
{
    [DataContract]
    public class ClassName : AbstractLeaf
    {
        public ClassName() : base(ElementType.CLASS_NAME) {
            Index = default(KeyValuePair<int, string>);
        }
        [DataMember]
        public String Name {get; set;}
        public Dictionary<int, string> Indexes { get; set; }
        public KeyValuePair<int, string> Index { get; set; }
        private static string CLASS = "CLASS: ";
        private static string INDEX = "INDEX: ";
        public override QueryDTO Execute(QueryParameters parameters)
        {
            if (parameters.Database.Schema == null)
                throw new ApplicationException("Schema can not be null!");
            Class selectedClass = parameters.Database.Schema.Classes.Values.SingleOrDefault(c => c.Name == Name);
            if (selectedClass == null) {
                Class errorClass = new Class() { Name = Name };
                DTOQueryResult errorResult = new DTOQueryResult
                {
                    NextResult = null,
                    QueryResultType = ResultType.StringResult,
                    StringOutput = "Unknown class: " + Name
                };
                return new QueryDTO { Result = errorResult, QueryClass = errorClass };
            }
            return new QueryDTO { QueryClass = selectedClass };
        }

        public override int Cost(QueryParameters parameters)
        {
            return 1; 
        }

        public override int Cardinality(QueryParameters queryParameters)
        {
            var classId = queryParameters.Database.Schema.Classes.Values.SingleOrDefault(c => c.Name == Name)?.ClassId;
            if(classId != null)
                return queryParameters.ServerSchemaStats.GetClassObjectNumber(queryParameters.Database.DatabaseId, 
                       queryParameters.Database.Schema.Classes.Values.SingleOrDefault(c => c.Name == Name).ClassId);
            else return 0;
        }
        public override string Value
        {
            get { return Name; }
        }

        public override bool Optimize(QueryParameters parameters, QueryStack queryStack)
        {
            var index = default(KeyValuePair<int, string>);


            if (Indexes != null && Indexes.Count > 0)
                index = Indexes
                               .OrderByDescending(p => parameters.IndexMechanism.GetPessimisticObjectFindCost(p.Key, Cardinality(parameters)))
                               .First();

            if (!index.Equals(Index) && Cardinality(parameters) > parameters.SettingsManager.MaxNumberObjectsFullScan)
            {
                Index = index;
                var selectStatement = (SelectStatement)queryStack.FindLastAncestorOnPeekByType(ElementType.SELECT);
                selectStatement.Index = index;
                return true;
            }

            
            


            return false;
        }

        public override AccessType AccessType
        {
            get
            {
                if (!Index.Equals(default(KeyValuePair<int, string>)))
                    return AccessType.INDEX;
                else 
                    return AccessType.FULL;
            }
        }
        public override string AccessObject
        {
            get
            {
                if (!Index.Equals(default(KeyValuePair<int, string>)))
                    return INDEX+ Index.Value;
                else
                    return CLASS+Name;
            }
        }

    }
}
