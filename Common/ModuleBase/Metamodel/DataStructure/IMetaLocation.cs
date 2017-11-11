using System.Collections.Concurrent;

namespace MUTDOD.Common.ModuleBase.Metamodel.DataStructure
{
    public interface IMetaLocation : IMetaObject
    {
        ConcurrentDictionary<string, IMetaObject> MetaObjects { get; }

        bool AddMetaObject(ref IMetaLocation metaObject);
        bool RemoveMetaObject(string metaObjectName);
        bool HasMetaObject(string metaObjectName);
    }
}
