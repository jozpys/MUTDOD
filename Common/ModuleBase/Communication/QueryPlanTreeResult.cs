using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUTDOD.Common.Types;

namespace MUTDOD.Common.ModuleBase.Communication
{
    class QueryPlanTreeResult : IQueryResult
    {
        public ResultType QueryResultType => throw new NotImplementedException();

        public IQueryResult NextResult => throw new NotImplementedException();

        public string StringOutput => throw new NotImplementedException();

        public List<Oid> QueryResults => throw new NotImplementedException();
    }
}
