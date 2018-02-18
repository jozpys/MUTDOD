﻿using MUTDOD.Common;
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
                createDatabase.DatabaseName = context.NAME().GetText();
                systemOperation.Add(createDatabase);
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

        /*

        public override IQueryTree VisitNew_object([NotNull] QueryGrammarParser.New_objectContext context)
        {
            QueryTree newObjectTree = new QueryTree();
            newObjectTree.TokenName = TokenName.NEW_OBJECT;
            newObjectTree.ProductionsList = new SubTrees();

            IQueryTree classNameTree = Visit(context.class_name());
            newObjectTree.ProductionsList.Add(classNameTree);

            IQueryTree attributesTree = Visit(context.object_initialization_attributes_list());
            newObjectTree.ProductionsList.Add(attributesTree);

            return newObjectTree;
        }

        public override IQueryTree VisitObject_initialization_attributes_list([NotNull] QueryGrammarParser.Object_initialization_attributes_listContext context)
        {
            QueryTree objectAtributesTree = new QueryTree();
            objectAtributesTree.TokenName = TokenName.OBJECT_INITIALIZATION_ATTRIBUTES_LIST;
            objectAtributesTree.ProductionsList = new SubTrees();

            foreach( var element in context.object_initialization_element())
            {
                IQueryTree elementTree = Visit(element);
                objectAtributesTree.ProductionsList.Add(elementTree);
            }

            return objectAtributesTree;
        }

        public override IQueryTree VisitObject_initialization_element([NotNull] QueryGrammarParser.Object_initialization_elementContext context)
        {
            QueryTree elementTree = new QueryTree();
            elementTree.TokenName = TokenName.OBJECT_INITIALIZATION_ELEMENT;
            elementTree.ProductionsList = new SubTrees();

            QueryTree attributeNameTree = new QueryTree();
            attributeNameTree.TokenName = TokenName.ATTRIBUTE_NAME;
            attributeNameTree.ProductionsList = new SubTrees();

            QueryTree nameTree = new QueryTree();
            nameTree.TokenName = TokenName.NAME;
            nameTree.TokenValue = context.NAME().GetText();
            attributeNameTree.ProductionsList.Add(nameTree);
            elementTree.ProductionsList.Add(attributeNameTree);

            IQueryTree attributeValueTree = Visit(GetElementValue(context));
            elementTree.ProductionsList.Add(attributeValueTree);

            return elementTree;
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

        public override IQueryTree VisitClass_delcaration([NotNull] QueryGrammarParser.Class_delcarationContext context)
        {
            QueryTree classDeclarationTree = new QueryTree();
            classDeclarationTree.TokenName = TokenName.CLASS_DECLARATION;
            classDeclarationTree.ProductionsList = new SubTrees();

            IQueryTree classNameTree = Visit(context.class_name());
            classDeclarationTree.ProductionsList.Add(classNameTree);

            foreach (var element in context.cls_attribute_dec_stm())
            {
                IQueryTree elementTree = Visit(element);
                classDeclarationTree.ProductionsList.Add(elementTree);
            }

            return classDeclarationTree;
        }

        public override IQueryTree VisitCls_attribute_dec_stm([NotNull] QueryGrammarParser.Cls_attribute_dec_stmContext context)
        {
            QueryTree attributeStmtTree = new QueryTree();
            attributeStmtTree.TokenName = TokenName.ATTRIBUTE_DEC_STM;
            attributeStmtTree.ProductionsList = new SubTrees();

            QueryTree attributeName = new QueryTree();
            attributeName.TokenName = TokenName.ATTRIBUTE_NAME;
            attributeName.ProductionsList = new SubTrees();
            attributeStmtTree.ProductionsList.Add(attributeName);

            QueryTree nameTree = new QueryTree();
            nameTree.TokenName = TokenName.NAME;
            nameTree.TokenValue = context.NAME().GetText();
            attributeName.ProductionsList.Add(nameTree);

            IQueryTree dateType = Visit(context.dataType());
            attributeStmtTree.ProductionsList.Add(dateType);

            return attributeStmtTree;
        }

        public override IQueryTree VisitDrop_stmt([NotNull] QueryGrammarParser.Drop_stmtContext context)
        {
            QueryTree dropTree = new QueryTree();
            dropTree.TokenName = TokenName.DROP;
            dropTree.ProductionsList = new SubTrees();

            IQueryTree classNameTree = Visit(context.class_name());
            dropTree.ProductionsList.Add(classNameTree);
            return dropTree;
        }
        */
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

        /*
        public override IQueryTree VisitDataType([NotNull] QueryGrammarParser.DataTypeContext context)
        {
            QueryTree dateTypeTree = new QueryTree();
            dateTypeTree.TokenName = TokenName.DATA_TYPE;
            dateTypeTree.ProductionsList = new SubTrees();

            QueryTree type = new QueryTree();
            if (context.BYTE_TYPE() != null)
            {
                type.TokenName = TokenName.BYTE_TYPE;
                type.TokenValue = "byte";
            }
            else if(context.SHORT_TYPE() != null)
            {
                type.TokenName = TokenName.SHORT_TYPE;
                type.TokenValue = "short";
            }
            else if (context.INT_TYPE() != null)
            {
                type.TokenName = TokenName.INT_TYPE;
                type.TokenValue = "int";
            }
            else if (context.LONG_TYPE() != null)
            {
                type.TokenName = TokenName.LONG_TYPE;
                type.TokenValue = "long";
            }
            else if (context.FLOAT_TYPE() != null)
            {
                type.TokenName = TokenName.FLOAT_TYPE;
                type.TokenValue = "float";
            }
            else if (context.DOUBLE_TYPE() != null)
            {
                type.TokenName = TokenName.DOUBLE_TYPE;
                type.TokenValue = "double";
            }
            else if (context.CHAR_TYPE() != null)
            {
                type.TokenName = TokenName.CHAR_TYPE;
                type.TokenValue = "char";
            }
            else if (context.STRING_TYPE() != null)
            {
                type.TokenName = TokenName.STRING_TYPE;
                type.TokenValue = "string";
            }
            else if (context.BOOL_TYPE() != null)
            {
                type.TokenName = TokenName.BOOL_TYPE;
                type.TokenValue = "bool";
            }
            else if (context.NAME() != null)
            {
                type.TokenName = TokenName.NAME;
                type.TokenValue = context.NAME().GetText();
            }
            dateTypeTree.ProductionsList.Add(type);

            return dateTypeTree;
        }
        */

        
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

            return null;
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

            return null;
        }
        
    }
}
