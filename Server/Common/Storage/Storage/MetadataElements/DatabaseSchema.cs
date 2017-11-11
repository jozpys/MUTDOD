using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using MUTDOD.Common.ModuleBase.Storage.Core.Metadata;
using MUTDOD.Common.Types;

namespace MutDood.Storage.Core.MetadataElements
{
    [Serializable]
    public class DatabaseSchema : IDatabaseSchema
    {
        public Did DatabaseId { get; set; }
        public ConcurrentDictionary<ClassId, Class> Classes { get; set; }
        public ConcurrentDictionary<PropertyId, Property> Properties { get; set; }
        public ConcurrentDictionary<ClassId, List<string>> Methods { get; set; }
    }
}