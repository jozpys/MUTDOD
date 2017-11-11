using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MUTDOD.Common.ModuleBase.Communication
{
    [Serializable]
    public class DTOQueryTree : IQueryTree
    {
        public string TokenName { get; set; }
        public string TokenValue { get; set; }
        public int TokenLine { get; set; }
        public int TokenCol { get; set; }

        public DTOSubTrees _subTrees;

        public ISubTrees ProductionsList
        {
            get { return _subTrees; }
        }

        public override string ToString()
        {
            if (_subTrees == null)
                return TokenValue;

            StringBuilder sb = new StringBuilder();
            _subTrees.ForEach(s => sb.Append(s).Append(" "));
            return sb.ToString();
        }

        public DTOQueryTree(IQueryTree queryTree)
        {
            TokenName = queryTree.TokenName;
            TokenValue = queryTree.TokenValue;
            TokenLine = queryTree.TokenLine;
            TokenCol = queryTree.TokenCol;
            if (queryTree.ProductionsList != null)
                _subTrees = new DTOSubTrees(queryTree.ProductionsList);
        }
    }

    [Serializable]
    public class DTOSubTrees : List<IQueryTree>, ISubTrees
    {
        public DTOSubTrees(ISubTrees subTrees)
        {
            subTrees.ToList().ForEach(s => Add(new DTOQueryTree(s)));
        }
    }
}
