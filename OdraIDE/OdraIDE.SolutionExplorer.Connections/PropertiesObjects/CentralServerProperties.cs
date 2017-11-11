using System.ComponentModel;
using MUTDOD.Common;

namespace OdraIDE.SolutionExplorer
{
    [DefaultProperty("Name")]
    public class CentralServerProperties
    {
        public static CentralServerProperties From(ServerInfo serverInfo)
        {
            CentralServerProperties prop = new CentralServerProperties();
            prop.Address = serverInfo.ServerAddress;
            prop.Build = serverInfo.ServerBuild;
            prop.Name = serverInfo.ServerName;
            prop.Type = serverInfo.ServerType;
            prop.Version = serverInfo.ServerVersion;
            prop.State = serverInfo.ServerState;

            return prop;
        }

        [Category("Central Server"), 
        Description("Address"),
        ReadOnly(true)]
        public string Address { get; set; }

        [Category("Central Server"),
        Description("Name"), 
        ReadOnly(true)]
        public string Name { get; set; }

        [Category("Central Server"),
        Description("Version"), ReadOnly(true)]
        public short Version { get; set; }

        [Category("Central Server"),
        Description("Build"), 
        ReadOnly(true)]
        public short Build { get; set; }

        [Category("Central Server"),
        Description("State"), 
        ReadOnly(true)]
        public ServerStates State { get; set; }

        [Category("Central Server"),
        Description("Type"), 
        ReadOnly(true)]
        public ServerTypes Type { get; set; }
    }
}
