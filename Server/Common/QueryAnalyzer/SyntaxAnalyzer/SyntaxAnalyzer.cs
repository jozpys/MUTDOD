using System.IO;
using PerCederberg.Grammatica.Runtime;

namespace MUTDOD.Server.Common.QueryAnalyzer.SyntaxAnalyzer
{
    internal class SyntaxAnalyzer
    {
       
        public QueryTree CheckQuerySyntax(string query)
        {
            Parser parser = new MUTDODQLParser((new StringReader(query)));

            try
            {
                Node n = parser.Parse();
                return this.GenerateQueryTree(n);
            }
            catch (ParserLogException ex)
            {
                throw new SyntaxException(ex.Message);
            }
        }


        private QueryTree GenerateQueryTree(Node n)
        {
            QueryTree querryTree = new QueryTree();
            Token t = null;

            if (n.GetChildCount() > 0)
            {
                querryTree.TokenName = n.Name;
                querryTree.ProductionsList = new SubTrees();

                for (int i = 0; i < n.GetChildCount(); i++)
                {
                    querryTree.ProductionsList.Add(GenerateQueryTree(n.GetChildAt(i)));
                }
            }
            else
            {
                t=(Token)n;
                querryTree.TokenName = t.Name;
                querryTree.TokenValue = t.Image;
                querryTree.TokenLine = t.StartLine;
                querryTree.TokenCol = t.StartColumn;
            }
            return querryTree;
        }
    }
}
