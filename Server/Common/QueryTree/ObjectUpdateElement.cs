using System;
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
    public class ObjectUpdateElement : AbstractComposite
    {
        public ObjectUpdateElement() : base(ElementType.OBJECT_UPDATE_ELEMENT) { }

        [DataMember]
        public String FieldName { get; set; }

        public override QueryDTO Execute(QueryParameters parameters)
        {
            IStorable toStore = parameters.Subquery.Value;
            List<Property> propeteries = parameters.Subquery.AdditionalValue;

            Property property = propeteries.SingleOrDefault(p => p.Name == FieldName);
            if (property == null)
                return new QueryDTO()
                {
                    Result = new DTOQueryResult
                    {
                        NextResult = null,
                        QueryResultType = ResultType.StringResult,
                        StringOutput = "Unknown field: " + FieldName
                    }
                };

            var valueElement = SingleElement();
            var valueDto = valueElement.Execute(parameters);
            String literalType = valueDto.AdditionalValue;

            if (literalType == null)
            {
                toStore.Properties.Remove(property);
                return new QueryDTO() { Value = null };
            }

            if (literalType != null && !literalType.Equals(property.Type))
            {
                return new QueryDTO()
                {
                    Result = new DTOQueryResult
                    {
                        NextResult = null,
                        QueryResultType = ResultType.StringResult,
                        StringOutput = "Can't use literal type " + literalType + " for property type " + property.Type
                    }
                };
            }

            if (valueElement.ElementType == ElementType.LITERAL)
            {
                var literal = valueDto.Value;
                toStore.Properties[property] = literal;
            }
            else if (valueElement.ElementType == ElementType.SELECT)
            {
                var objects = valueDto.QueryObjects;
                if (property.IsArray)
                {
                    List<Object> elements = new List<Object>();
                    foreach (var storable in objects)
                    {
                        elements.Add(storable.Oid.Id);
                    }
                    toStore.Properties[property] = elements;
                }
                else
                {
                    var objectId = objects.Single().Oid.Id;
                    toStore.Properties[property] = objectId;
                }
            }
            else if (valueElement.ElementType == ElementType.ARRAY)
            {
                var array = valueDto.Value;
                toStore.Properties[property] = array;
            }

            return new QueryDTO() { Value = property };
        }


    }
}
