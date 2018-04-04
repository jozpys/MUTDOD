using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using MUTDOD.Common.ModuleBase.Communication;

namespace MUTDOD.Server.Common.QueryTree.Type
{
    [DataContract]
    public class Name : AbstractLeaf
    {
        public Name() : base(ElementType.DATA_TYPE) { }

        [DataMember]
        public String ClassName { get; set; }

        public override QueryDTO Execute(QueryParameters parameters)
        {
            var dataTypeDto = new QueryDTO { Value = ClassName, AdditionalValue = false };
            return dataTypeDto;
        }
    }
}
