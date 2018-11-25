using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUTDOD.Common;
using MUTDOD.Common.Communication;
using MUTDOD.Common.ModuleBase.Communication;
using MUTDOD.Common.Types;

namespace MUTDOD.Server.Common.QueryTree
{
    public class DropMethod : AbstractComposite
    {
        public DropMethod() : base(ElementType.DROP_METHOD) { }

        public String Name { get; set; }
        public List<IMethodParameter> Parameters { get; set; }

        public override QueryDTO Execute(QueryParameters parameters)
        {
            if(Parameters == null)
            {
                Parameters = Enumerable.Empty<IMethodParameter>().ToList();
            }
            var classId = parameters.Subquery.QueryClass.ClassId;
            List<IMethod> classMethods = parameters.Database.Schema.Methods[classId];
            IMethod method = classMethods.Where(
                m => m.Name == Name && m.Parameters.Count == Parameters.Count && m.Parameters.All(p => Parameters.Contains(p))).SingleOrDefault();
            if(method == null)
            {
                return new QueryDTO
                {
                    Result = new DTOQueryResult
                    {
                        QueryResultType = ResultType.StringResult,
                        StringOutput = "No method with name:" + Name
                    }
                };
            }

            parameters.Database.Schema.Methods[classId].Remove(method);

            return new QueryDTO
            {
                Value = method
            };
        }
    }
}
