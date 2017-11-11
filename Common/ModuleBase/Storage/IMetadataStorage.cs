using MUTDOD.Common.ModuleBase.Storage.Core.Metadata;
using MUTDOD.Common.Types;

namespace MUTDOD.Common.ModuleBase.Storage
{
    /// <summary>
    /// Facilitating methods for metadata manipulation
    /// </summary>
    public interface IMetadataStorage
    {
        void SaveSchema(IDatabaseSchema schema);
        IDatabaseSchema GetSchema(Did databaseId);
    }
}
