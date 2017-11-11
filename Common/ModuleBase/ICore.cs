using System.Collections.Generic;

namespace MUTDOD.Common.ModuleBase
{
    public interface ICore
    {
        void Init(IDictionary<string, object> parameters);

        IModule GetModule(IModule module);
    }
}
