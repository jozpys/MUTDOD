using System.Collections.Generic;
using MUTDOD.Common.ModuleBase.Storage;
using MUTDOD.Common.ModuleBase.Storage.Core.Metadata;
using MUTDOD.Common.Settings;
using MUTDOD.Common.Types;

namespace MUTDOD.Common.ModuleBase
{
    public interface IStorage : IModule
    {
        Oid Save(Did dbId, IStorable toStore);
        IEnumerable<Oid> Save(Did dbId, IEnumerable<IStorable> toStore);
        IStorable Get(Did dbId, Oid oid);
        IEnumerable<IStorable> GetAll(Did dbId);
        IStorable[] Find(Did dbId, ISearchCriteria searchCriteria);
        IDatabaseParameters[] GetDatabases();
        IDatabaseParameters GetDatabase(Did did);
        DeleteDatabaseStatus RemoveDatabase(IDatabaseRemoveParameters databaseRemoveParameters);
        Did CreateDatabase(IDatabaseParameters databaseParameters);
        Did RenameDatabase(IDatabaseParameters databaseParameters, string databaseNewName, ISettingsManager settingsManager);
        IDatabaseSchema GetSchema(Did databaseId);
        void SaveSchema(IDatabaseSchema schema);
    }
}
