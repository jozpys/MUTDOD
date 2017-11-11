using System.Collections.Generic;
using System.Linq;
using MUTDOD.Common.ModuleBase.Storage.Core.Metadata;
using MUTDOD.Server.Common.QueryAnalyzer.SyntaxAnalyzer;

namespace MUTDOD.Server.Common.QueryAnalyzer.MetamodelHelper
{
    internal class Database
    {
        public Database(IDatabaseSchema databaseSchema)
        {
            classes = databaseSchema.Classes.Select(c => new Class
            {
                fields =
                    databaseSchema.Properties.Where(p => p.Value.ParentClassId == c.Value.ClassId.Id)
                        .Select(f => new Field {name = f.Value.Name, protectionLevel = MUTDODQLProtectionLevel.Public})
                        .ToList(),
                isGeneric = false,
                methods =
                    databaseSchema.Methods.ContainsKey(c.Key)
                        ? databaseSchema.Methods[c.Key].Select(
                            m =>
                                new Method
                                {
                                    name = m,
                                    protectionLevel = MUTDODQLProtectionLevel.Public,
                                    parameters = new List<Param>()
                                }).ToList()
                        : new List<Method>(),
                protectionLevel = MUTDODQLProtectionLevel.Public,
                name = c.Value.Name
            }).ToList();
        }

        public string title;

        public string description;

        public List<Class> classes;
    }
}
