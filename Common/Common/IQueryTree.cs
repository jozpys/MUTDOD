using System.Collections.Generic;

namespace MUTDOD.Common
{
    public interface IQueryTree
    {
        TokenName TokenName { get; }
        string TokenValue { get; }
        int TokenLine { get; }
        int TokenCol { get; }
        ISubTrees ProductionsList { get; }
    }

    public interface ISubTrees : IList<IQueryTree> { }

    public enum TokenName
    {
        UNKNOWN,
        STATEMENT,
        SYSTEM_OPERATION,
        GET_SYSTEM_INFO,
        CREATE_DATABASE,
        NAME,
        GET,
        GET_STM,
        K_DEREF,
        GET_HEADER,
        CLASS_NAME,
        NEW_OBJECT,
        OBJECT_INITIALIZATION_ATTRIBUTES_LIST,
        OBJECT_INITIALIZATION_ELEMENT,
        ATTRIBUTE_NAME,
        CLASS_DECLARATION,
        INTERFACE_DECLARATION,
        ATTRIBUTE_DEC_STM,
        METHOD_DEC_STM,
        METHOD_NAME,
        RELATION_DEC_STM,
        DROP,
        WHERE_CLAUSE,
        K_WHERE,
        AND,
        OR,
        CLAUSE,
        AND_OR_CLAUSE,
        WHERE_OPERATION,
        WHERE_TAIL,
        WHERE_OPERATOR,
        IS_NULL,
        IS_NOT_NULL,
        COMPARISON_OPERATOR,
        WHERE_VALUE,
        DATA_TYPE,
        BYTE_TYPE,
        SHORT_TYPE,
        INT_TYPE,
        LONG_TYPE,
        FLOAT_TYPE,
        DOUBLE_TYPE,
        CHAR_TYPE,
        STRING_TYPE,
        BOOL_TYPE,
        LITERAL,
        NUMBER,
        FLOAT_PRESICION,
        INTEGER,
        STRING_VALUE,
        BOOL_VALUE,
        NULL_VALUE,
        GREATER,
        LESS,
        GREATER_EQUAL,
        LESS_EQUAL,
        ISEQUAL,
        NOT_EQUAL,
        SEMICOLON
    }
}
