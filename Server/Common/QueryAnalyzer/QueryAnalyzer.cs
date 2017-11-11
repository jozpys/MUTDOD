using MUTDOD.Common;
using MUTDOD.Common.ModuleBase;
using MUTDOD.Common.ModuleBase.Storage.Core.Metadata;
using MUTDOD.Server.Common.QueryAnalyzer.MetamodelHelper;

namespace MUTDOD.Server.Common.QueryAnalyzer 
{
    public class QueryAnalyzer : Module, IQueryAnalyzer
    {
        public QueryAnalyzer()
        {
        }

        public IQueryTree ParseQuery(IQuery inputQuery, IDatabaseSchema databaseSchema)
        {
            SyntaxAnalyzer.SyntaxAnalyzer syntaxAlr = new SyntaxAnalyzer.SyntaxAnalyzer();
            QueryTree qt = syntaxAlr.CheckQuerySyntax(inputQuery.QueryText);
            
            SemanticAnalyzer.SemanticAnalyzer semanticAlr = new SemanticAnalyzer.SemanticAnalyzer();
            semanticAlr.CheckQuerySemantic(qt, new Database(databaseSchema));

            return qt;
        }
        public string Name
        {
            get { return "QueryAnalyzer"; }
        }
    }
}
