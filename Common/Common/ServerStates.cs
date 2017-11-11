using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUTDOD.Common
{
    [Serializable]
    public enum ServerStates
    {
        Paused,
        Running,
        Stopped,
        Starting,
        Stopping,
        Restarting,
        Reconfiguring,
        Error
    }
}
