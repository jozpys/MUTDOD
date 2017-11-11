using System.Collections.Generic;
using MUTDOD.Server.Common.QueryAnalyzer.SyntaxAnalyzer;

namespace MUTDOD.Server.Common.QueryAnalyzer.MetamodelHelper
{
    internal class Class
    {
        public string name;

        public MUTDODQLProtectionLevel protectionLevel;

        public bool isGeneric;

        public List<Field> fields;

        public List<Method> methods;
    }
}
