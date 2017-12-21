using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUTDOD.Common;
using MUTDOD.Common.Communication;
using System.IO;
using System.Xml.Serialization;
using MUTDOD.Common.ModuleBase.Communication;

namespace MUTDOD.Server.Common.EBNFQueryAnalyzer
{
    public class SystemInformation : AbstractLeaf
    {
        public SystemInformation() : base(ElementType.SYSTEM_INFO) { }
        public override QueryDTO Execute(QueryParameters parameters)
        {
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            var xmlSerializer = new XmlSerializer(typeof(SystemInfo));
            xmlSerializer.Serialize(sw, parameters.SystemInfo);
            var result = new DTOQueryResult()
            {
                NextResult = null,
                QueryResults = null,
                QueryResultType = ResultType.SystemInfo,
                StringOutput = sb.ToString()
            };
            return new QueryDTO { Result = result };
        }
    }
}
