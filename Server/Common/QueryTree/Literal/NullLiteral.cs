﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using MUTDOD.Common.ModuleBase.Communication;
using MUTDOD.Common.Types;

namespace MUTDOD.Server.Common.QueryTree.Literal
{
    [DataContract]
    public class NullLiteral : AbstractLeaf
    {
        public NullLiteral() : base(ElementType.LITERAL){}
        public override QueryDTO Execute(QueryParameters parameters)
        {
            var nullDto = new QueryDTO { Value = null, AdditionalValue = null };
            return nullDto;
        }
    }
}
