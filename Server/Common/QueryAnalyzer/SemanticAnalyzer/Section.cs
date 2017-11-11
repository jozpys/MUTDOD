using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MUTDOD.Server.Common.QueryAnalyzer.SemanticAnalyzer
{
    public enum Section
    {
        Target,
        If_Statement,
	    Try_Catch,
        For_Statement,
	    Foreach_Statement,
	    While_Statement,
        Class_Declaration,
        Method_Declaration,
        Parallel
    }
}
