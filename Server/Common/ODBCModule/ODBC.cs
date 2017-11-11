using System.Collections.Concurrent;
using System.ServiceModel;
using MUTDOD.Common.ModuleBase;
using MUTDOD.Common.Types;

namespace MUTDOD.Server.Common.ODBCModule
{
    public class ODBC : Module, IOdbc
    {
        public ODBC()
        {
            CentralServersConnections = new ConcurrentDictionary<Oid, ServiceHost>();
            DataServersConnections = new ConcurrentDictionary<Oid, ServiceHost>();
        }

        public string Name
        {
            get { return Constant.Name; }
        }

        private static class Constant
        {
            public const string Name = "ODBC";
        }

        public ConcurrentDictionary<Oid, ServiceHost> CentralServersConnections
        {
            get;
            private set;
        }

        public ConcurrentDictionary<Oid, ServiceHost> DataServersConnections
        {
            get;
            private set;
        }

        public bool ConnectToCentralServer(string serverAddress)
        {
            bool isConnected = false;

            return isConnected;
        }

        public bool ConnectToDataServer(string serverAddress)
        {
            bool isConnected = false;



            return isConnected;
        }
    }
}
