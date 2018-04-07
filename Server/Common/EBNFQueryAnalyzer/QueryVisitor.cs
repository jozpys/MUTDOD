using MUTDOD.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using MUTDOD.Common.ModuleBase;
using MUTDOD.Common.ModuleBase.Communication;
using MUTDOD.Server.Common.QueryTree;
using MUTDOD.Server.Common.QueryTree.Literal;
using MUTDOD.Server.Common.QueryTree.Operator;
using MUTDOD.Server.Common.QueryTree.Type;

namespace MUTDOD.Server.Common.EBNFQueryAnalyzer
{
    class QueryVisitor : QueryGrammarBaseVisitor<IQueryElement>
    {
        public override IQueryElement VisitStart([NotNull] QueryGrammarParser.StartContext context)
        {
            return Visit(context.statement());
        }

        public override IQueryElement VisitStatement([NotNull] QueryGrammarParser.StatementContext context)
        {

            if (context.system_operation() != null)
            {
                IQueryElement system_operation = Visit(context.system_operation());
                return system_operation;
            }
            else if (context.get_stmt() != null)
            {
                IQueryElement select = Visit(context.get_stmt());
                return select;
            }
            else if(context.class_delcaration() != null)
            {
                IQueryElement classDeclaration = Visit(context.class_delcaration());
                return classDeclaration;
            }
            else if(context.interface_declaration() != null)
            {
                IQueryElement interfaceDeclaration = Visit(context.interface_declaration());
                return interfaceDeclaration;
            }
            else if(context.drop_stmt() != null)
            {
                IQueryElement dropStatement = Visit(context.drop_stmt());
                return dropStatement;
            }
            else if(context.new_object() != null)
            {
                IQueryElement newObject = Visit(context.new_object());
                return newObject;
            }


            return null;
        }

        public override IQueryElement VisitSystem_operation([NotNull] QueryGrammarParser.System_operationContext context)
        {
            SystemOperation systemOperation = new SystemOperation();
            if (context.op.Type == QueryGrammarParser.SYS_INFO)
            {
                SystemInformation systemInfo = new SystemInformation();
                systemOperation.Add(systemInfo);
            }
            else if (context.op.Type == QueryGrammarParser.CREATE_DB)
            {
                CreateDatabase createDatabase = new CreateDatabase();
                createDatabase.DatabaseName = context.db_name.Text;
                systemOperation.Add(createDatabase);
            }
            else if (context.op.Type == QueryGrammarParser.RENAME_DB)
            {
                RenameDatabase renameDatabase = new RenameDatabase();
                renameDatabase.DatabaseName = context.db_name.Text;
                renameDatabase.NewDatabaseName = context.db_new_name.Text;
                systemOperation.Add(renameDatabase);
            }
            else if (context.op.Type == QueryGrammarParser.DROP_DB)
            {
                DropDatabase renameDatabase = new DropDatabase();
                renameDatabase.DatabaseName = context.db_name.Text;
                systemOperation.Add(renameDatabase);
            }

            return systemOperation;
        }

        public override IQueryElement VisitGet_stmt([NotNull] QueryGrammarParser.Get_stmtContext context)
        {
            SelectStatement select = new SelectStatement();
            IQueryElement className = Visit(context.get_header().class_name());
            select.Add(className);

            if (context.K_DEREF() != null)
            {
                select.Deref = true;
            }

            if (context.where_clause() != null)
            {
                IQueryElement where = Visit(context.where_clause());
                select.Add(where);
            }

            return select;
        }

        public override IQueryElement VisitClass_name([NotNull] QueryGrammarParser.Class_nameContext context)
        {
            ClassName className = new ClassName();
            className.Name = context.NAME().GetText();

            return className;
        }

        public override IQueryElement VisitNew_object([NotNull] QueryGrammarParser.New_objectContext context)
        {
            NewObject newObject = new NewObject();

            IQueryElement className = Visit(context.class_name());
            newObject.Add(className);

            foreach ( var element in context.object_initialization_attributes_list().object_initialization_element())
            {
                IQueryElement objectElement = Visit(element);
                newObject.Add(objectElement);
            }

            return newObject;
        }

