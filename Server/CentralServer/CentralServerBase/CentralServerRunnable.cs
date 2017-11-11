using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;
using MUTDOD.Common;
using MUTDOD.Common.ModuleBase;
using MUTDOD.Common.ModuleBase.Communication;
using MUTDOD.Common.ServerBase;
using MUTDOD.Common.Settings;
using MUTDOD.Server.CentralServer.CSODBC;

namespace MUTDOD.Server.CentralServer.CentralServerBase
{
    public class CentralServerRunnable : ServerRunnable
    {
        private readonly ISettingsManager _settingsManager;

        public CentralServerRunnable(ICore core, ISettingsManager settingsManager, ICentralServerEngine queryEngine, IStorage storage, ILogger logger)
            : base(core,logger)
        {
            _settingsManager = settingsManager;

            var centralServerContract = new CentralServerConnector(queryEngine,storage, logger,settingsManager);
            centralServerContract.CentralServerInfo = new ServerInfo
            {
                ServerAddress = Adress,
                ServerName = Name,
                ServerState = ServerStates.Running,
                ServerType = ServerTypes.DataServer,
                ServerBuild = 1,
                ServerVersion = 1
            };

            _serviceHost = new ServiceHost(
                centralServerContract,
                new Uri(Adress));
            //_serviceHost.
            _serviceHost.AddServiceEndpoint(
                typeof (ICentralServerContract),
                _settingsManager.CentralServerRemoteBinding,
                "");
        }

        #region Implementation of Server

        public override string Name { get; set; }

        public override string Adress
        {
            get { return _settingsManager.CentralServerRemoteAdress; }
        }

        public override short Port
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        #endregion

        protected override void InitMain()
        {
            base.InitMain();

            //mgor: Ten fragment kodu trzeba zastąpić ładowaniem ustawień z SettingsManagera
            Name = "CentralServer_" + Guid.NewGuid().ToString();

            //mgor: Podać listę parametrów
            Core.Init(new Dictionary<string, object>());
        }

        private ServiceHost _serviceHost;

        protected override void RunMain()
        {
            base.RunMain();
            _serviceHost.Open();
            Thread.Sleep(500);
        }

        protected override void StopMain()
        {
            base.StopMain();
            if (_serviceHost != null && _serviceHost.State == CommunicationState.Opened)
            {
                _serviceHost.Close();
                _serviceHost = null;
            }

            Thread.Sleep(500);
        }
    }
}
