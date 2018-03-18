using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Discovery;
using System.Threading;
using System.Xml.Serialization;
using MUTDOD.Common;
using MUTDOD.Common.Communication;
using MUTDOD.Common.Settings;
using MUTDOD.Server.Common.CoreModule.Communication;
using OdraIDE.Core.Services;

namespace OdraIDE.Core.Connection
{
    [PartCreationPolicy(CreationPolicy.Shared)]
    [Export(Services.Connection.ConnectionService, typeof(IConnectionService))]
    public class ConnectionService : IConnectionService
    {
        private static readonly ISettingsManager settingsManager = new HardcodedSettings();
        private static readonly IQuery _getSystemInfo = InternalQueries.SystemInfoQuery;
        private static readonly DatabaseInfo _databaseInfoForSystemInfoQuery = new DatabaseInfo(){Name = string.Empty};
        public event EventHandler Connected;
        public event EventHandler Disconnected;
        public event EventHandler DatabasesChanged;
        //public event EventHandler NewDatabaseCreated;
        public event EventHandler<IsExecutingEventArgs> IsExecutingChanged;

        private BackgroundWorker m_worker = new BackgroundWorker();
        private ICentralServerContract m_serverChanel;

        [Import(Messaging.MessagingService, typeof(IMessagingService))]
        private IMessagingService messageService { get; set; }

        public struct QueryStruct
        {
            public DatabaseInfo DbName { get; set; }
            public IQuery Query { get; set; }
        }

        ICentralServerContract IConnectionService.Connect()
        {
            if (m_serverChanel == null)
            {
                
                //EndpointAddress endPointAddress = FindMyServiceAddress();

                ChannelFactory<ICentralServerContract> scf = new ChannelFactory<ICentralServerContract>(settingsManager.CentralServerRemoteBinding, settingsManager.CentralServerRemoteAdress);
                
                m_serverChanel = scf.CreateChannel();

                //DiscoveryClient dicovery
                (m_serverChanel as IContextChannel).OperationTimeout = TimeSpan.FromHours(5);

                Connected(this, EventArgs.Empty);
            }

            return m_serverChanel;
        }

        private EndpointAddress FindMyServiceAddress()
        {
            // Create DiscoveryClient
            DiscoveryClient discoveryClient = new DiscoveryClient(new UdpDiscoveryEndpoint());

            // Create FindCriteria
            FindCriteria findCriteria = new FindCriteria(typeof(ICentralServerContract));
            findCriteria.MaxResults = 1;

            // Find IMyService endpoints
            FindResponse findResponse = discoveryClient.Find(findCriteria);

            Console.WriteLine("Found {0} IMyService endpoint(s).", findResponse.Endpoints.Count);

            if (findResponse.Endpoints.Count > 0)
            {
                return findResponse.Endpoints[0].Address;
            }
            else
            {
                return null;
            }
        }

        public void Disconnect()
        {
            //TODO obsluga laczenia i zrywania polaczenia, przechwytywanie wyjatkow itd.
            m_serverChanel = null;
            Disconnected(this, EventArgs.Empty);
        }

