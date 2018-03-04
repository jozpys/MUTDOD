﻿using System.ServiceModel;
using MUTDOD.Common.Communication;

namespace MUTDOD.Server.Common.CoreModule.Communication
{
    [ServiceContract(Namespace = "Mutdod.Server.CentralSever")]
    public interface ICentralServerContract : IExecutionServerContract
    {
        [OperationContract]
        string RegisterDataServer(string serverName, string adress);
    }
}
