using MUTDOD.Common;
using MUTDOD.Common.Communication;
using System.ServiceModel;

namespace MUTDOD.Server.Common.CoreModule.Communication
{
    [ServiceContract(Namespace = "Mutdod.Server.DataSever")]
    public interface IDataServerContract
    {
        [OperationContract]
        DTOQueryResult ExecuteQuery(DatabaseInfo dbName, DTOQueryTree queryTree);
    }
}