        public void GetSystemInfo(GetSystemInfoCompleted getSystemInfoCompleted)
        {
            if (m_serverChanel == null)
            {
                throw new NoConnectionException();
            }

            if (!m_worker.IsBusy)
            {
                m_worker = new BackgroundWorker();
                m_worker.WorkerSupportsCancellation = true;
                m_worker.DoWork += new DoWorkEventHandler(DoExecuteQuery);

                m_worker.RunWorkerCompleted += delegate(object s, RunWorkerCompletedEventArgs e)
                {
                    var qr = e.Result as IQueryResult;
                    IsExecutingChanged(this, new IsExecutingEventArgs(false));
                    //TODO odbsluga wyjatkow
                    if (e.Error != null)
                    {
                        messageService.ShowMessage(e.Error.Message, "Error");
                        getSystemInfoCompleted(ExecuteQueryStatus.Error, null, qr);
                        Disconnected(this, EventArgs.Empty);
                    }
                    else if (e.Cancelled)
                    {
                        getSystemInfoCompleted(ExecuteQueryStatus.Canceled, null, qr);
                    }
                    else
                    {
                        if (qr.QueryResultType == ResultType.SystemInfo)
                        {
                            StringReader txtR = new StringReader(qr.StringOutput);
                            XmlSerializer xmlSerializer = new XmlSerializer(typeof (SystemInfo));
                            SystemInfo systemInfo = xmlSerializer.Deserialize(txtR) as SystemInfo;

                            Databases = systemInfo.Databases.Select(d => d.Name).ToList();

                            getSystemInfoCompleted(ExecuteQueryStatus.Done, systemInfo, qr);
                        }
                        else if (qr.QueryResultType == ResultType.StringResult)
                        {
                            messageService.ShowMessage(qr.StringOutput, "Unexpected result");
                            getSystemInfoCompleted(ExecuteQueryStatus.Error, null, qr);
                        }
                        else
                        {
                            messageService.ShowMessage(String.Format("[{0}]", qr.QueryResultType),
                                "Unexpected result");
                            getSystemInfoCompleted(ExecuteQueryStatus.Error, null, qr);
                        }
                    }

                };

                //wraps args to query structure
                QueryStruct queryStruct = new QueryStruct();
                queryStruct.DbName = _databaseInfoForSystemInfoQuery;
                queryStruct.Query = _getSystemInfo;

                IsExecutingChanged(this, new IsExecutingEventArgs(true));
                m_worker.RunWorkerAsync(queryStruct);
            }
        }

        public void CreateNewDatabase(DatabaseInfo dbName, CreateNewDatabasCompleted createNewDatabasCompleted)
        {
            if (m_serverChanel == null)
            {
                throw new NoConnectionException();
            }

            if (!m_worker.IsBusy)
            {
                m_worker = new BackgroundWorker();
                m_worker.DoWork += new DoWorkEventHandler(DoExecuteQuery);

                m_worker.RunWorkerCompleted += delegate(object s, RunWorkerCompletedEventArgs e)
                {
                    var qr = e.Result as IQueryResult;
                    IsExecutingChanged(this, new IsExecutingEventArgs(false));
                    //TODO odbsluga wyjatkow
                    if (qr == null || qr.QueryResultType != ResultType.DatabaseInfo)
                    {
                        createNewDatabasCompleted(ExecuteQueryStatus.Error, dbName, qr);
                    }
                    else if (e.Error != null)
                    {
                        messageService.ShowMessage(e.Error.Message, "Error");
                        createNewDatabasCompleted(ExecuteQueryStatus.Error, dbName, qr);
                        Disconnected(this, EventArgs.Empty);
                    }
                    else
                    {
                        createNewDatabasCompleted(ExecuteQueryStatus.Done, dbName,qr);
                        Databases.Add(dbName.Name);
                        DatabasesChanged(this, EventArgs.Empty);
                    }

                };

                //wraps args to query structure
                QueryStruct queryStruct = new QueryStruct();
                queryStruct.DbName = dbName;
                queryStruct.Query = InternalQueries.CreateDatabaseQuery(dbName.Name);

                IsExecutingChanged(this, new IsExecutingEventArgs(true));
                m_worker.RunWorkerAsync(queryStruct);
            }
        }

