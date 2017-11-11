using System.Collections.Generic;
using MUTDOD.Common.Types;

namespace MUTDOD.Common.ModuleBase.Storage.Core
{
    public interface ISerializer
    {
        List<SerializedStorable> Serialize(IStorable objectToStore);
        IStorable Deserialize(Did dbId, SerializedStorable serializedStorable);
    }
}