        public override IQueryElement VisitObject_initialization_element([NotNull] QueryGrammarParser.Object_initialization_elementContext context)
        {
            ObjectInitializationElement objectElement = new ObjectInitializationElement();
            objectElement.FieldName = context.NAME().GetText();

            IQueryElement attributeValue = Visit(GetElementValue(context));
            objectElement.Add(attributeValue);

            return objectElement;
        }

        private IParseTree GetElementValue([NotNull] QueryGrammarParser.Object_initialization_elementContext context)
        {
            if (context.literal() != null)
            {
                return context.literal();
            }
            else if(context.get_stmt() != null)
            {
                return context.get_stmt();
            }

            throw new SyntaxException("Attribute literal or statement");
        }

        public override IQueryElement VisitClass_delcaration([NotNull] QueryGrammarParser.Class_delcarationContext context)
        {
            ClassDeclaration classDeclaration = new ClassDeclaration();

            IQueryElement className = Visit(context.class_name());
            classDeclaration.Add(className);

            if (context.parent_type() != null)
            {
                IQueryElement parentClasses = Visit(context.parent_type());
                classDeclaration.Add(parentClasses);
            }

            foreach (var attribute in context.cls_attribute_dec_stm())
            {
                IQueryElement attributeDeclaration = Visit(attribute);
                classDeclaration.Add(attributeDeclaration);
            }

            return classDeclaration;
        }

        public override IQueryElement VisitParent_type([NotNull] QueryGrammarParser.Parent_typeContext context)
        {
            ParentClasses parentClasses = new ParentClasses();

            foreach( var className in context.NAME())
            {
                ClassName parentClassName = new ClassName();
                parentClassName.Name = className.GetText();
                parentClasses.Add(parentClassName);
            }

            return parentClasses;
        }

        public override IQueryElement VisitCls_attribute_dec_stm([NotNull] QueryGrammarParser.Cls_attribute_dec_stmContext context)
        {
            AttributeDeclaration attribute = new AttributeDeclaration();
            attribute.Name = context.NAME().GetText();

            IQueryElement dateType = Visit(context.dataType());
            attribute.Add(dateType);

            return attribute;
        }

        public override IQueryElement VisitInterface_declaration([NotNull] QueryGrammarParser.Interface_declarationContext context)
        {
            InterfaceDeclaration interfaceDeclaration = new InterfaceDeclaration();
            interfaceDeclaration.Name = context.NAME().GetText();

            foreach (var attribute in context.attribute_dec_stm())
            {
                IQueryElement attributeDeclaration = Visit(attribute);
                interfaceDeclaration.Add(attributeDeclaration);
            }

            return interfaceDeclaration;
        }

        public override IQueryElement VisitAttribute_dec_stm([NotNull] QueryGrammarParser.Attribute_dec_stmContext context)
        {
            AttributeDeclaration attribute = new AttributeDeclaration();
            attribute.Name = context.NAME().GetText();

            IQueryElement dateType = Visit(context.dataType());
            attribute.Add(dateType);

            return attribute;
        }

        public override IQueryElement VisitDrop_stmt([NotNull] QueryGrammarParser.Drop_stmtContext context)
        {
            DropClass dropClass = new DropClass();

            IQueryElement className = Visit(context.class_name());
            dropClass.Add(className);
            return dropClass;
        }

        public override IQueryElement VisitWhere_clause([NotNull] QueryGrammarParser.Where_clauseContext context)
        {
            WhereStatement where = new WhereStatement();

            IQueryElement clause = Visit(context.clause());
            where.Add(clause);

            return where;
        }

        public override IQueryElement VisitClause([NotNull] QueryGrammarParser.ClauseContext context)
        {
            return Visit(context.where_operation());
        }

