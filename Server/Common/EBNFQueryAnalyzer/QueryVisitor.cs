using MUTDOD.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using MUTDOD.Common.ModuleBase;

namespace MUTDOD.Server.Common.EBNFQueryAnalyzer
{
    class QueryVisitor : QueryGrammarBaseVisitor<IQueryTree>
    {
        public override IQueryTree VisitStart([NotNull] QueryGrammarParser.StartContext context)
        {
            return Visit(context.statement());
        }
        public override IQueryTree VisitStatement([NotNull] QueryGrammarParser.StatementContext context)
        {
            QueryTree statementTree = new QueryTree();
            statementTree.TokenName = TokenName.STATEMENT;

            statementTree.ProductionsList = new SubTrees();
            if (context.system_operation() != null)
            {
                IQueryTree system_operation = Visit(context.system_operation());
                statementTree.ProductionsList.Add(system_operation);
                statementTree.ProductionsList.Add(createSemicolonTree());
                return statementTree;
            }
            else if (context.get_stmt() != null)
            {
                IQueryTree getStmt = Visit(context.get_stmt());
                statementTree.ProductionsList.Add(getStmt);
                statementTree.ProductionsList.Add(createSemicolonTree());
                return statementTree;
            }
            else if(context.new_object() != null)
            {
                IQueryTree newObject = Visit(context.new_object());
                statementTree.ProductionsList.Add(newObject);
                statementTree.ProductionsList.Add(createSemicolonTree());
                return statementTree;
            }
            else if(context.class_delcaration() != null)
            {
                IQueryTree classDeclaration = Visit(context.class_delcaration());
                statementTree.ProductionsList.Add(classDeclaration);
                statementTree.ProductionsList.Add(createSemicolonTree());
                return statementTree;
            }
            else if(context.drop_stmt() != null)
            {
                IQueryTree dropStmt = Visit(context.drop_stmt());
                statementTree.ProductionsList.Add(dropStmt);
                statementTree.ProductionsList.Add(createSemicolonTree());
                return statementTree;
            }

            return statementTree;
        }

        public override IQueryTree VisitSystem_operation([NotNull] QueryGrammarParser.System_operationContext context)
        {
            QueryTree systemOpertionTree = new QueryTree();
            systemOpertionTree.TokenName = TokenName.SYSTEM_OPERATION;

            systemOpertionTree.ProductionsList = new SubTrees();
            if(context.op.Type == QueryGrammarParser.SYS_INFO)
            {
                QueryTree systemInfoTree = new QueryTree();
                systemInfoTree.TokenName = TokenName.GET_SYSTEM_INFO;
                systemOpertionTree.ProductionsList.Add(systemInfoTree);
            }
            else if (context.op.Type == QueryGrammarParser.CREATE_DB)
            {
                QueryTree createDatabaseTree = new QueryTree();
                createDatabaseTree.TokenName = TokenName.CREATE_DATABASE;
                createDatabaseTree.ProductionsList = new SubTrees();

                QueryTree nameTree = new QueryTree();
                nameTree.TokenName = TokenName.NAME;
                nameTree.TokenValue = context.NAME().GetText();
                createDatabaseTree.ProductionsList.Add(nameTree);

                systemOpertionTree.ProductionsList.Add(createDatabaseTree);
            }

            return systemOpertionTree;
        }

        public override IQueryTree VisitGet_stmt([NotNull] QueryGrammarParser.Get_stmtContext context)
        {
            QueryTree getTree = new QueryTree();
            getTree.TokenName = TokenName.GET;
            getTree.ProductionsList = new SubTrees();

            QueryTree getStmtTree= new QueryTree();
            getStmtTree.TokenName = TokenName.GET_STM;
            getStmtTree.ProductionsList = new SubTrees();
            IQueryTree getHeaderTree = Visit(context.get_header());
            getStmtTree.ProductionsList.Add(getHeaderTree);

            if (context.K_DEREF() != null)
            {
                QueryTree derefTree = new QueryTree();
                derefTree.TokenName = TokenName.K_DEREF;
                getStmtTree.ProductionsList.Add(derefTree);
            }

            if (context.where_clause() != null)
            {
                IQueryTree whereTree = Visit(context.where_clause());
                getStmtTree.ProductionsList.Add(whereTree);
            }

            getTree.ProductionsList.Add(getStmtTree);

            return getTree;
        }

