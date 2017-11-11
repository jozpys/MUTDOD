using MUTDOD.Common.Types;

namespace MUTDOD.Common.ModuleBase.Storage.Core.Metadata
{
    public interface IDatabaseRemoveParameters
    {
        Did DatabaseToRemove { get; set; }
    }
}