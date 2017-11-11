using System.Collections.Generic;
using MUTDOD.Server.Common.QueryAnalyzer.MetamodelHelper;

namespace MUTDOD.Server.Common.QueryAnalyzer.SemanticAnalyzer
{
    internal class Environment
    {
        public Environment parent;
        public List<Class> classList;
        public List<string> labelList;
        public Dictionary<string, string> fields;
        public Section section;
    }
}
