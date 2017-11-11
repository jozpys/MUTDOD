using System.Collections.Generic;

namespace MUTDOD.Common.ModuleBase.Metamodel.DataStructure
{
    public interface IMetaClass : IMetaInterface
    {
        IDictionary<string, IMetaAttribute> Attributes { get; }

        IMetaClass ExtendedClass { get; set; }
        IDictionary<string, IMetaClass> Extensions { get; }

        bool AddMetaAttribute(ref IMetaAttribute metaAttribute);
        bool RemoveMetaAttribute(string metaAttributeName);

        bool AddExtension(ref IMetaClass metaClass);
        bool RemoveExtension(string metaClassName);
    }
}