        public void RenameDatabase(DatabaseInfo dbName, string newDatabaseName, RenameDatabaseNameCompleted renameDatabaseNameCompleted)
        {
            if (m_serverChanel == null)
            {
                throw new NoConnectionException();
            }

            if (!m_worker.IsBusy)
            {
                m_worker = new BackgroundWorker();
                m_worker.DoWork += new DoWorkEventHandler(DoExecuteQuery);

                m_worker.RunWorkerCompleted += delegate (object s, RunWorkerCompletedEventArgs e)
                {
                    var qr = e.Result as IQueryResult;
                    IsExecutingChanged(this, new IsExecutingEventArgs(false));
                    //TODO odbsluga wyjatkow
                    if (qr == null || qr.QueryResultType != ResultType.SystemInfo)
                    {
                        renameDatabaseNameCompleted(ExecuteQueryStatus.Error, dbName, qr);
                    }
                    else if (e.Error != null)
                    {
                        messageService.ShowMessage(e.Error.Message, "Error");
                        renameDatabaseNameCompleted(ExecuteQueryStatus.Error, dbName, qr);
                        Disconnected(this, EventArgs.Empty);
                    }
                    else
                    {
                        renameDatabaseNameCompleted(ExecuteQueryStatus.Done, dbName, qr);
                        Databases.Add(dbName.Name);
                        DatabasesChanged(this, EventArgs.Empty);
                    }

                };

                //wraps args to query structure
                QueryStruct queryStruct = new QueryStruct();
                queryStruct.DbName = dbName;
                queryStruct.Query = InternalQueries.RenameDatabaseQuery(dbName.Name, newDatabaseName);

                IsExecutingChanged(this, new IsExecutingEventArgs(true));
                m_worker.RunWorkerAsync(queryStruct);
            }
        }


        public void ExecuteQuery(DatabaseInfo dbName, IQuery query, ExecuteQueryCompleted executeQueryCompleted)
        {
            if (m_serverChanel == null)
            {
                throw new NoConnectionException();
            }

            if (!m_worker.IsBusy)
            {
                m_worker = new BackgroundWorker();
                m_worker.WorkerSupportsCancellation = true;
                m_worker.DoWork += new DoWorkEventHandler(DoExecuteQuery);

                m_worker.RunWorkerCompleted += delegate(object s, RunWorkerCompletedEventArgs e)
                {

                    IsExecutingChanged(this, new IsExecutingEventArgs(false));
                    if (e.Error != null)
                    {
                        messageService.ShowMessage(e.Error.Message, "Error");
                        executeQueryCompleted(ExecuteQueryStatus.Error, null);
                        Disconnected(this, EventArgs.Empty);
                    }
                    else if (e.Cancelled)
                    {
                        executeQueryCompleted(ExecuteQueryStatus.Canceled, null);
                    }
                    else
                    {
                        var qr = e.Result as IQueryResult;
                        executeQueryCompleted(ExecuteQueryStatus.Done, qr);
                    }
                };

                //wraps args to query structure
                QueryStruct queryStruct = new QueryStruct();
                queryStruct.DbName = dbName;
                queryStruct.Query = query;

                IsExecutingChanged(this, new IsExecutingEventArgs(true));
                m_worker.RunWorkerAsync(queryStruct);
            }
        }

        void DoExecuteQuery(object sender, DoWorkEventArgs e)
        {
            QueryStruct queryStruct = (QueryStruct)e.Argument;

            try
            {
                e.Result = m_serverChanel.ExecuteQuery(new DatabaseInfo{Name = queryStruct.DbName.Name},
                    new DTOQuery(queryStruct.Query));
            }
            catch (EndpointNotFoundException)
            {
                Disconnect();
                throw;
            }
            catch (CommunicationException)
            {
                Disconnect();
                throw;
            }
            if (m_worker.WorkerSupportsCancellation)
            {
                while (true)
                {
                    if (m_worker.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }
                    if (e.Result != null)
                    {
                        return;
                    }
                    Thread.Sleep(10);
                }
            }
            
        }

        public void CancelExecutingQuery()
        {
            m_worker.CancelAsync();
        }

        public bool IsConnected
        {
            get { return m_serverChanel != null; }
        }

        private IList m_Databases;

        public IList Databases
        {
            get 
            {
                if (m_serverChanel == null)
                {
                    throw new NoConnectionException();
                }
                return m_Databases;
            }

            private set
            {
                if (m_Databases != value)
                {
                    m_Databases = value;
                    DatabasesChanged(this, EventArgs.Empty);
                }
            }
        }        
    }
}
