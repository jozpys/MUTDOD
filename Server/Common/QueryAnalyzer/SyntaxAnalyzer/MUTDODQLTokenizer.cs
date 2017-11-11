/*
 * MUTDODQLTokenizer.cs
 *
 * THIS FILE HAS BEEN GENERATED AUTOMATICALLY. DO NOT EDIT!
 */

using System.IO;

using PerCederberg.Grammatica.Runtime;

/**
 * <remarks>A character stream tokenizer.</remarks>
 */
internal class MUTDODQLTokenizer : Tokenizer {

    /**
     * <summary>Creates a new tokenizer for the specified input
     * stream.</summary>
     *
     * <param name='input'>the input stream to read</param>
     *
     * <exception cref='ParserCreationException'>if the tokenizer
     * couldn't be initialized correctly</exception>
     */
    public MUTDODQLTokenizer(TextReader input)
        : base(input, true) {

        CreatePatterns();
    }

    /**
     * <summary>Initializes the tokenizer by creating all the token
     * patterns.</summary>
     *
     * <exception cref='ParserCreationException'>if the tokenizer
     * couldn't be initialized correctly</exception>
     */
    private void CreatePatterns() {
        TokenPattern  pattern;

        pattern = new TokenPattern((int) MUTDODQLConstants.WHITESPACE,
                                   "WHITESPACE",
                                   TokenPattern.PatternType.REGEXP,
                                   "[ \\t\\n\\r]+");
        pattern.Ignore = true;
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.UNKNOWN_CHAR,
                                   "UNKNOWN_CHAR",
                                   TokenPattern.PatternType.REGEXP,
                                   "[$^~`\\%#]");
        pattern.ErrorMessage = "unexpected token";
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.BYTE_TYPE,
                                   "BYTE_TYPE",
                                   TokenPattern.PatternType.REGEXP,
                                   "(BYTE)|(byte)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.SHORT_TYPE,
                                   "SHORT_TYPE",
                                   TokenPattern.PatternType.REGEXP,
                                   "(SHORT)|(short)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.INT_TYPE,
                                   "INT_TYPE",
                                   TokenPattern.PatternType.REGEXP,
                                   "(INT)|(int)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.LONG_TYPE,
                                   "LONG_TYPE",
                                   TokenPattern.PatternType.REGEXP,
                                   "(LONG)|(long)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.FLOAT_TYPE,
                                   "FLOAT_TYPE",
                                   TokenPattern.PatternType.REGEXP,
                                   "(FLOAT)|(float)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.DOUBLE_TYPE,
                                   "DOUBLE_TYPE",
                                   TokenPattern.PatternType.REGEXP,
                                   "(DOUBLE)|(double)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.CHAR_TYPE,
                                   "CHAR_TYPE",
                                   TokenPattern.PatternType.REGEXP,
                                   "(CHAR)|(char)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.STRING_TYPE,
                                   "STRING_TYPE",
                                   TokenPattern.PatternType.REGEXP,
                                   "(STRING)|(string)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.BOOL_TYPE,
                                   "BOOL_TYPE",
                                   TokenPattern.PatternType.REGEXP,
                                   "(BOOL)|(bool)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.VOID_TYPE,
                                   "VOID_TYPE",
                                   TokenPattern.PatternType.REGEXP,
                                   "(VOID)|(VOID)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.K_ADD,
                                   "K_ADD",
                                   TokenPattern.PatternType.REGEXP,
                                   "(ADD)|(add)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.K_ADD_DR,
                                   "K_ADD_DR",
                                   TokenPattern.PatternType.REGEXP,
                                   "(ADD DYNAMIC ROLE)|(add dynamic role)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.K_ARRAY,
                                   "K_ARRAY",
                                   TokenPattern.PatternType.REGEXP,
                                   "(ARRAY)|(array)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.K_AS,
                                   "K_AS",
                                   TokenPattern.PatternType.REGEXP,
                                   "(AS)|(as)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.K_ASC,
                                   "K_ASC",
                                   TokenPattern.PatternType.REGEXP,
                                   "(ASC)|(acs)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.K_ATTRIBUTE,
                                   "K_ATTRIBUTE",
                                   TokenPattern.PatternType.REGEXP,
                                   "(ATTRIBUTE)|(attribute)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.K_BETWEEN,
                                   "K_BETWEEN",
                                   TokenPattern.PatternType.REGEXP,
                                   "(BETWEEN)|(between)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.K_BREAK,
                                   "K_BREAK",
                                   TokenPattern.PatternType.REGEXP,
                                   "(BREAK)|(break)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.K_CASE,
                                   "K_CASE",
                                   TokenPattern.PatternType.REGEXP,
                                   "(CASE)|(case)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.K_CLASS,
                                   "K_CLASS",
                                   TokenPattern.PatternType.REGEXP,
                                   "(CLASS)|(class)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.K_DEFAULT,
                                   "K_DEFAULT",
                                   TokenPattern.PatternType.REGEXP,
                                   "(DEFAULT)|(default)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.K_DELETE,
                                   "K_DELETE",
                                   TokenPattern.PatternType.REGEXP,
                                   "(DELETE)|(delete)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.K_DEREF,
                                   "K_DEREF",
                                   TokenPattern.PatternType.REGEXP,
                                   "(DEREF)|(deref)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.K_DESC,
                                   "K_DESC",
                                   TokenPattern.PatternType.REGEXP,
                                   "(DESC)|(desc)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.K_DO,
                                   "K_DO",
                                   TokenPattern.PatternType.REGEXP,
                                   "(DO)|(do)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.K_DROLE,
                                   "K_DROLE",
                                   TokenPattern.PatternType.REGEXP,
                                   "(DYNAMIC ROLE)|(dynamic role)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.K_DROP,
                                   "K_DROP",
                                   TokenPattern.PatternType.REGEXP,
                                   "(DROP)|(drop)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.K_ELSE,
                                   "K_ELSE",
                                   TokenPattern.PatternType.REGEXP,
                                   "(ELSE)|(else)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.K_END,
                                   "K_END",
                                   TokenPattern.PatternType.REGEXP,
                                   "(END)|(end)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.K_ENDFOR,
                                   "K_ENDFOR",
                                   TokenPattern.PatternType.REGEXP,
                                   "(ENDFOR)|(endfor)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.K_ENDFOREACH,
                                   "K_ENDFOREACH",
                                   TokenPattern.PatternType.REGEXP,
                                   "(ENDFOREACH)|(endforeach)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.K_ENDIF,
                                   "K_ENDIF",
                                   TokenPattern.PatternType.REGEXP,
                                   "(ENDIF)|(endif)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.K_ENDSWITCH,
                                   "K_ENDSWITCH",
                                   TokenPattern.PatternType.REGEXP,
                                   "(ENDSWITCH)|(endswitch)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.K_ENDWHILE,
                                   "K_ENDWHILE",
                                   TokenPattern.PatternType.REGEXP,
                                   "(ENDWHILE)|(endwhile)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.K_EXPLICIT,
                                   "K_EXPLICIT",
                                   TokenPattern.PatternType.REGEXP,
                                   "(EXPLICIT)|(explicit)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.K_FOR,
                                   "K_FOR",
                                   TokenPattern.PatternType.REGEXP,
                                   "(FOR)|(for)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.K_FOREACH,
                                   "K_FOREACH",
                                   TokenPattern.PatternType.REGEXP,
                                   "(FOREACH)|(foreach)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.K_FROM,
                                   "K_FROM",
                                   TokenPattern.PatternType.REGEXP,
                                   "(FROM)|(from)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.K_GROUP,
                                   "K_GROUP",
                                   TokenPattern.PatternType.REGEXP,
                                   "(GROUP BY)|(group by)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.K_IF,
                                   "K_IF",
                                   TokenPattern.PatternType.REGEXP,
                                   "(IF) | (if)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.K_IN,
                                   "K_IN",
                                   TokenPattern.PatternType.REGEXP,
                                   "(IN)|(in)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.K_INTEGRAL_BLOCK,
                                   "K_INTEGRAL_BLOCK",
                                   TokenPattern.PatternType.REGEXP,
                                   "(INTEGRAL BLOCK)|(integral block)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.K_INTERFACE,
                                   "K_INTERFACE",
                                   TokenPattern.PatternType.REGEXP,
                                   "(INTERFACE)|(interface)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.K_IS_IN_DR,
                                   "K_IS_IN_DR",
                                   TokenPattern.PatternType.REGEXP,
                                   "(IS IN ROLE)|(is in role)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.IS_NULL,
                                   "IS_NULL",
                                   TokenPattern.PatternType.REGEXP,
                                   "(IS NULL)|(is null)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.IS_NOT_NULL,
                                   "IS_NOT_NULL",
                                   TokenPattern.PatternType.REGEXP,
                                   "(IS NOT NULL)|(is not null)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.K_METHOD,
                                   "K_METHOD",
                                   TokenPattern.PatternType.REGEXP,
                                   "(METHOD)|(method)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.K_NEW,
                                   "K_NEW",
                                   TokenPattern.PatternType.REGEXP,
                                   "(NEW)|(new)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.K_ONTIME,
                                   "K_ONTIME",
                                   TokenPattern.PatternType.REGEXP,
                                   "(ONTIME)|(ontime)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.K_ORDER,
                                   "K_ORDER",
                                   TokenPattern.PatternType.REGEXP,
                                   "(ORDER BY)|(order by)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.K_OUT,
                                   "K_OUT",
                                   TokenPattern.PatternType.REGEXP,
                                   "(OUT)|(out)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.K_RELATION,
                                   "K_RELATION",
                                   TokenPattern.PatternType.REGEXP,
                                   "(RELATION)|(relation)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.K_REMOVE_ALL_DR,
                                   "K_REMOVE_ALL_DR",
                                   TokenPattern.PatternType.REGEXP,
                                   "(REMOVE ALL ROLE)|(remove all role)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.K_REMOVE_DR,
                                   "K_REMOVE_DR",
                                   TokenPattern.PatternType.REGEXP,
                                   "(REMOVE ROLE)|(remove role)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.K_RETURN,
                                   "K_RETURN",
                                   TokenPattern.PatternType.REGEXP,
                                   "(RETURN)|(return)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.K_SELECT,
                                   "K_SELECT",
                                   TokenPattern.PatternType.REGEXP,
                                   "(SELECT)|(select)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.K_SET,
                                   "K_SET",
                                   TokenPattern.PatternType.REGEXP,
                                   "(SET)|(set)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.K_SWITCH,
                                   "K_SWITCH",
                                   TokenPattern.PatternType.REGEXP,
                                   "(SWITCH)|(switch)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.K_TEMPORAL,
                                   "K_TEMPORAL",
                                   TokenPattern.PatternType.REGEXP,
                                   "(TEMPORAL)|(temporal)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.K_THEN,
                                   "K_THEN",
                                   TokenPattern.PatternType.REGEXP,
                                   "(THEN)|(then)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.K_UNIQUE,
                                   "K_UNIQUE",
                                   TokenPattern.PatternType.REGEXP,
                                   "(UNIQUE)|(unique)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.K_UPDATE,
                                   "K_UPDATE",
                                   TokenPattern.PatternType.REGEXP,
                                   "(UPDATE)|(update)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.K_WHERE,
                                   "K_WHERE",
                                   TokenPattern.PatternType.REGEXP,
                                   "(WHERE)|(where)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.K_WHILE,
                                   "K_WHILE",
                                   TokenPattern.PatternType.REGEXP,
                                   "(WHILE)|(while)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.STATIC_CLS,
                                   "STATIC_CLS",
                                   TokenPattern.PatternType.REGEXP,
                                   "(STATIC)|(static)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.CONSTANT_CLS,
                                   "CONSTANT_CLS",
                                   TokenPattern.PatternType.REGEXP,
                                   "(CONSTANT)|(constant)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.INVARIANT_CLS,
                                   "INVARIANT_CLS",
                                   TokenPattern.PatternType.REGEXP,
                                   "(INVARIANT)|(invariant)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.UNION,
                                   "UNION",
                                   TokenPattern.PatternType.REGEXP,
                                   "(UNION)|(union)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.INTERSECTION,
                                   "INTERSECTION",
                                   TokenPattern.PatternType.REGEXP,
                                   "(INTERSECTION)|(intersection)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.DIFFERENCE,
                                   "DIFFERENCE",
                                   TokenPattern.PatternType.REGEXP,
                                   "(DIFFERENCE)|(difference)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.O_PAREN,
                                   "O_PAREN",
                                   TokenPattern.PatternType.STRING,
                                   "(");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.C_PAREN,
                                   "C_PAREN",
                                   TokenPattern.PatternType.STRING,
                                   ")");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.O_CURLY,
                                   "O_CURLY",
                                   TokenPattern.PatternType.STRING,
                                   "{");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.C_CURLY,
                                   "C_CURLY",
                                   TokenPattern.PatternType.STRING,
                                   "}");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.O_BRACK,
                                   "O_BRACK",
                                   TokenPattern.PatternType.STRING,
                                   "[");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.C_BRACK,
                                   "C_BRACK",
                                   TokenPattern.PatternType.STRING,
                                   "]");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.PARAM,
                                   "PARAM",
                                   TokenPattern.PatternType.STRING,
                                   "@");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.SYS_INFO,
                                   "SYS_INFO",
                                   TokenPattern.PatternType.STRING,
                                   "SystemInfo");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.CREATE_DB,
                                   "CREATE_DB",
                                   TokenPattern.PatternType.STRING,
                                   "CreateDatabase");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.TASKS,
                                   "TASKS",
                                   TokenPattern.PatternType.STRING,
                                   "tasks");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.PARALLEL_MTD,
                                   "PARALLEL_MTD",
                                   TokenPattern.PatternType.STRING,
                                   "parallel_method");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.ZERO_ONE,
                                   "ZERO_ONE",
                                   TokenPattern.PatternType.STRING,
                                   "<0..1>");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.ONE_ONE,
                                   "ONE_ONE",
                                   TokenPattern.PatternType.STRING,
                                   "<1..1>");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.ZERO_INFINITY,
                                   "ZERO_INFINITY",
                                   TokenPattern.PatternType.STRING,
                                   "<0..*>");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.ONE_INFINITY,
                                   "ONE_INFINITY",
                                   TokenPattern.PatternType.STRING,
                                   "<1..*>");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.DIGIT,
                                   "DIGIT",
                                   TokenPattern.PatternType.REGEXP,
                                   "0|1|2|3|4|5|6|7|8|9");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.STRING_VALUE,
                                   "STRING_VALUE",
                                   TokenPattern.PatternType.REGEXP,
                                   "[\"'\"][a-zA-Z_0-9@$^~`\\:\\-\\%#\\s]+[\"'\"]");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.BOOL_VALUE,
                                   "BOOL_VALUE",
                                   TokenPattern.PatternType.REGEXP,
                                   "(true)|(TRUE)|(false)|(FALSE)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.NULL_VALUE,
                                   "NULL_VALUE",
                                   TokenPattern.PatternType.REGEXP,
                                   "(NULL)|(null)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.ADD,
                                   "ADD",
                                   TokenPattern.PatternType.STRING,
                                   "+");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.SUB,
                                   "SUB",
                                   TokenPattern.PatternType.STRING,
                                   "-");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.MUL,
                                   "MUL",
                                   TokenPattern.PatternType.STRING,
                                   "*");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.DIV,
                                   "DIV",
                                   TokenPattern.PatternType.STRING,
                                   "/");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.MOD,
                                   "MOD",
                                   TokenPattern.PatternType.STRING,
                                   "%");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.GREATER,
                                   "GREATER",
                                   TokenPattern.PatternType.STRING,
                                   ">");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.LESS,
                                   "LESS",
                                   TokenPattern.PatternType.STRING,
                                   "<");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.GREATER_EQUAL,
                                   "GREATER_EQUAL",
                                   TokenPattern.PatternType.STRING,
                                   ">=");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.LESS_EQUAL,
                                   "LESS_EQUAL",
                                   TokenPattern.PatternType.STRING,
                                   "<=");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.ISEQUAL,
                                   "ISEQUAL",
                                   TokenPattern.PatternType.STRING,
                                   "==");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.NOT_EQUAL,
                                   "NOT_EQUAL",
                                   TokenPattern.PatternType.STRING,
                                   "<>");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.AND,
                                   "AND",
                                   TokenPattern.PatternType.STRING,
                                   "&");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.OR,
                                   "OR",
                                   TokenPattern.PatternType.STRING,
                                   "|");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.XOR,
                                   "XOR",
                                   TokenPattern.PatternType.STRING,
                                   "^");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.COND_AND,
                                   "COND_AND",
                                   TokenPattern.PatternType.STRING,
                                   "&&");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.COND_OR,
                                   "COND_OR",
                                   TokenPattern.PatternType.STRING,
                                   "||");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.ASSIGN,
                                   "ASSIGN",
                                   TokenPattern.PatternType.STRING,
                                   "=");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.ADD_ASSIGN,
                                   "ADD_ASSIGN",
                                   TokenPattern.PatternType.STRING,
                                   "+=");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.SUB_ASSIGN,
                                   "SUB_ASSIGN",
                                   TokenPattern.PatternType.STRING,
                                   "-=");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.MUL_ASSIGN,
                                   "MUL_ASSIGN",
                                   TokenPattern.PatternType.STRING,
                                   "*=");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.DIV_ASSIGN,
                                   "DIV_ASSIGN",
                                   TokenPattern.PatternType.STRING,
                                   "/=");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.MOD_ASSIGN,
                                   "MOD_ASSIGN",
                                   TokenPattern.PatternType.STRING,
                                   "%=");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.COUNT,
                                   "COUNT",
                                   TokenPattern.PatternType.REGEXP,
                                   "(COUNT)|(count)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.SUM,
                                   "SUM",
                                   TokenPattern.PatternType.REGEXP,
                                   "(SUM)|(sum)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.AVERAGE,
                                   "AVERAGE",
                                   TokenPattern.PatternType.REGEXP,
                                   "(AVG)|(avg)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.MIN,
                                   "MIN",
                                   TokenPattern.PatternType.REGEXP,
                                   "(MIN)|(min)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.MAX,
                                   "MAX",
                                   TokenPattern.PatternType.REGEXP,
                                   "(MAX)|(max)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.FIRST,
                                   "FIRST",
                                   TokenPattern.PatternType.REGEXP,
                                   "(FIRST)|(first)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.LAST,
                                   "LAST",
                                   TokenPattern.PatternType.REGEXP,
                                   "(LAST)|(last)");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.SELECTION,
                                   "SELECTION",
                                   TokenPattern.PatternType.STRING,
                                   ".");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.COMMA,
                                   "COMMA",
                                   TokenPattern.PatternType.STRING,
                                   ",");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.COLON,
                                   "COLON",
                                   TokenPattern.PatternType.STRING,
                                   ":");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.SEMICOLON,
                                   "SEMICOLON",
                                   TokenPattern.PatternType.STRING,
                                   ";");
        AddPattern(pattern);

        pattern = new TokenPattern((int) MUTDODQLConstants.NAME,
                                   "NAME",
                                   TokenPattern.PatternType.REGEXP,
                                   "[a-zA-Z][a-zA-Z_0-9]*");
        AddPattern(pattern);
    }
}
