using System;
using System.Collections;
using MUTDOD.Common;
using MUTDOD.Common.ModuleBase.Communication;
using MUTDOD.Server.Common.CoreModule.Communication;

namespace OdraIDE.Core
{
    public interface IConnectionService
    {
        event EventHandler Connected;
        event EventHandler Disconnected;
        event EventHandler DatabasesChanged;
        //event EventHandler NewDatabaseCreated;
        event EventHandler<IsExecutingEventArgs> IsExecutingChanged;

        ICentralServerContract Connect();
        void Disconnect();
        bool IsConnected { get; }
        void GetSystemInfo(GetSystemInfoCompleted getSystemInfoCompleted);
        void CreateNewDatabase(DatabaseInfo dbName, CreateNewDatabasCompleted createNewDatabasCompleted);
        void RenameDatabase(DatabaseInfo dbName, string newDatabaseName, RenameDatabaseNameCompleted createNewDatabasCompleted);
        void DeleteDatabase(DatabaseInfo dbName, DeleteDatabaseCompleted createNewDatabasCompleted);
        void ExecuteQuery(DatabaseInfo dbName, IQuery query, ExecuteQueryCompleted executeQueryCompleted);
        void CancelExecutingQuery();
        IList Databases { get; }
        SystemInfo SystemInfo { get; }
    }

    public delegate void ExecuteQueryCompleted(ExecuteQueryStatus status, IQueryResult result);
    public delegate void GetSystemInfoCompleted(ExecuteQueryStatus status, SystemInfo systemInfo, IQueryResult result);
    public delegate void CreateNewDatabasCompleted(ExecuteQueryStatus status, DatabaseInfo database, IQueryResult result);
    public delegate void RenameDatabaseNameCompleted(ExecuteQueryStatus status, DatabaseInfo systemInfo, IQueryResult result);
    public delegate void DeleteDatabaseCompleted(ExecuteQueryStatus status, DatabaseInfo database, IQueryResult result);

    public enum ExecuteQueryStatus
    {
        Done,
        Canceled,
        Error
    }

    public class IsExecutingEventArgs : EventArgs
    {
        private bool m_IsExecuting;
        public bool IsExecuting { get { return m_IsExecuting; } }

        public IsExecutingEventArgs(bool isExecuting)
        {
            m_IsExecuting = isExecuting;
        }
    }
}
