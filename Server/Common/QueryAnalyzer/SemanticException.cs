using System.Collections.Generic;
using MUTDOD.Common.ModuleBase;

namespace MUTDOD.Server.Common.QueryAnalyzer
{
    public class SemanticException : QuerySemanticException
    {
        internal List<SemanticExceptionItem> exceptionList;

        public override IEnumerable<IQuerySemanticExceptionItem> ExceptionList
        {
            get
            {
                return this.exceptionList ?? new List<SemanticExceptionItem>();
            }
        }

        public SemanticException()
            : base("For details check exception list below:")
        {
        }
    }
}
