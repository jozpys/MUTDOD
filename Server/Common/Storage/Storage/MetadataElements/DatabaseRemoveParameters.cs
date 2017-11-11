using MUTDOD.Common.ModuleBase.Storage.Core.Metadata;
using MUTDOD.Common.Types;

namespace MUTDOD.Server.Common.Storage.MetadataElements
{
    public class DatabaseRemoveParameters : IDatabaseRemoveParameters
    {
        public Did DatabaseToRemove { get; set; }
    }
}