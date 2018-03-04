using MUTDOD.Common;
using MUTDOD.Common.ModuleBase.Communication;
using MUTDOD.Server.Common.QueryTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MUTDOD.Server.Common.CoreModule.Communication
{
    [DataContract]
    [KnownType(typeof(SystemOperation))]
    [KnownType(typeof(SystemInformation))]
    public class DTOQueryTree
    {
        [DataMember]
        public IQueryElement QueryTree { get; set; }

        public override string ToString()
        {
            return "test";
        }

        public DTOQueryTree(IQueryElement queryTree)
        {
            QueryTree = queryTree;
        }
    }
}
