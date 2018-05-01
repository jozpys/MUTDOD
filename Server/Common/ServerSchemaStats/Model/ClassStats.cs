using MUTDOD.Common.ModuleBase.Storage.Core.Metadata;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUTDOD.Server.Common.ServerStats
{
    [Serializable]
    public class ClassStats
    {
        public ConcurrentDictionary<ClassId, int> objectNumbers { get; set; }

        public ClassStats()
        {
            objectNumbers = new ConcurrentDictionary<ClassId, int>();
        }
    }
}
