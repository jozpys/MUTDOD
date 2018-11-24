using System.Collections.Concurrent;
using System.Collections.Generic;
using MUTDOD.Common.Types;

namespace MUTDOD.Common.ModuleBase.Storage.Core.Metadata
{
    public interface IDatabaseSchema 
    {
        Did DatabaseId { get; set; }
        ConcurrentDictionary<ClassId, Class> Classes { get; set; }
        ConcurrentDictionary<PropertyId, Property> Properties { get; set; }
        ConcurrentDictionary<ClassId, List<IMethod>> Methods { get; set; }
        List<Property> ClassProperties(Class className);
    }
}