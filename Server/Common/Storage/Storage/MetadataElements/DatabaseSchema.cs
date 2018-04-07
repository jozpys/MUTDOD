using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
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

        public List<Property> ClassProperties(Class className)
        {
            ISet<Property> classProperties = new HashSet<Property>();
            classProperties.UnionWith(Properties.Values.Where(p => p.ParentClassId == className.ClassId.Id));

            foreach (var parentClass in className.AllParents())
            {
                classProperties.UnionWith(Properties.Values.Where(p => p.ParentClassId == parentClass.ClassId.Id));
            }

            return classProperties.ToList();
        }
    }
     
}