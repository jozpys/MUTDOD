namespace MUTDOD.Common
{
    public static class InternalQueries
    {
        public static readonly IQuery SystemInfoQuery = new SystemInfoQuery();

        public static IQuery CreateDatabaseQuery(string dbName)
        {
            return new CreateDatabaseQuery(dbName);
        }

        public static IQuery RenameDatabaseQuery(string dbName, string newName)
        {
            return new RenameDatabaseQuery(dbName, newName);
        }

        public static IQuery DropDatabaseQuery(string dbName)
        {
            return new DropDatabaseQuery(dbName);
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

    internal class RenameDatabaseQuery : IQuery
    {
        private readonly string _dbName;
        private readonly string _newName;

        internal RenameDatabaseQuery(string dbName, string newName)
        {
            _dbName = dbName;
            _newName = newName;
        }

        public string QueryText
        {
            get { return string.Format("@RenameDatabase {0}, {1};", _dbName, _newName); }
        }
    }

    internal class DropDatabaseQuery : IQuery
    {
        private readonly string _dbName;

        internal DropDatabaseQuery(string dbName)
        {
            _dbName = dbName;
        }

        public string QueryText
        {
            get { return string.Format("@DropDatabase {0};", _dbName); }
        }
    }
}
