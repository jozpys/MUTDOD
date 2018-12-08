﻿using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;
using MUTDOD.Common;
using MUTDOD.Common.ModuleBase;
using MUTDOD.Common.ModuleBase.Storage.Core.Metadata;
using MUTDOD.Common.ServerBase;
using MUTDOD.Common.Settings;
using MUTDOD.Server.Common.CoreModule.Communication;
using MUTDOD.Server.DataServer.DSODBC;

namespace MUTDOD.Server.DataServer.DataServerBase
{
    public class DataServerRunnable : ServerRunnable
    {
        private readonly ISettingsManager _settingsManager;
        private readonly IStorage _storage;
        private readonly ILogger _logger;

        public DataServerRunnable(ICore core, ISettingsManager settingsManager, IStorage storage, ILogger logger) : base(core, logger)
        {
            _settingsManager = settingsManager;
            _storage = storage;
            _logger = logger;
            do
            {
                Port = Convert.ToInt16(DateTime.Now.Millisecond % 10000);
                _addres = string.Format("net.tcp://localhost:{0}", Port);
            } while (_addres == settingsManager.CentralServerRemoteAdress);

            InitServiceHost();
        }

        private void InitServiceHost()
        {
            var dataServerConnector = new DataServerConnector(_settingsManager, _storage, _logger);

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

        public override short Port { get; set; }

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
           
            bool done = false;
            do
            {
                 try
                {
                _serviceHost.Open();
                ChannelFactory<ICentralServerContract> scf =
                    new ChannelFactory<ICentralServerContract>(_settingsManager.CentralServerRemoteBinding,
                        _settingsManager.CentralServerRemoteAdress);

                var m_serverChanel = scf.CreateChannel();

               
                    m_serverChanel.RegisterDataServer(Name, Adress);
                    done = true;
                }
                catch (CommunicationObjectFaultedException)
                {
                    _logger.Log("DataServer", "Faulted communication! Wating for recall", MessageLevel.Warning);
                    Thread.Sleep(3000);
                }
                catch (Exception)
                {
                    _logger.Log("DataServer", "Can not connect to Central server! Wating for recall",
                        MessageLevel.Warning);
                    Thread.Sleep(3000);
                }

                if (!done)
                {
                    do
                    {
                        Port += 1;
                        _addres = string.Format("net.tcp://localhost:{0}", Port);
                    } while (_addres == _settingsManager.CentralServerRemoteAdress);
                    InitServiceHost();
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
