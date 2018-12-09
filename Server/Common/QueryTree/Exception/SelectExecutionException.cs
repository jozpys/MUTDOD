using MUTDOD.Common.ModuleBase.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUTDOD.Server.Common.QueryTree
{
    public class SelectExecutionException : Exception
    {
        public SelectExecutionException(QueryDTO queryDto)
        {
            ErrorDto = queryDto;
        }
        public QueryDTO ErrorDto { get; set; }
    }
}
