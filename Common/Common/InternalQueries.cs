namespace MUTDOD.Common
{
    public static class InternalQueries
    {
        public static readonly IQuery SystemInfoQuery = new SystemInfoQuery();

        public static IQuery CreateDatabaseQuery(string dbName)
        {
            return new CreateDatabaseQuery(dbName);
        }
    }

    internal class SystemInfoQuery : IQuery
    {
        public string QueryText
        {
            get { return "@SystemInfo;"; }
        }
    }

    internal class CreateDatabaseQuery : IQuery
    {
        private readonly string _dbName;

        internal CreateDatabaseQuery(string dbName)
        {
            _dbName = dbName;
        }

        public string QueryText
        {
            get { return string.Format("@CreateDatabase {0};", _dbName); }
        }
    }
}
