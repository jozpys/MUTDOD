using MUTDOD.Common.ModuleBase;

namespace MUTDOD.Server.Common.QueryAnalyzer
{
    public class SemanticExceptionItem : IQuerySemanticExceptionItem
    {
        public string name { get; internal set; }
        public string message { get; internal set; }
        public int line { get; internal set; }
        public int col { get; internal set; }


        public SemanticExceptionItem(string _name, string _message, int _line, int _col)
        {
            this.name = _name;
            this.message = _message;
            this.line = _line;
            this.col = _col;
        }

        public override string ToString()
        {
            return name + " (" + line + "," + col + ") " + message;
        }
    }
}
