using System.ServiceModel;
using MUTDOD.Common.Communication;

namespace MUTDOD.Common.ModuleBase.Communication
{
    [ServiceContract(Namespace = "Mutdod.Server.CentralSever")]
    public interface ICentralServerContract : IExecutionServerContract
    {
        [OperationContract]
        string RegisterDataServer(string serverName, string adress);
    }
}
