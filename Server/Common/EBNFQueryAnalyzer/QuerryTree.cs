using System.Collections.Generic;
using MUTDOD.Common;

namespace MUTDOD.Server.Common.EBNFQueryAnalyzer

{
    internal class QueryTree : IQueryTree
    {
        public string TokenName { get; set; }
        public string TokenValue { get; set; }
        public int TokenLine { get; set; }
        public int TokenCol { get; set; }
        public ISubTrees ProductionsList { get; set; }

        public QueryTree()
        {
            this.TokenName = string.Empty;
            this.TokenValue = string.Empty;
            this.TokenLine = 0;
            this.TokenCol = 0;
            this.ProductionsList = null;
        }
    }

    public class SubTrees : List<IQueryTree>, ISubTrees { }
}
