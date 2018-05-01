using MUTDOD.Common;

namespace OdraIDE.Editor.Sbql.Commands
{
    public class SbqlQuery : IQuery
    {
        public SbqlQuery(string query)
        {
            QueryText = query;
        }
        public string QueryText { get; private set; }
    }
}
