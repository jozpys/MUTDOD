using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using MUTDOD.Common;
using MUTDOD.Common.Communication;
using MUTDOD.Common.ModuleBase;
using MUTDOD.Common.ModuleBase.Communication;
using MUTDOD.Common.Settings;
using MUTDOD.Server.Common.CoreModule.Communication;

namespace MUTDOD.Server.CentralServer.CSODBC
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class CentralServerConnector : ICentralServerContract
    {
        private readonly ICentralServerEngine _queryEngine;
        private readonly IStorage _storage;
        private readonly ILogger _logger;
        private readonly ISettingsManager _settingsManager;
        private object _locker = new object();
        public ServerInfo CentralServerInfo { private get; set; }

        private Dictionary<Guid, ServerInfo> _dataServers;

        private SystemInfo systemInfo
        {
            get
            {
                var si = new SystemInfo();
                si.CentralServer = CentralServerInfo;
                si.Databases = _storage.GetDatabases().Select(d => new DatabaseInfo
                {
                    Name = d.Name,
                    Classes =
                        d.Schema.Classes.Select(
                            c =>
                                new DatabaseClass
                                {
                                    Name = c.Value.Name,
                                    Fields = d.Schema.Properties.Where(p => p.Value.ParentClassId == c.Value.ClassId.Id).Select(p => p.Value.Name).ToList(),
                                    Methods = d.Schema.Methods.ContainsKey(c.Key) ? d.Schema.Methods[c.Key] : new List<string>()
                                }).ToList()
                }).ToList();
                si.DataServer = _dataServers.Values.ToList();

                return si;
            }
        }

        public CentralServerConnector(ICentralServerEngine queryEngine, IStorage storage, ILogger logger, ISettingsManager settingsManager)
        {
            _queryEngine = queryEngine;
            _storage = storage;
            _logger = logger;
            _settingsManager = settingsManager;
            _dataServers = new Dictionary<Guid, ServerInfo>();
        }

        public DTOQueryResult ExecuteQuery(DatabaseInfo dbName, DTOQuery query)
        {
            _logger.Log("CentralServerConnector", string.Format("{0} {1}", dbName, query.QueryText),
                MessageLevel.QueryExecution);
            try
            {
                return
                    new DTOQueryResult(_queryEngine.Execute(dbName.Name, query, systemInfo,
                        qt => RunOnDataServers(dbName, qt)));
            }
            catch (Exception ex)
            {
                return new DTOQueryResult() {QueryResultType = ResultType.StringResult, StringOutput = ex.ToString()};
            }
        }

        private void RunOnDataServers(DatabaseInfo dbName, IQueryElement queryTree)
        {
            _dataServers.Values.ToList().ForEach(s =>
            {
                var scf =
                    new ChannelFactory<IDataServerContract>(_settingsManager.CentralServerRemoteBinding,
                        s.ServerAddress);

                var m_serverChanel = scf.CreateChannel();

                (m_serverChanel as IContextChannel).OperationTimeout = TimeSpan.FromMinutes(1);

                _logger.Log("CentralServerConnector", String.Format("Response from {0}: {1}", s.ServerName,
                    m_serverChanel.ExecuteQuery(dbName, new DTOQueryTree(queryTree)).StringOutput), MessageLevel.QueryExecution);
                scf.Close();
            });
        }

        public string RegisterDataServer(string serverName, string serverAddress)
        {
            lock (_locker)
            {
                var guid = Guid.NewGuid();

                _logger.Log("CentralServerConnector",
                    string.Format("Server [{0}] has been registered on address:\t{1}", serverName, serverAddress),
                    MessageLevel.Operations);

                _dataServers.Add(guid, new ServerInfo
                {
                    ServerAddress = serverAddress,
                    ServerName = serverName,
                    ServerState = ServerStates.Running,
                    ServerType = ServerTypes.DataServer,
                    ServerBuild = 1,
                    ServerVersion = 1
                });

                return guid.ToString();
            }
        }

        public bool IsAvailable()
        {
            return true;
        }
    }
}
