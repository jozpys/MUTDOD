using System.ServiceModel;
using MUTDOD.Common.Communication;

namespace MUTDOD.Common.ModuleBase.Communication
{
    [ServiceContract(Namespace = "Mutdod.Server.DataSever")]
    public interface IDataServerContract
    {
        [OperationContract]
        DTOQueryResult ExecuteQuery(DatabaseInfo dbName, DTOQueryTree queryTree);
    }
}
