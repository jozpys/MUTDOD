using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUTDOD.Common;
using MUTDOD.Common.Communication;
using MUTDOD.Common.ModuleBase.Communication;

namespace MUTDOD.Server.Common.QueryTree
{
    public class MethodDeclaration : AbstractLeaf
    {
        public MethodDeclaration() : base(ElementType.METHOD_DECLARATION) { }

        public String Name { get; set; }

        public override QueryDTO Execute(QueryParameters parameters)
        {
            var classId = parameters.Subquery.QueryClass.ClassId;
            parameters.Database.Schema.Methods[classId].Add(Name);

            return new QueryDTO
            {
                Result = new DTOQueryResult
                {
                    QueryResultType = ResultType.StringResult,
                    StringOutput = "Method added:" + Name
                }
            };
        }
    }
}
