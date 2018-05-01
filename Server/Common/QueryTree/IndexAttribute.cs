using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using MUTDOD.Common.ModuleBase.Communication;
using MUTDOD.Common.Types;

namespace MUTDOD.Server.Common.QueryTree
{
    public class IndexAttribute : AbstractLeaf
    {
        [DataMember]
        public String Name { get; set; }

        public IndexAttribute() : base(ElementType.INDEX_ATTRIBUTE) { }

        public override QueryDTO Execute(QueryParameters parameters)
        {
            var classProperties = parameters.Database.Schema.ClassProperties(parameters.Subquery.QueryClass);
            var classProperty = classProperties.Where(p => p.Name == Name).Single();

            if (classProperty == null)
            {
                throw new NoClassPropertyException { PropertyName = Name };
            }

            return new QueryDTO() { Value = classProperty };
        }
    }
}
