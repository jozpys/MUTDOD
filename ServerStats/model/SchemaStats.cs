using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUTDOD.Common.Types;
using MutDood.Storage.Core.MetadataElements;

namespace ServerStats.model
{
    class SchemaStats 
    {
        public DatabaseSchema databaseSchema { get; set; }
        public PropertyStats propertyStats { get; set; }
        public ClassStats classStats { get; set; }
     }
}
