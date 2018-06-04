using MUTDOD.Common.Types;
using MUTDOD.Server.Common.ServerStats.StatsStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUTDOD.Server.Common.ServerStats
{
    [Serializable]
    public class SchemaStats : StatisticData
    {

        public Did DatabaseId { get; set; }
        public PropertyStats PropertyStats { get; set; }
        public ClassStats ClassStats { get; set; }

        public SchemaStats()
        {
            PropertyStats = new PropertyStats();
            ClassStats = new ClassStats();
        }
        
    }
}
