using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUTDOD.Common
{
    public class ServerInfo
    {
        public string ServerAddress { get; set; }
        public string ServerName { get; set; }
        public short ServerVersion { get; set; }
        public short ServerBuild { get; set; }
        public ServerStates ServerState { get; set; }
        public ServerTypes ServerType { get; set; } 
    }
}