        public override IQueryTree VisitGet_header([NotNull] QueryGrammarParser.Get_headerContext context)
        {
            QueryTree getHeaderTree = new QueryTree();
            getHeaderTree.TokenName = TokenName.GET_HEADER;
            getHeaderTree.ProductionsList = new SubTrees();

            IQueryTree classNameTree = Visit(context.class_name());
            getHeaderTree.ProductionsList.Add(classNameTree);

            return getHeaderTree;
        }

        public override IQueryTree VisitClass_name([NotNull] QueryGrammarParser.Class_nameContext context)
        {
            QueryTree classNameTree = new QueryTree();
            classNameTree.TokenName = TokenName.CLASS_NAME;
            classNameTree.ProductionsList = new SubTrees();

            QueryTree nameTree = new QueryTree();
            nameTree.TokenName = TokenName.NAME;
            nameTree.TokenValue = context.NAME().GetText();
            classNameTree.ProductionsList.Add(nameTree);

            return classNameTree;
        }

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

        public override IQueryTree VisitWhere_clause([NotNull] QueryGrammarParser.Where_clauseContext context)
        {
            QueryTree whereClauseTree = new QueryTree();
            whereClauseTree.TokenName = TokenName.WHERE_CLAUSE;
            whereClauseTree.ProductionsList = new SubTrees();

            QueryTree whereTree = new QueryTree();
            whereTree.TokenName = TokenName.K_WHERE;
            whereTree.ProductionsList = new SubTrees();
            whereClauseTree.ProductionsList.Add(whereTree);

            IQueryTree clauseTree = Visit(context.clause());
            whereClauseTree.ProductionsList.Add(clauseTree);

            return whereClauseTree;
        }

        public override IQueryTree VisitClause([NotNull] QueryGrammarParser.ClauseContext context)
        {
            QueryTree clauseTree = new QueryTree();
            clauseTree.TokenName = TokenName.CLAUSE;
            clauseTree.ProductionsList = new SubTrees();
            IQueryTree whereOperationTree = Visit(context.where_operation());
            clauseTree.ProductionsList.Add(whereOperationTree);

            return clauseTree;
        }

        public override IQueryTree VisitWhere_operation([NotNull] QueryGrammarParser.Where_operationContext context)
        {
            QueryTree clauseTree = new QueryTree();
            clauseTree.TokenName = TokenName.WHERE_OPERATION;
            clauseTree.ProductionsList = new SubTrees();

            IQueryTree leftValueTree = Visit(context.left);
            clauseTree.ProductionsList.Add(leftValueTree);

            QueryTree whereTailTree = new QueryTree();
            whereTailTree.TokenName = TokenName.WHERE_TAIL;
            whereTailTree.ProductionsList = new SubTrees();
            clauseTree.ProductionsList.Add(whereTailTree);

            QueryTree whereOperator = new QueryTree();
            whereOperator.TokenName = TokenName.WHERE_OPERATOR;
            whereOperator.ProductionsList = new SubTrees();
            whereTailTree.ProductionsList.Add(whereOperator);


            if (context.where_operator().is_null() != null)
            {
                QueryTree isNullTree = new QueryTree();
                isNullTree.TokenName = TokenName.IS_NULL;
                isNullTree.ProductionsList = new SubTrees();
                whereOperator.ProductionsList.Add(isNullTree);
                return clauseTree;
            }
            else if(context.where_operator().is_not_null() != null)
            {
                QueryTree isNotNullTree = new QueryTree();
                isNotNullTree.TokenName = TokenName.IS_NOT_NULL;
                isNotNullTree.ProductionsList = new SubTrees();
                whereOperator.ProductionsList.Add(isNotNullTree);
                return clauseTree;
            }
            

            QueryTree comperisionTree = new QueryTree();
            comperisionTree.TokenName = TokenName.COMPARISON_OPERATOR;
            comperisionTree.ProductionsList = new SubTrees();
            whereOperator.ProductionsList.Add(comperisionTree);

            IQueryTree operatorTree = Visit(context.where_operator().comparison_operator());
            comperisionTree.ProductionsList.Add(operatorTree);

            IQueryTree rightValueTree = Visit(context.right);
            whereTailTree.ProductionsList.Add(rightValueTree);

            return clauseTree;
        }

