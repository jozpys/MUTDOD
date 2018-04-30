using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUTDOD.Common.ModuleBase.Storage.Core.Metadata;


namespace ServerStats.model
{
    class ClassStats
    {
        public ConcurrentDictionary<ClassId, int> objectHist;
    }
}
