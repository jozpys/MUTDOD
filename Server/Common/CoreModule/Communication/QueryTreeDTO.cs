using MUTDOD.Common;
using MUTDOD.Common.ModuleBase.Communication;
using MUTDOD.Server.Common.QueryTree;
using MUTDOD.Server.Common.QueryTree.Literal;
using MUTDOD.Server.Common.QueryTree.Operator;
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
    [KnownType(typeof(CreateDatabase))]
    [KnownType(typeof(RenameDatabase))]
    [KnownType(typeof(DropDatabase))]
    [KnownType(typeof(SelectStatement))]
    [KnownType(typeof(ClassName))]
    [KnownType(typeof(WhereStatement))]
    [KnownType(typeof(OperationIsNull))]
    [KnownType(typeof(OperationIsNotNull))]
    [KnownType(typeof(OperationComperision))]
    [KnownType(typeof(LeftOperand))]
    [KnownType(typeof(RightOperand))]

    [KnownType(typeof(OperatorGrater))]
    [KnownType(typeof(OperatorGraterEqual))]
    [KnownType(typeof(OperatorIsEqual))]
    [KnownType(typeof(OperatorLess))]
    [KnownType(typeof(OperatorLessEqual))]
    [KnownType(typeof(OperatorNotEqual))]

    [KnownType(typeof(ClassProperty))]
    [KnownType(typeof(BoolLiteral))]
    [KnownType(typeof(IntegerLiteral))]
    [KnownType(typeof(NullLiteral))]
    [KnownType(typeof(StringLiteral))]
    public class DTOQueryTree
    {
        [DataMember]
        public IQueryElement QueryTree { get; set; }

        public override string ToString()
        {
            return QueryTree.ToString();
        }

        public DTOQueryTree(IQueryElement queryTree)
        {
            QueryTree = queryTree;
        }
    }
}
