using System.Collections.Generic;

namespace MUTDOD.Common
{
    public interface IQueryResult
    {
        ResultType QueryResultType { get; }
        bool IsError { get; }
        IQueryResult NextResult { get; }
        string StringOutput { get;  }
        List<MUTDOD.Common.Types.Oid> QueryResults { get; }
    }
}
