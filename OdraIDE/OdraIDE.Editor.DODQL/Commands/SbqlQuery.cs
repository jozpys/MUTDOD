using MUTDOD.Common;

namespace OdraIDE.Editor.Sbql.Commands
{
    internal class SbqlQuery : IQuery
    {
        public SbqlQuery(string query)
        {
            QueryText = query;
        }
        public string QueryText { get; private set; }
    }
}
