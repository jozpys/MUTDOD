﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using MUTDOD.Common;
using MUTDOD.Common.Communication;
using MUTDOD.Common.ModuleBase.Communication;
using MUTDOD.Common.Types;

namespace MUTDOD.Server.Common.QueryTree
{
    [DataContract]
    public class ClassProperty : AbstractComposite
    {
        public ClassProperty() : base (ElementType.CLASS_PROPERTY){}
        [DataMember]
        public String Name { get; set; }
        public Dictionary<int, string> indexesOnAttribute;
        public override QueryDTO Execute(QueryParameters parameters)
        {
            IStorable databaseObject = parameters.Subquery.QueryObjects.Single();
            var objectProperty = databaseObject.Properties.Where(p => p.Key.Name == Name);
            if(!objectProperty.Any())
            {
                var parentClass = parameters.Subquery.QueryClass;
                var classProperties = parameters.Database.Schema.ClassProperties(parentClass);
                var classProperty = classProperties.Where(p => p.Name == Name);

                if (!classProperty.Any())
                {
                    throw new NoClassPropertyException { PropertyName = Name };
                }

                return new QueryDTO { Value = null };
            }

            var propertyValue = objectProperty.Single().Value;
            if (!objectProperty.Single().Key.IsValueType)
            {
                var propertyObject = parameters.Storage.Get(parameters.Database.DatabaseId, (Guid)propertyValue);
                parameters.Subquery.QueryObjects = new List<IStorable> { propertyObject };

                if (TryGetElement(ElementType.CLASS_PROPERTY, out IQueryElement childProperty))
                {
                    return childProperty.Execute(parameters);
                }

                var result = parameters.Subquery;
                result.Result = new DTOQueryResult { QueryResultType = ResultType.ReferencesOnly };
                return result;
            }

            var propertyDto = new QueryDTO {
                Result = new DTOQueryResult
                {
                    QueryResultType = ResultType.StringResult,
                    StringOutput = "Property " + Name + ": " + propertyValue
                },
                Value = propertyValue
            };
            return propertyDto;
        }

        public void Optimize(QueryParameters queryParameters)
        {
            List<string> attributes = new List<string>();
            attributes.Add(Name);
            indexesOnAttribute = queryParameters.IndexMechanism.getIndexesForAttributes(queryParameters.Subquery.QueryClass.Name, attributes);
        }
    }
}
