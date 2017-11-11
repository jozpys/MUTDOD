using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;
using MUTDOD.Common;
using MUTDOD.Common.ModuleBase;
using MUTDOD.Common.ModuleBase.Communication;
using MUTDOD.Common.ServerBase;
using MUTDOD.Common.Settings;
using MUTDOD.Server.DataServer.DSODBC;

namespace MUTDOD.Server.DataServer.DataServerBase
{
    public class DataServerRunnable : ServerRunnable
    {
        private readonly ISettingsManager _settingsManager;
        private readonly ILogger _logger;

        public DataServerRunnable(ICore core, ISettingsManager settingsManager, ILogger logger) : base(core, logger)
        {
            _settingsManager = settingsManager;
            _logger = logger;
            do
            {
                _addres = string.Format("net.tcp://localhost:{0}", DateTime.Now.Millisecond%10000);
            } while (_addres == settingsManager.CentralServerRemoteAdress);

            var dataServerConnector = new DataServerConnector(logger);

            _serviceHost = new ServiceHost(
                dataServerConnector,
                new Uri(Adress));
            //_serviceHost.
            _serviceHost.AddServiceEndpoint(
                typeof(IDataServerContract),
                _settingsManager.CentralServerRemoteBinding,
                "");
        }

        private string _addres;

        #region Implementation of Server

        public override string Name { get; set; }

        public override string Adress
        {
            get { return _addres; }
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
            Name = string.Format("DataServer_{0}", Guid.NewGuid());
            Core.Init(new Dictionary<string, object>());
        }
        private ServiceHost _serviceHost;

        protected override void RunMain()
        {
            base.RunMain();
            _serviceHost.Open();
            ChannelFactory<ICentralServerContract> scf =
                new ChannelFactory<ICentralServerContract>(_settingsManager.CentralServerRemoteBinding,
                    _settingsManager.CentralServerRemoteAdress);

            var m_serverChanel = scf.CreateChannel();
            bool done = false;
            do
            {
                try
                {
                    m_serverChanel.RegisterDataServer(Name, Adress);
                    done = true;
                }
                catch (CommunicationObjectFaultedException)
                {
                    _logger.Log("DataServer", "Faulted communication! Wating for recall", MessageLevel.Warning);
                    m_serverChanel = scf.CreateChannel();
                    Thread.Sleep(3000);
                }
                catch (Exception)
                {
                    _logger.Log("DataServer", "Can not connect to Central server! Wating for recall",
                        MessageLevel.Warning);
                    Thread.Sleep(3000);
                }
            } while (!done);
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
