using System;
using System.ServiceModel;
using MUTDOD.Common;
using MUTDOD.Common.Communication;
using MUTDOD.Common.ModuleBase.Communication;
using MUTDOD.Common.Settings;

namespace TestMultiClient
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var settingsManager = new HardcodedSettings();
            ChannelFactory<ICentralServerContract> scf =
                new ChannelFactory<ICentralServerContract>(settingsManager.CentralServerRemoteBinding,
                    settingsManager.CentralServerRemoteAdress);

            var m_serverChanel = scf.CreateChannel();

            (m_serverChanel as IContextChannel).OperationTimeout = TimeSpan.FromHours(5);

            m_serverChanel.ExecuteQuery(new DatabaseInfo {Name = "TestDB"},
                new DTOQuery(InternalQueries.SystemInfoQuery));
        }
    }
}
