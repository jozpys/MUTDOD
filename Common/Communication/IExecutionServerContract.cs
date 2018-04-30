using System.ServiceModel;

namespace MUTDOD.Common.Communication
{
    [ServiceContract(Namespace = "Mutdod.Server")]
    public interface IExecutionServerContract
    {
        [OperationContract]
        DTOQueryResult ExecuteQuery(DatabaseInfo dbName, DTOQuery query);

        [OperationContract]
        bool IsAvailable();
    }

    [ServiceContract(Namespace = "Mutdod.Server")]
    public interface IQueryPlanServerContract
    {
        [OperationContract]
        IQueryPlanReslult GetQueryPlan(DatabaseInfo dbName, DTOQuery query);

    }
}
