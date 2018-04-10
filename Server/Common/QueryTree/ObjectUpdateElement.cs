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
            var literalElement = SingleElement();
            var literal = literalElement.Execute(parameters).Value;
            toStore.Properties[property] = literal;

            return new QueryDTO() { Value = property };
        }
    }
}
