using System;
using System.Collections.Generic;
using MUTDOD.Common.ModuleBase.Storage.Core;
using MUTDOD.Common.ModuleBase.Storage.Core.Metadata;
using MUTDOD.Common.Types;

namespace MUTDOD.Common.ModuleBase.Storage.Strategy
{
    public interface IEngine : IDisposable
    {
        void OpenDatabase(IDatabaseParameters parameters);
        void DeleteDataFiles(IDatabaseParameters parameters);

        void Save(Did did, SerializedStorable storable);
        void Save(Did did, List<SerializedStorable> storables);

        void Remove(Did did, SerializedStorable storable);
        void Remove(Did did, List<SerializedStorable> storables);

        byte[] Read(Did did, Oid oid);
        Dictionary<Oid, byte[]> ReadAll(Did did);
    }
}