using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using MUTDOD.Common;
using MUTDOD.Common.Communication;
using MUTDOD.Common.ModuleBase.Communication;
using MUTDOD.Common.ModuleBase.Storage.Core.Metadata;

namespace MUTDOD.Server.Common.QueryTree
{
    [DataContract]
    public class InterfaceDeclaration : AbstractComposite
    {
        public InterfaceDeclaration() : base(ElementType.INTERFACE_DECLARATION) { }

        public string Name { get; set; }
        public override QueryDTO Execute(QueryParameters parameters)
        {
            if (parameters.Database == null)
            {
                parameters.Log("Database is required!", MessageLevel.Error);
                var errorResult = new DTOQueryResult()
                {
                    NextResult = null,
                    QueryResults = null,
                    QueryResultType = ResultType.StringResult,
                    StringOutput = "Error ocured while interface creation"
                };
                return new QueryDTO() { Result = errorResult };
            }

            bool nameExists = parameters.Database.Schema.Classes.Values.Any(c => c.Name == Name);
            if (nameExists)
            {
                var classExistsResult = new DTOQueryResult()
                {
                    NextResult = null,
                    QueryResults = null,
                    QueryResultType = ResultType.StringResult,
                    StringOutput = "Class or interface with name: " + Name + " arleady exists!"
                };
                return new QueryDTO() { Result = classExistsResult };
            }

            var classId = new ClassId
            {
                Name = Name,
                Id = (parameters.Database.Schema.Classes.Max(d => (long?)d.Key.Id) ?? 0) + 1
            };
            var classDef = new Class
            {
                ClassId = classId,
                Name = Name,
                Interface = true
            };
            parameters.Subquery = new QueryDTO { QueryClass = classDef };

            if (TryGetElement(ElementType.PARENT_CLASSES, out IQueryElement parentClassesElement))
            {
                var parentClasses = parentClassesElement.Execute(parameters);
                if(parentClasses.Result != null)
                {
                    return parentClasses;
                }
                classDef.Parent = parentClasses.Value;
            }

            foreach (var attr in AllElements(ElementType.ATTRIBUTE_DECLARATION))
            {
                attr.Execute(parameters);
            }

            /*
            _database.Schema.Methods.TryAdd(classId, new List<string>());
            
            foreach (var meth in queryTree.ProductionsList.Where(t => t.TokenName == TokenName.METHOD_DEC_STM))
            {
                var methName = (isClass
                    ? meth.ProductionsList.Single(t => t.TokenName == TokenName.METHOD_NAME)
                    : meth)
                    .ProductionsList.Single(t => t.TokenName == TokenName.NAME).TokenValue;
                _database.Schema.Methods[classId].Add(methName);
            }
            foreach (var rel in queryTree.ProductionsList.Where(t => t.TokenName == TokenName.RELATION_DEC_STM))
            {
                var attrName = rel.ProductionsList.Single(t => t.TokenName == TokenName.NAME).TokenValue;
                string typeName =
                    rel.ProductionsList.Single(t => t.TokenName == TokenName.DATA_TYPE)
                        .ProductionsList.Single()
                        .TokenValue;
                var propertyId = new PropertyId
                {
                    Id = 1 + (_database.Schema.Properties.Max(p => (long?)p.Key.Id) ?? 0),
                    Name = attrName,
                    ParentClassId = classId.Id
                };
                _database.Schema.Properties.TryAdd(propertyId, new Property
                {
                    ParentClassId = classId.Id,
                    Name = attrName,
                    PropertyId = propertyId,
                    Type = typeName,
                    IsValueType = false
                });
            }
            */
            if (!parameters.Database.Schema.Classes.TryAdd(classId, classDef))
            {
                parameters.Log("Could not define new interface", MessageLevel.Error);
                var errorResult = new DTOQueryResult()
                {
                    QueryResultType = ResultType.StringResult,
                    StringOutput = "Error ocured while interface creation"
                };
                return new QueryDTO { Result = errorResult };
            }
            parameters.Storage.SaveSchema(parameters.Database.Schema);
            parameters.Log("Defined new interface: " + Name, MessageLevel.QueryExecution);
            var result = new DTOQueryResult()
            {
                QueryResultType = ResultType.StringResult,
                StringOutput = "New interface: " + Name + " created."
            };
            return new QueryDTO() { Result = result };
        }
    }
}
