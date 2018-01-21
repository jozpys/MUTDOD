using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUTDOD.Common;
using MUTDOD.Common.Communication;
using MUTDOD.Common.ModuleBase.Communication;
using MUTDOD.Common.Types;

namespace MUTDOD.Server.Common.QueryTree
{
    public class ClassProperty : AbstractLeaf
    {
        public ClassProperty() : base (ElementType.CLASS_PROPERTY){}
        public String Name { get; set; }
        public override QueryDTO Execute(QueryParameters parameters)
        {
            IStorable databaseObject = parameters.Subquery.QueryObjects.Single();
            var objectProperty = databaseObject.Properties.Where(p => p.Key.Name == Name);
            if(!objectProperty.Any())
            {
                throw new NoClassPropertyException { PropertyName = Name};
            }
            var propertyDto = new QueryDTO { Value = objectProperty.Single().Value };
            return propertyDto;
        }
    }
}
