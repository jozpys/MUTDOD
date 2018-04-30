using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUTDOD.Common.Types;


namespace ServerStats.model
{
    class PropertyStats
    {
        public Property property { get; }
        public ConcurrentDictionary<Object, int> ValueHist { get; set; }
    }
}
