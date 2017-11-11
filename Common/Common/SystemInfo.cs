using System.Collections.Generic;

namespace MUTDOD.Common
{
    public class SystemInfo
    {
        public ServerInfo CentralServer { get; set; }
        public List<ServerInfo> DataServer { get; set; }
        public List<DatabaseInfo> Databases { get; set; }
    }
}
