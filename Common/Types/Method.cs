using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUTDOD.Common.Types
{
    [Serializable]
    public class Method : IMethod
    {
        public string Name { get; set; }
        public List<IMethodParameter> Parameters { get; set; } = new List<IMethodParameter>();
        public string ReturnType { get; set; }
    }

    [Serializable]
    public class MethodParameter : IMethodParameter
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public ParameterDirection Direction { get; set; }
    }
}
