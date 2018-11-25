﻿using System;
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
using MUTDOD.Server.Common.QueryTree.Aggregate;
using MUTDOD.Common.Types;

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
                IQueryElement interfaceDeclaration = Visit(context.class_delcaration());
                return interfaceDeclaration;
            }
            else if(context.interface_declaration() != null)
            {
                IQueryElement interfaceDeclaration = Visit(context.interface_declaration());
                return interfaceDeclaration;
            }
            else if(context.alter_class() != null)
            {
                IQueryElement alterClass = Visit(context.alter_class());
                return alterClass;
            }
            else if(context.alter_interface() != null)
            {
                IQueryElement alterInterface = Visit(context.alter_interface());
                return alterInterface;
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
            else if (context.update_object() != null)
            {
                IQueryElement updateObject = Visit(context.update_object());
                return updateObject;
            }
            else if(context.delete_object() != null)
            {
                IQueryElement deleteObject = Visit(context.delete_object());
                return deleteObject;
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
            if (context.K_DEREF() != null)
            {
                Deref deref = new Deref();

                IQueryElement parentSelect = Visit(context.get_stmt());
                deref.Add(parentSelect);
                return deref;
            }

            if(context.aggregate_function() != null)
            {
                IQueryCompositeElement aggregate = Visit(context.aggregate_function()).GetComposite();

                IQueryElement parentSelect = Visit(context.get_stmt());
                aggregate.Add(parentSelect);
                return aggregate;
            }

            if (context.get_stmt() != null)
            {
                IQueryCompositeElement parentSelect = Visit(context.get_stmt()).GetComposite();
                if (context.child_value() != null)
                {
                    IQueryElement property = Visit(context.child_value());
                    parentSelect.Add(property);
                }

                return parentSelect;
            }

            SelectStatement select = new SelectStatement();
            IQueryElement className = Visit(context.get_header().class_name());
            select.Add(className);

            if (context.where_clause() != null)
            {
                IQueryElement where = Visit(context.where_clause());
                select.Add(where);
            }

            return select;
        }

        public override IQueryElement VisitAggregate_function([NotNull] QueryGrammarParser.Aggregate_functionContext context)
        {
            if (context.COUNT() != null)
            {
                Count count = new Count();
                return count;
            }
            else if(context.MAX() != null)
            {
                Maximum max = new Maximum();
                return max;
            }
            else if(context.MIN() != null)
            {
                Minimum min = new Minimum();
                return min;
            }
            else if(context.SUM() != null)
            {
                Sum sum = new Sum();
                return sum;
            }
            else if(context.AVERAGE() != null)
            {
                Average average = new Average();
                return average;
            }
            

            throw new SyntaxException("Unsuported aggregate function.");
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
            else if(context.object_initialization_array() != null)
            {
                return context.object_initialization_array();
            }

            throw new SyntaxException("Attribute literal or statement");
        }

        public override IQueryElement VisitObject_initialization_array([NotNull] QueryGrammarParser.Object_initialization_arrayContext context)
        {
            ArrayValues arrayValues = new ArrayValues();
            if(context.literal_list() != null)
            {
                foreach( var literal in context.literal_list().literal())
                {
                    IQueryElement literalElement = Visit(literal);
                    arrayValues.Add(literalElement);
                }
            }

            return arrayValues;
        }

        public override IQueryElement VisitUpdate_object([NotNull] QueryGrammarParser.Update_objectContext context)
        {
            UpdateObject updateObject = new UpdateObject();

            IQueryElement selectObjects = Visit(context.get_stmt());
            updateObject.Add(selectObjects);

            foreach (var element in context.object_update_attributes_list().object_update_element())
            {
                IQueryElement objectElement = Visit(element);
                updateObject.Add(objectElement);
            }

            return updateObject;
        }

        public override IQueryElement VisitObject_update_element([NotNull] QueryGrammarParser.Object_update_elementContext context)
        {
            ObjectUpdateElement objectElement = new ObjectUpdateElement();
            objectElement.FieldName = context.NAME().GetText();

            IQueryElement attributeValue = Visit(GetElementValue(context));
            objectElement.Add(attributeValue);

            return objectElement;
        }

        public override IQueryElement VisitDelete_object([NotNull] QueryGrammarParser.Delete_objectContext context)
        {
            DeleteObject deleteObject = new DeleteObject();

            IQueryElement selectObjects = Visit(context.get_stmt());
            deleteObject.Add(selectObjects);

            return deleteObject;
        }

        private IParseTree GetElementValue([NotNull] QueryGrammarParser.Object_update_elementContext context)
        {
            if (context.literal() != null)
            {
                return context.literal();
            }
            else if (context.get_stmt() != null)
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

            foreach (var method in context.cls_method_dec_stm())
            {
                IQueryElement methodDeclaration = Visit(method);
                classDeclaration.Add(methodDeclaration);
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
            if(context.K_ARRAY() != null)
            {
                attribute.IsArray = true;
            }

            IQueryElement dateType = Visit(context.dataType());
            attribute.Add(dateType);

            return attribute;
        }

        public override IQueryElement VisitCls_method_dec_stm([NotNull] QueryGrammarParser.Cls_method_dec_stmContext context)
        {
            MethodDeclaration method = new MethodDeclaration();
            method.Name = context.NAME().GetText();

            IQueryElement dateType = Visit(context.dataType());
            method.Add(dateType);

            return method;
        }

        public override IQueryElement VisitInterface_declaration([NotNull] QueryGrammarParser.Interface_declarationContext context)
        {
            InterfaceDeclaration interfaceDeclaration = new InterfaceDeclaration();
            interfaceDeclaration.Name = context.NAME().GetText();

            if (context.parent_type() != null)
            {
                ParentClasses parentClasses = (ParentClasses) Visit(context.parent_type());
                parentClasses.InterfaceOnly = true;
                interfaceDeclaration.Add(parentClasses);
            }

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
            if(context.K_ARRAY() != null)
            {
                attribute.IsArray = true;
            }

            IQueryElement dateType = Visit(context.dataType());
            attribute.Add(dateType);

            return attribute;
        }

        public override IQueryElement VisitAlter_class([NotNull] QueryGrammarParser.Alter_classContext context)
        {
            AlterClass alterClass = new AlterClass();

            IQueryElement className = Visit(context.class_name());
            alterClass.Add(className);

            foreach (var addAttribute in context.add_cls_attribute_dec_stm())
            {
                IQueryElement attributeDeclaration = Visit(addAttribute.cls_attribute_dec_stm());
                alterClass.Add(attributeDeclaration);
            }

            foreach (var dropAttribute in context.drop_cls_attribute_dec_stm())
            {
                IQueryElement attributeDeclaration = Visit(dropAttribute);
                alterClass.Add(attributeDeclaration);
            }

            foreach(var addMethod in context.add_cls_method_dec_stm())
            {
                IQueryElement methodDeclaretion = Visit(addMethod.cls_method_dec_stm());
                alterClass.Add(methodDeclaretion);
            }

            foreach (var dropMethod in context.drop_cls_method_dec_stm())
            {
                IQueryElement methodDeclaretion = Visit(dropMethod);
                alterClass.Add(methodDeclaretion);
            }

            return alterClass;
        }

        public override IQueryElement VisitAlter_interface([NotNull] QueryGrammarParser.Alter_interfaceContext context)
        {
            AlterInterface alterInterface = new AlterInterface();

            IQueryElement className = Visit(context.class_name());
            alterInterface.Add(className);

            foreach (var addAttribute in context.add_attribute_dec_stm())
            {
                IQueryElement attributeDeclaration = Visit(addAttribute.attribute_dec_stm());
                alterInterface.Add(attributeDeclaration);
            }

            foreach (var dropAttribute in context.drop_attribute_dec_stm())
            {
                IQueryElement attributeDeclaration = Visit(dropAttribute);
                alterInterface.Add(attributeDeclaration);
            }

            return alterInterface;
        }

        public override IQueryElement VisitDrop_attribute_dec_stm([NotNull] QueryGrammarParser.Drop_attribute_dec_stmContext context)
        {
            DropAttribute dropAttribute = new DropAttribute();
            dropAttribute.Name = context.NAME().GetText();

            return dropAttribute;
        }

        public override IQueryElement VisitDrop_cls_attribute_dec_stm([NotNull] QueryGrammarParser.Drop_cls_attribute_dec_stmContext context)
        {
            DropAttribute dropAttribute = new DropAttribute();
            dropAttribute.Name = context.NAME().GetText();

            return dropAttribute;
        }

        public override IQueryElement VisitDrop_cls_method_dec_stm([NotNull] QueryGrammarParser.Drop_cls_method_dec_stmContext context)
        {
            DropMethod dropMethod = new DropMethod();
            dropMethod.Name = context.NAME().GetText();

            return dropMethod;
        }

        public override IQueryElement VisitMethod_param([NotNull] QueryGrammarParser.Method_paramContext context)
        {
            MethodParam param = new MethodParam();
            param.Name = context.NAME().GetText();

            IQueryElement dateType = Visit(context.dataType());
            param.Add(dateType);
            
            if(context.K_OUT() != null)
            {
                param.Direction = ParameterDirection.OUT;
            }
            else
            {
                param.Direction = ParameterDirection.IN;
            }

            return param;
        }

        public override IQueryElement VisitDrop_stmt([NotNull] QueryGrammarParser.Drop_stmtContext context)
        {

            if (context.K_CLASS() != null)
            {
                DropClass dropClass = new DropClass();

                IQueryElement className = Visit(context.class_name());
                dropClass.Add(className);
                return dropClass;
            }
            else if(context.K_INTERFACE() != null)
            {
                DropInterface dropInterface = new DropInterface();

                IQueryElement className = Visit(context.class_name());
                dropInterface.Add(className);
                return dropInterface;
            }

            throw new SyntaxException("Unsupported object type.");
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
            IQueryElement operation = Visit(context.where_operation());
            if(context.and_or_clause() != null)
            {
                IQueryElement clasule = Visit(context.and_or_clause().clause());

                IQueryCompositeElement operatorElement = GetLogicalElement(context.and_or_clause()).GetComposite();
                operatorElement.Add(operation);
                operatorElement.Add(clasule);

                return operatorElement;
            }

            return operation;
        }

        private IQueryElement GetLogicalElement(QueryGrammarParser.And_or_clauseContext operatorContext)
        {
            if(operatorContext.AND() != null)
            {
                return new LogicalOperatorAnd();
            }
            else if(operatorContext.OR() != null)
            {
                return new LogicalOperatorOr();
            }

            return null;
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
                property.Name = context.NAME().GetText();

                if(context.child_value() != null)
                {
                    IQueryElement propertyChild = Visit(context.child_value());
                    property.Add(propertyChild);
                }
                return property;
            }

            return null;
        }

        public override IQueryElement VisitChild_value([NotNull] QueryGrammarParser.Child_valueContext context)
        {
            var firstPropertyName = context.NAME().First();
            ClassProperty property = new ClassProperty();
            property.Name = firstPropertyName.GetText();

            ClassProperty parentProperty = property;
            foreach (var propertyName in context.NAME().Skip(1))
            {
                ClassProperty childProperty = new ClassProperty();
                childProperty.Name = propertyName.GetText();

                parentProperty.Add(childProperty);
                parentProperty = childProperty;
            }
            return property;
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
            if (context.number() != null)
            {
                if(context.number().FLOAT_PRESICION() != null)
                {
                    FloatLiteral floatLiteral = new FloatLiteral();
                    floatLiteral.Value = context.number().GetText();
                    return floatLiteral;
                }
                IntegerLiteral literal = new IntegerLiteral();
                literal.Value = context.number().GetText();
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
            else if(context.K_NULL() != null)
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
