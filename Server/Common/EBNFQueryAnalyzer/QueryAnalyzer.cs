using MUTDOD.Common.ModuleBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUTDOD.Common;
using MUTDOD.Common.ModuleBase.Storage.Core.Metadata;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using MUTDOD.Common.ModuleBase.Communication;

namespace MUTDOD.Server.Common.EBNFQueryAnalyzer
{
    public class EBNFQueryAnalyzer : Module, IQueryAnalyzer
    {
        public IQueryElement ParseQuery(IQuery inputQuery)
        {
            AntlrInputStream input = new AntlrInputStream(inputQuery.QueryText);
            QueryGrammarLexer lexer = new QueryGrammarLexer(input);
            CommonTokenStream tokens = new CommonTokenStream(lexer);

            QueryGrammarParser parser = new QueryGrammarParser(tokens);
            parser.AddErrorListener(new QueryParserErrorListener());
            IParseTree tree = parser.start();
            IQueryElement queryTree = new QueryVisitor().Visit(tree);
            return queryTree;
        }

        public string Name => "EBNFQueryAnalyzer";
    }
}
