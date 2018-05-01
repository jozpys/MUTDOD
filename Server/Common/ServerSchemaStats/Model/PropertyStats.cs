using MUTDOD.Common.Types;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUTDOD.Server.Common.ServerStats
{
    [Serializable]
    public class PropertyStats
    {
        public ConcurrentDictionary<Object, PropertyId> ValueHist { get; set; }

        public PropertyStats()
        {
            ValueHist = new ConcurrentDictionary<object, PropertyId>();
        }
    }
}
