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
    public class DropAttribute : AbstractComposite
    {
        public DropAttribute() : base(ElementType.DROP_ATTRIBUTE) { }

        public String Name { get; set; }

        public override QueryDTO Execute(QueryParameters parameters)
        {
            var classProperties = parameters.Database.Schema.ClassProperties(parameters.Subquery.QueryClass);

            var parametersWithName = classProperties.Where(prop => prop.Name == Name);
            if (!parametersWithName.Any())
            {
                var errorMessage = new DTOQueryResult()
                {
                    QueryResultType = ResultType.StringResult,
                    StringOutput = "No attribute with name: " + Name
                };
                return new QueryDTO() { Result = errorMessage };
            }

            var parameterToDrop = parametersWithName.SingleOrDefault(pId => pId.ParentClassId == parameters.Subquery.QueryClass.ClassId.Id);
            if (parameterToDrop == null)
            {
                var parentClassParameter = parametersWithName.Single();
                var parentClass = parameters.Database.Schema.Classes.Values.Single(cl => cl.ClassId.Id == parentClassParameter.ParentClassId);
                var errorMessage = new DTOQueryResult()
                {
                    QueryResultType = ResultType.StringResult,
                    StringOutput = "Attribute with name: " + Name + " belongs to parent class(interface): " + parentClass.Name
                };
                return new QueryDTO() { Result = errorMessage };

            }

            Boolean success = parameters.Database.Schema.Properties.TryRemove(parameterToDrop.PropertyId, out Property dropedProperty);

            if (!success)
            {
                var errorMessage = new DTOQueryResult()
                {
                    QueryResultType = ResultType.StringResult,
                    StringOutput = "Error while droping attribute: " + Name
                };
                return new QueryDTO() { Result = errorMessage };
            }

            return new QueryDTO() { Value = dropedProperty };
        }
    }
}
