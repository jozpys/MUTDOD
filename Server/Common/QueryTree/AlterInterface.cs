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
    public class AlterInterface : AbstractComposite
    {
        public AlterInterface() : base(ElementType.ALTER_INTERFACE) { }

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

            IQueryElement classNameElement = Element(ElementType.CLASS_NAME);
            QueryDTO classResult = classNameElement.Execute(parameters);
            if (classResult.Result != null || !classResult.QueryClass.Interface )
            {
                var noInterfaceResult = new DTOQueryResult()
                {
                    NextResult = null,
                    QueryResults = null,
                    QueryResultType = ResultType.StringResult,
                    StringOutput = "No interface with name: " + classResult.QueryClass.Name
                };
                return new QueryDTO() { Result = noInterfaceResult };
            }

            var classDef = classResult.QueryClass;
            var classId = classDef.ClassId;
            var interfaceName = classDef.Name;
            parameters.Subquery = new QueryDTO { QueryClass = classDef };

            foreach (var attr in AllElements(ElementType.ATTRIBUTE_DECLARATION))
            {
                var attributeResult = attr.Execute(parameters);
                if (attributeResult.Result != null)
                {
                    return attributeResult;
                }
            }

            foreach (var attr in AllElements(ElementType.DROP_ATTRIBUTE))
            {
                var dropAttributeResult = attr.Execute(parameters);
                if (dropAttributeResult.Result != null)
                {
                    return dropAttributeResult;
                }
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
            parameters.Storage.SaveSchema(parameters.Database.Schema);
            parameters.Log("Altered interface: " + interfaceName, MessageLevel.QueryExecution);
            var result = new DTOQueryResult()
            {
                QueryResultType = ResultType.StringResult,
                StringOutput = "Alter interface: " + interfaceName + " complited."
            };
            return new QueryDTO() { Result = result };
        }
    }
}
