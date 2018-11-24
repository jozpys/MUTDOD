
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUTDOD.Common.ModuleBase.Communication;
using MUTDOD.Common.Types;

namespace MUTDOD.Server.Common.QueryTree
{
    public class MethodParam : AbstractComposite
    {
        public MethodParam() : base(ElementType.METHOD_PARAM) { }

        public string Name { get; set; }
        public ParameterDirection Direction { get; set; }
        public override QueryDTO Execute(QueryParameters parameters)
        {
            var typeElement = SingleElement();
            var typeDao = typeElement.Execute(parameters);
            MethodParameter param = new MethodParameter { Name = Name, Type = typeDao.Value, Direction = Direction };
            return new QueryDTO() { Value = param };
        }
    }
}
