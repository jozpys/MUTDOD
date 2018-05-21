using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUTDOD.Common;
using MUTDOD.Common.Communication;
using System.Xml.Serialization;
using System.IO;
using MUTDOD.Common.ModuleBase.Communication;
using MUTDOD.Common.ModuleBase.Storage.Core.Metadata;
using System.Runtime.Serialization;

namespace MUTDOD.Server.Common.QueryTree
{
    [DataContract]
    public class CreateDatabase : AbstractLeaf
    {
        public CreateDatabase() : base(ElementType.CREATE_DATABASE) { }
        [DataMember]
        public String DatabaseName { get; set; }

        public override QueryDTO Execute(QueryParameters parameters)
        {
            try
            {
                var did = parameters.Storage.CreateDatabase(new DatabaseParameters(DatabaseName, parameters.SettingsManager));
                parameters.Log(string.Format("new database created as {0}", did), MessageLevel.Info);
                var sb = new StringBuilder();
                var sw = new StringWriter(sb);
                var xmlSerializer = new XmlSerializer(typeof(SystemInfo));
                parameters.SystemInfo.Databases = parameters.Storage.GetDatabases().Select(d => new DatabaseInfo
                {
                    Name = d.Name,
                    Classes =
                       d.Schema.Classes.Select(
                           c =>
                               new DatabaseClass
                               {
                                   Name = c.Value.Name,
                                   Interface = c.Value.Interface,
                                   Fields = d.Schema.ClassProperties(c.Value).Select(f => new Field { Name = f.Name, Type = f.Type, IsArray = f.IsArray }).ToList(),
                                   Methods = d.Schema.Methods.ContainsKey(c.Key) ? d.Schema.Methods[c.Key] : new List<string>()
                               }).ToList()
                }).ToList();
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
            catch (Exception ex)
            {
                var errorResult = new DTOQueryResult()
                {
                    NextResult = null,
                    QueryResults = null,
                    QueryResultType = ResultType.StringResult,
                    StringOutput = "Error during database creation: " + ex.ToString()
                };
                return new QueryDTO { Result = errorResult };
            }
        }
    }
}
