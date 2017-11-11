using System.Collections.Concurrent;
using System.ServiceModel;
using MUTDOD.Common.Types;

namespace MUTDOD.Common.ModuleBase
{
    public interface IOdbc : IModule
    {
        ConcurrentDictionary<Oid, ServiceHost> CentralServersConnections { get; }
        ConcurrentDictionary<Oid, ServiceHost> DataServersConnections { get; }
    }
}
