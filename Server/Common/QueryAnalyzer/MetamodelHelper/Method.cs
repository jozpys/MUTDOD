using System.Collections.Generic;
using MUTDOD.Server.Common.QueryAnalyzer.SyntaxAnalyzer;

namespace MUTDOD.Server.Common.QueryAnalyzer.MetamodelHelper
{
    internal class Method
    {
        public string name;

        public MUTDODQLProtectionLevel protectionLevel;

        public string returnedType;

        public List<Param> parameters;
    }
}
