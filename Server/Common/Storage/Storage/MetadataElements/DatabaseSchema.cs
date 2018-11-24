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
        public ConcurrentDictionary<ClassId, List<IMethod>> Methods { get; set; }

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

        public List<IMethod> ClassMethods(Class className)
        {
            ISet<IMethod> classProperties = new HashSet<IMethod>();
            if(Methods.TryGetValue(className.ClassId, out List<IMethod> methods))
            {
                classProperties.UnionWith(methods);
            }
            

            foreach (var parentClass in className.AllParents())
            {
                if (Methods.TryGetValue(parentClass.ClassId, out List<IMethod> parentMethods))
                {
                    classProperties.UnionWith(parentMethods);
                }
            }

            return classProperties.ToList();
        }
    }
     
}