        public override IQueryTree VisitWhere_value([NotNull] QueryGrammarParser.Where_valueContext context)
        {
            QueryTree whereValueTree = new QueryTree();
            whereValueTree.TokenName = TokenName.WHERE_VALUE;
            whereValueTree.ProductionsList = new SubTrees();
            if(context.literal() != null)
            {
                IQueryTree literalTree = Visit(context.literal());
                whereValueTree.ProductionsList.Add(literalTree);
            }
            else if(context.NAME() != null)
            {
                QueryTree nameTree = new QueryTree();
                nameTree.TokenName = TokenName.NAME;
                nameTree.TokenValue = context.GetText();
                whereValueTree.ProductionsList.Add(nameTree);
            }

            return whereValueTree;
        }

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

        public override IQueryTree VisitLiteral([NotNull] QueryGrammarParser.LiteralContext context)
        {
            QueryTree literalTree = new QueryTree();
            literalTree.TokenName = TokenName.LITERAL;
            literalTree.ProductionsList = new SubTrees();

            QueryTree valueTree = new QueryTree();
            if (context.NUMBER() != null)
            {
                valueTree.TokenName = TokenName.NUMBER;
                valueTree.ProductionsList = new SubTrees();

                QueryTree integerTree = new QueryTree();
                integerTree.TokenName = TokenName.INTEGER;
                integerTree.TokenValue = context.NUMBER().GetText();
                valueTree.ProductionsList.Add(integerTree);
            }
            else if(context.STRING_VALUE() != null)
            {
                valueTree.TokenName = TokenName.STRING_VALUE;
                valueTree.TokenValue = context.STRING_VALUE().GetText().Replace("'", "\"");
            }
            else if(context.BOOL_VALUE() != null)
            {
                valueTree.TokenName = TokenName.BOOL_VALUE;
                valueTree.TokenValue = context.BOOL_VALUE().GetText();
            }
            else if(context.NULL_VALUE() != null)
            {
                valueTree.TokenName = TokenName.NULL_VALUE;
            }
            literalTree.ProductionsList.Add(valueTree);

            return literalTree;
        }

        public override IQueryTree VisitComparison_operator([NotNull] QueryGrammarParser.Comparison_operatorContext context)
        {
            QueryTree literalTree = new QueryTree();
            if (context.GREATER() != null)
            {
                literalTree.TokenName = TokenName.GREATER;
            }
            else if(context.LESS() != null)
            {
                literalTree.TokenName = TokenName.LESS;
            }
            else if (context.GREATER_EQUAL() != null)
            {
                literalTree.TokenName = TokenName.GREATER_EQUAL;
            }
            else if (context.LESS_EQUAL() != null)
            {
                literalTree.TokenName = TokenName.LESS_EQUAL;
            }
            else if (context.ISEQUAL() != null)
            {
                literalTree.TokenName = TokenName.ISEQUAL;
            }
            else if (context.NOT_EQUAL() != null)
            {
                literalTree.TokenName = TokenName.NOT_EQUAL;
            }

            return literalTree;
        }

        private IQueryTree createSemicolonTree()
        {
            QueryTree semicolonTree = new QueryTree();
            semicolonTree.TokenName = TokenName.SEMICOLON;
            semicolonTree.TokenValue = ";";
            return semicolonTree;
        }
    }
}
