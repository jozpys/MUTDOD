﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using MUTDOD.Common;
using MUTDOD.Common.Communication;
using MUTDOD.Common.ModuleBase.Communication;
using MUTDOD.Common.ModuleBase.Storage.Core.Metadata;
using MUTDOD.Common.Types;

namespace MUTDOD.Server.Common.QueryTree
{
    [DataContract]
    public class ClassDeclaration : AbstractComposite
    {
        public ClassDeclaration() : base(ElementType.CLASS_DECLARATION) { }
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
                    StringOutput = "Error ocured while class creation"
                };
                return new QueryDTO() { Result = errorResult };
            }

            IQueryElement classNameElement = Element(ElementType.CLASS_NAME);
            QueryDTO classResult = classNameElement.Execute(parameters);
            String newClassName = classResult.QueryClass.Name;
            if (classResult.Result == null)
            {
                var classExistsResult = new DTOQueryResult()
                {
                    NextResult = null,
                    QueryResults = null,
                    QueryResultType = ResultType.StringResult,
                    StringOutput = "Class or interface with name: " + newClassName + " arleady exists!"
                };
                return new QueryDTO() { Result = classExistsResult };
            }

            var classId = new ClassId
            {
                Name = newClassName,
                Id = (parameters.Database.Schema.Classes.Max(d => (long?)d.Key.Id) ?? 0) + 1
            };
            var classDef = new Class
            {
                ClassId = classId,
                Name = newClassName
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

            foreach (IQueryElement attr in AllElements(ElementType.ATTRIBUTE_DECLARATION))
            {
                var attributeResult = attr.Execute(parameters);
                if(attributeResult.Result != null)
                {
                    return attributeResult;
                }
            }


            parameters.Database.Schema.Methods.TryAdd(classId, new List<IMethod>());

            foreach(IQueryElement meth in AllElements(ElementType.METHOD_DECLARATION))
            {
                meth.Execute(parameters);
            }

            /*
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
                parameters.Log("Could not define new class", MessageLevel.Error);
                var errorResult = new DTOQueryResult()
                {
                    QueryResultType = ResultType.StringResult,
                    StringOutput = "Error ocured while class creation"
                };
                return new QueryDTO { Result = errorResult };
            }
            parameters.Storage.SaveSchema(parameters.Database.Schema);
            parameters.Log("Defined new class: " + newClassName, MessageLevel.QueryExecution);
            var result = new DTOQueryResult()
            {
                QueryResultType = ResultType.StringResult,
                StringOutput = "New class: " + newClassName + " created."
            };
            return new QueryDTO() { Result = result };
        }
    }
}
