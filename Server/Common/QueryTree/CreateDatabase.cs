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

namespace MUTDOD.Server.Common.QueryTree
{
    public class CreateDatabase : AbstractLeaf
    {
        public CreateDatabase() : base(ElementType.CREATE_DATABASE) { }
        public String DatabaseName { get; set; }

        public override QueryDTO Execute(QueryParameters parameters)
        {
            try
            {
                var did = parameters.Storage.CreateDatabase(new DatabaseParameters(DatabaseName, parameters.SettingsManager));
                parameters.Log(string.Format("new database created as {0}", did), MessageLevel.Info);
                var sb2 = new StringBuilder();
                var sw2 = new StringWriter(sb2);
                var xmlSerializer2 = new XmlSerializer(typeof(DatabaseInfo));
                var db = parameters.Storage.GetDatabase(did);
                xmlSerializer2.Serialize(sw2,
                    new DatabaseInfo()
                    {
                        Name = db.Name,
                        Classes = db.Schema.Classes.Select(c => new DatabaseClass
                        {
                            Name = c.Value.Name,
                            Fields =
                                db.Schema.Properties.Where(p => p.Value.ParentClassId == c.Value.ClassId.Id)
                                    .Select(p => p.Value.Name)
                                    .ToList(),
                            Methods =
                                db.Schema.Methods.ContainsKey(c.Key)
                                    ? db.Schema.Methods[c.Key]
                                    : new List<string>()
                        }).ToList()
                    });
                var result = new DTOQueryResult()
                {
                    NextResult = null,
                    QueryResults = null,
                    QueryResultType = ResultType.DatabaseInfo,
                    StringOutput = sb2.ToString()
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