        public override IQueryElement VisitWhere_operation([NotNull] QueryGrammarParser.Where_operationContext context)
        {
            IQueryElement leftValueTree = Visit(context.left);

            if (context.where_operator().is_null() != null)
            {
                OperationIsNull isNullTree = new OperationIsNull();
                isNullTree.Add(leftValueTree);
                return isNullTree;
            }
            else if(context.where_operator().is_not_null() != null)
            {
                OperationIsNotNull isNotNullTree = new OperationIsNotNull();
                isNotNullTree.Add(leftValueTree);
                return isNotNullTree;
            }

            OperationComperision comperasionTree = new OperationComperision();

            LeftOperand left = new LeftOperand();
            left.Add(leftValueTree);
            comperasionTree.Add(left);

            IQueryElement operatorTree = Visit(context.where_operator().comparison_operator());
            comperasionTree.Add(operatorTree);

            RightOperand right = new RightOperand();
            IQueryElement rightValueTree = Visit(context.right);
            right.Add(rightValueTree);
            comperasionTree.Add(right);

            return comperasionTree;
        }

        public override IQueryElement VisitWhere_value([NotNull] QueryGrammarParser.Where_valueContext context)
        {
            if(context.literal() != null)
            {
                IQueryElement literal = Visit(context.literal());
                return literal;
            }
            else if(context.NAME() != null)
            {
                ClassProperty property = new ClassProperty();
                property.Name = context.GetText();
                return property;
            }

            return null;
        }

        public override IQueryElement VisitDataType([NotNull] QueryGrammarParser.DataTypeContext context)
        {
            if (context.BYTE_TYPE() != null)
            {
                return new ByteType();
            }
            else if(context.SHORT_TYPE() != null)
            {
                return new ShortType();
            }
            else if (context.INT_TYPE() != null)
            {
                return new IntType();
            }
            else if (context.LONG_TYPE() != null)
            {
                return new LongType();
            }
            else if (context.FLOAT_TYPE() != null)
            {
                return new FloatType();
            }
            else if (context.DOUBLE_TYPE() != null)
            {
                return new DoubleType();
            }
            else if (context.CHAR_TYPE() != null)
            {
                return new CharType();
            }
            else if (context.STRING_TYPE() != null)
            {
                return new StringType();
            }
            else if (context.BOOL_TYPE() != null)
            {
                return new BoolType();
            }
            else if (context.NAME() != null)
            {
                return new Name() { ClassName = context.NAME().GetText() };
            }

            throw new SyntaxException("Unsupported data type.");
        }

        
        public override IQueryElement VisitLiteral([NotNull] QueryGrammarParser.LiteralContext context)
        {
            if (context.NUMBER() != null)
            {
                IntegerLiteral literal = new IntegerLiteral();
                literal.Value = context.NUMBER().GetText();
                return literal;
            }
            else if(context.STRING_VALUE() != null)
            {
                StringLiteral literal = new StringLiteral();
                literal.Value = context.STRING_VALUE().GetText().Replace("'", "\"");
                return literal;
            }
            else if(context.BOOL_VALUE() != null)
            {
                BoolLiteral literal = new BoolLiteral();
                literal.Value = context.BOOL_VALUE().GetText();
                return literal;
            }
            else if(context.NULL_VALUE() != null)
            {
                NullLiteral literal = new NullLiteral();
                return literal;
            }

            throw new SyntaxException("Unsupported literal.");
        }

        public override IQueryElement VisitComparison_operator([NotNull] QueryGrammarParser.Comparison_operatorContext context)
        {
            if (context.GREATER() != null)
            {
                return new OperatorGrater();
            }
            else if(context.LESS() != null)
            {
                return new OperatorLess();
            }
            else if (context.GREATER_EQUAL() != null)
            {
                return new OperatorGraterEqual();
            }
            else if (context.LESS_EQUAL() != null)
            {
                return new OperatorLessEqual();
            }
            else if (context.ISEQUAL() != null)
            {
                return new OperatorIsEqual();
            }
            else if (context.NOT_EQUAL() != null)
            {
                return new OperatorNotEqual();
            }

            throw new SyntaxException("Unsupported operator.");
        }
        
    }
}
