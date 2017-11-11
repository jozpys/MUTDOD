using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUTDOD.Common
{
    [Serializable]
    public enum ResultType
    {
        StringResult,
        DatabaseInfo,
        DatabaseInfoArray,
        ReferencesOnly,
        SystemInfo,
        Default
    }
}
