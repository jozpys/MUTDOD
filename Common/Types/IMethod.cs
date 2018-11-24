using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUTDOD.Common.Types
{
    public interface IMethod
    {
        string Name { get; set; }
        List<IMethodParameter> Parameters { get; set; }
        string ReturnType { get; set; }

    }

    public interface IMethodParameter
    {
        string Name { get; set; }
        string Type { get; set; }
        ParameterDirection Direction { get; set; }
    }

    public enum ParameterDirection { IN, OUT }
}
