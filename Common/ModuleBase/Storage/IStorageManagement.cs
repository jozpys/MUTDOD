using MUTDOD.Common.ModuleBase.Storage.Core.Metadata;
using MUTDOD.Common.Types;

namespace MUTDOD.Common.ModuleBase.Storage
{
    /// <summary>
    /// Facilitating methods for manipulating storage settings
    /// </summary>
    public interface IStorageManagement
    {
        Did CreateDatabase(IDatabaseParameters databaseParameters);
        DeleteDatabaseStatus RemoveDatabase(IDatabaseRemoveParameters databaseRemoveParameters);
        IDatabaseParameters[] GetDatabases();
        IDatabaseParameters GetDatabase(Did did);
    }
}
