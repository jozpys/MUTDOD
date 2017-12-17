using Antlr4.Runtime;
using MUTDOD.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUTDOD.Server.Common.EBNFQueryAnalyzer
{
    class QueryParserErrorListener : BaseErrorListener, IAntlrErrorListener<IQueryTree>
    {
        public override void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            throw new SyntaxException("Line " + line + ":" + charPositionInLine + " " + msg);
        }

        public void SyntaxError(IRecognizer recognizer, IQueryTree offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            throw new SyntaxException("Line " + line + ":" + charPositionInLine + " " + msg);
        }
    }
}
