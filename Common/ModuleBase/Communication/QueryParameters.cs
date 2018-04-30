using MUTDOD.Common.ModuleBase.Storage.Core.Metadata;
using MUTDOD.Common.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUTDOD.Common.ModuleBase.Communication
{
    [Serializable]
    public class QueryParameters
    {
        public IDatabaseParameters Database { get; set; }
        public SystemInfo SystemInfo { get; set; }
        public IStorage Storage { get; set; }
        public ISettingsManager SettingsManager { get; set; }
        public Action<String, MessageLevel> Log { get; set; }
        public QueryDTO Subquery { get; set; }
        public IIndexMechanism IndexMechanism { get; set; }
    }
}
