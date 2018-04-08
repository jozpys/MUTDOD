using MUTDOD.Common;
using MUTDOD.Common.Communication;
using MUTDOD.Common.ModuleBase.Communication;
using MUTDOD.Common.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUTDOD.Server.Common.QueryTree
{
    public class AttributeDeclaration : AbstractComposite
    {
        public AttributeDeclaration() : base(ElementType.ATTRIBUTE_DECLARATION) { }

        public String Name { get; set; }

        public override QueryDTO Execute(QueryParameters parameters)
        {
            var typeElement = SingleElement();
            var typeDao = typeElement.Execute(parameters);

            var propertyId = new PropertyId
            {
                Id = 1 + parameters.Database.Schema.Properties.Max(p => (long?)p.Key.Id) ?? 0,
                Name = Name,
                ParentClassId = parameters.Subquery.QueryClass.ClassId.Id
            };

            var property = new Property
            {
                ParentClassId = parameters.Subquery.QueryClass.ClassId.Id,
                Name = Name,
                PropertyId = propertyId,
                Type = typeDao.Value,
                IsValueType = typeDao.AdditionalValue
            };

            Boolean success = parameters.Database.Schema.Properties.TryAdd(propertyId, property);

            if (!success)
            {
                var errorMessage = new DTOQueryResult()
                {
                    QueryResultType = ResultType.StringResult,
                    StringOutput = "Error while creating new attribute: " + Name
                };
                return new QueryDTO() { Result = errorMessage };
            }

            return new QueryDTO() { Value = property };
        }
    }
}
