using System.Collections.Generic;

namespace MUTDOD.Common.ModuleBase.Metamodel.DataStructure
{
    public interface IMetaInterface : IMetaType
    {
        IDictionary<string, IMetaOperation> Operations { get; }

        IDictionary<string, IMetaInterface> Derivments { get; }
        IDictionary<string, IMetaInterface> Inheritances { get; }

        bool AddMetaOperation(ref IMetaOperation metaOperaton);
        bool RemoveMetaOperation(string metaAttributeName);

        bool AddDerivement(ref IMetaInterface metaInterface);
        bool RemoveDerivement(string metaInterfaceName);

        bool AddInheritance(ref IMetaInterface metaInterface);
        bool RemoveInheritance(string metaInterfaceName);
    }
}
