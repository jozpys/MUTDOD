using System.ComponentModel;
using MUTDOD.Common;

namespace OdraIDE.SolutionExplorer
{
    [DefaultProperty("Name")]
    public class DataServerProperties
    {
        public static DataServerProperties From(ServerInfo serverInfo)
        {
            DataServerProperties prop = new DataServerProperties();
            prop.Address = serverInfo.ServerAddress;
            prop.Build = serverInfo.ServerBuild;
            prop.Name = serverInfo.ServerName;
            prop.Type = serverInfo.ServerType;
            prop.Version = serverInfo.ServerVersion;
            prop.State = serverInfo.ServerState;

            return prop;
        }

        [Category("Data Server"),
        Description("Address"),
        ReadOnly(true)]
        public string Address { get; set; }

        [Category("Data Server"),
        Description("Name"),
        ReadOnly(true)]
        public string Name { get; set; }

        [Category("Data Server"),
        Description("Version"), ReadOnly(true)]
        public short Version { get; set; }

        [Category("Data Server"),
        Description("Build"),
        ReadOnly(true)]
        public short Build { get; set; }

        [Category("Data Server"),
        Description("State"),
        ReadOnly(true)]
        public ServerStates State { get; set; }

        [Category("Data Server"),
        Description("Type"),
        ReadOnly(true)]
        public ServerTypes Type { get; set; }
    }
}
