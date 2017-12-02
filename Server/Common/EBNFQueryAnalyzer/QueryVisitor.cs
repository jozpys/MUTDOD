using MUTDOD.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

namespace MUTDOD.Server.Common.EBNFQueryAnalyzer
{
    class QueryVisitor : QueryGrammarBaseVisitor<IQueryTree>
    {
        public override IQueryTree VisitStart([NotNull] QueryGrammarParser.StartContext context)
        {
            return Visit(context.statement());
        }
        public override IQueryTree VisitStatement([NotNull] QueryGrammarParser.StatementContext context)
        {
            QueryTree statementTree = new QueryTree();
            statementTree.TokenName = "STATEMENT";

            statementTree.ProductionsList = new SubTrees();
            IQueryTree system_operation = Visit(context.system_operation());
            statementTree.ProductionsList.Add(system_operation);
            statementTree.ProductionsList.Add(createSemicolonTree());
            return statementTree;
        }

        public override IQueryTree VisitSystem_operation([NotNull] QueryGrammarParser.System_operationContext context)
        {
            QueryTree systemOpertionTree = new QueryTree();
            systemOpertionTree.TokenName = "SYSTEM_OPERATION";

            systemOpertionTree.ProductionsList = new SubTrees();
            if(context.op.Type == QueryGrammarParser.SYS_INFO)
            {
                QueryTree systemInfoTree = new QueryTree();
                systemInfoTree.TokenName = "GET_SYSTEM_INFO";
                systemOpertionTree.ProductionsList.Add(systemInfoTree);
            }

            return systemOpertionTree;
        }

        private IQueryTree createSemicolonTree()
        {
            QueryTree semicolonTree = new QueryTree();
            semicolonTree.TokenName = "SEMICOLON";
            semicolonTree.TokenValue = ";";
            return semicolonTree;
        }
    }
}
