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
using MUTDOD.Common.Types;

namespace MUTDOD.Server.Common.QueryTree
{
    [DataContract]
    public class ClassProperty : AbstractComposite
    {
        public ClassProperty() : base (ElementType.CLASS_PROPERTY){}
        [DataMember]
        public String Name { get; set; }
        public override QueryDTO Execute(QueryParameters parameters)
        {
            var properties = new List<KeyValuePair<Property, object>>();
            foreach (IStorable storable in parameters.Subquery.QueryObjects)
            {
                var objectProperty = storable.Properties.Where(p => p.Key.Name == Name);
                properties.AddRange(objectProperty);
                
            }

            var parentClass = parameters.Subquery.QueryClass;
            var classProperties = parameters.Database.Schema.ClassProperties(parentClass);
            var classProperty = classProperties.Where(p => p.Name == Name);

            if (!properties.Any())
            {

                if (!classProperty.Any())
                {
                    throw new NoClassPropertyException { PropertyName = Name };
                }

                return new QueryDTO { QueryObjects = Enumerable.Empty<IStorable>(), Value = null };
            }

            var property = properties.First();
            if (!property.Key.IsValueType)
            {
                parameters.Subquery.QueryObjects = GetPropertyObjects(parameters, properties);

                if (TryGetElement(ElementType.CLASS_PROPERTY, out IQueryElement childProperty))
                {
                    var classType = classProperty.Single();
                    parameters.Subquery.QueryClass = parameters.Database.Schema.Classes.Values.SingleOrDefault(c => c.Name == classType.Type);
                    return childProperty.Execute(parameters);
                }

                var result = parameters.Subquery;
                result.Result = new DTOQueryResult { QueryResultType = ResultType.ReferencesOnly };
                result.AdditionalValue = classProperty.Single();
                return result;
            }

            var propertyValues = GetPropertyValues(parameters, properties);
            if (propertyValues.Count() == 1)
            {
                var singleValue = propertyValues.Single();
                var singlePropertyDto = new QueryDTO
                {
                    Result = new DTOQueryResult
                    {
                        QueryResultType = ResultType.StringResult,
                        StringOutput = "Property " + Name + ": " + singleValue
                    },
                    Value = singleValue,
                    AdditionalValue = classProperty.Single()
                };
                return singlePropertyDto;
            }

            var propertyDto = new QueryDTO {
                Result = new DTOQueryResult
                {
                    QueryResultType = ResultType.StringResult,
                    StringOutput = "Property " + Name + ": " + String.Join(", ", propertyValues)
                },
                Value = propertyValues,
                AdditionalValue = classProperty.Single()
            };
            return propertyDto;
        }

        private IEnumerable<IStorable> GetPropertyObjects(QueryParameters parameters, IEnumerable<KeyValuePair<Property, object>> properties)
        {
            var aggreagatedProperties = new List<IStorable>();
            foreach(var prop in properties)
            {
                if (prop.Key.IsArray)
                {
                    List<IStorable> objects = new List<IStorable>();
                    List<Object> array = (List<Object>)prop.Value;
                    foreach (var storable in array)
                    {
                        var arrayObject = parameters.Storage.Get(parameters.Database.DatabaseId, (Guid)storable);
                        objects.Add(arrayObject);
                    }
                    aggreagatedProperties.AddRange(objects);
                }
                else
                {
                    var propertyObject = parameters.Storage.Get(parameters.Database.DatabaseId, (Guid)prop.Value);
                    aggreagatedProperties.Add(propertyObject);
                }
            }

            return aggreagatedProperties;
        }

        private IEnumerable<Object> GetPropertyValues(QueryParameters parameters, IEnumerable<KeyValuePair<Property, object>> properties)
        {
            var aggreagatedProperties = new List<Object>();
            foreach (var prop in properties)
            {
                if (prop.Key.IsArray)
                {
                    List<Object> array = (List<Object>)prop.Value;
                    aggreagatedProperties.AddRange(array);
                }
                else
                {
                    aggreagatedProperties.Add(prop.Value);
                }
            }

            return aggreagatedProperties;
        }
    }
}
