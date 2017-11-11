using System.Collections.Concurrent;

namespace MUTDOD.Common.ModuleBase.Metamodel.DataStructure
{
    public interface IMetaOperation : IMetaObject
    {
        IMetaType Owner { get; }
        IMetaType Result { get; }
        ConcurrentDictionary<string, IMetaType> Parameters { get; }

        void AddParameter(ref IMetaType parameterMetaType, string parameterName);
        void RemoveParameter(string parameterName);
    }
}
