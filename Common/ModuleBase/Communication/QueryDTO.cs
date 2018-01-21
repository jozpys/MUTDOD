using MUTDOD.Common.Communication;
using MUTDOD.Common.ModuleBase.Storage.Core.Metadata;
using MUTDOD.Common.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUTDOD.Common.ModuleBase.Communication
{
    public class QueryDTO
    {
        public DTOQueryResult Result { get; set; }
        public Class QueryClass { get; set; }
        public IEnumerable<IStorable> QueryObjects { get; set; }
        public Object Value { get; set; }
        public Object AdditionalValue { get; set; }
    }
}
