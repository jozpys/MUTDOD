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
    public class MethodDeclaration : AbstractComposite
    {
        public MethodDeclaration() : base(ElementType.METHOD_DECLARATION) { }

        public String Name { get; set; }
        public List<IMethodParameter> Parameters { get; set; }

        public override QueryDTO Execute(QueryParameters parameters)
        {
            var classId = parameters.Subquery.QueryClass.ClassId;
            var typeElement = Element(ElementType.DATA_TYPE);
            var typeDao = typeElement.Execute(parameters);
            Method method = new Method { Name = Name, ReturnType = typeDao.Value };
            foreach(var paramElement in AllElements(ElementType.METHOD_PARAM))
            {
                var paramDao = paramElement.Execute(parameters);
                method.Parameters.Add(paramDao.Value);
            }
            parameters.Database.Schema.Methods[classId].Add(method);

            return new QueryDTO
            {
                Value = method
            };
        }
    }
}
