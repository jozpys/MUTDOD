using MUTDOD.Common.ModuleBase;
using MUTDOD.Common.ModuleBase.Storage.Core.Metadata;
using MUTDOD.Common.Types;
using ServerStats.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerStats
{
    public class Statistic : Module, IStats
    {
        private List<SchemaStats> schemaStats;

        public int getClassObjectNumber(Did databaseId, ClassId classId)
        {
            return schemaStats.Find(p => p.databaseSchema.DatabaseId.Equals(databaseId))
                              .classStats.objectHist.TryGetValue(classId, out int number) ? number : 0;
        }

        public void propertyValue(Property property, object value)
        {
            throw new NotImplementedException();
        }

        public int propertyValueNumber(Property property)
        {
            throw new NotImplementedException();
        }

        public bool recalculateStats()
        {
            throw new NotImplementedException();
        }

        public string Name
        {
            get
            {
                return
                    "Statistic";
            }
        }
    }
}
