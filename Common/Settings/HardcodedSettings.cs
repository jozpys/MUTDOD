using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace MUTDOD.Common.Settings
{
    public class HardcodedSettings : ISettingsManager
    {
        public string CentralServerRemoteAdress
        {
            get { return "net.tcp://localhost:1599"; }
        }

        public Binding CentralServerRemoteBinding
        {
            get
            {
                NetTcpBinding binding = new NetTcpBinding();
                binding.SendTimeout = new TimeSpan(0, 10, 0);
                binding.ReceiveTimeout = new TimeSpan(0, 10, 0);

                NetTcpSecurity security = new NetTcpSecurity();
                security.Mode = SecurityMode.None;

                TcpTransportSecurity transport = new TcpTransportSecurity();
                transport.ClientCredentialType = TcpClientCredentialType.None;
                security.Transport = transport;

                MessageSecurityOverTcp message = new MessageSecurityOverTcp();
                message.ClientCredentialType = MessageCredentialType.None;
                security.Message = message;

                binding.Security = security;

                return binding;
            }

        }

        public StorageStrategy StorageStrategy
        {
            get { return StorageStrategy.Speed; }
        }

        public string DataBaseDirectory { get { return "DataStorage"; } }
        public string LogFileDirectory { get { return "Logs\\"; } }
    }
}
