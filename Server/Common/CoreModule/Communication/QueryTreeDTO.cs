using MUTDOD.Common;
using MUTDOD.Common.ModuleBase.Communication;
using MUTDOD.Server.Common.QueryTree;
using MUTDOD.Server.Common.QueryTree.Literal;
using MUTDOD.Server.Common.QueryTree.Operator;
using MUTDOD.Server.Common.QueryTree.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MUTDOD.Server.Common.CoreModule.Communication
{
    [DataContract]
    [KnownType(typeof(HashSet<IQueryElement>))]
    [KnownType(typeof(SystemOperation))]
    [KnownType(typeof(SystemInformation))]
    [KnownType(typeof(CreateDatabase))]
    [KnownType(typeof(RenameDatabase))]
    [KnownType(typeof(DropDatabase))]
    [KnownType(typeof(ClassDeclaration))]
    [KnownType(typeof(InterfaceDeclaration))]
    [KnownType(typeof(ParentClasses))]
    [KnownType(typeof(AttributeDeclaration))]
    [KnownType(typeof(DropClass))]
    [KnownType(typeof(NewObject))]
    [KnownType(typeof(ObjectInitializationElement))]
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

    [KnownType(typeof(BoolType))]
    [KnownType(typeof(ByteType))]
    [KnownType(typeof(CharType))]
    [KnownType(typeof(DoubleType))]
    [KnownType(typeof(FloatType))]
    [KnownType(typeof(IntType))]
    [KnownType(typeof(LongType))]
    [KnownType(typeof(ShortType))]
    [KnownType(typeof(StringType))]
    [KnownType(typeof(Name))]
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
