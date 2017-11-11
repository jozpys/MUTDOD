/*
 * MUTDODQLParser.cs
 *
 * THIS FILE HAS BEEN GENERATED AUTOMATICALLY. DO NOT EDIT!
 */

using System.IO;

using PerCederberg.Grammatica.Runtime;

/**
 * <remarks>A token stream parser.</remarks>
 */
internal class MUTDODQLParser : RecursiveDescentParser {

    /**
     * <summary>An enumeration with the generated production node
     * identity constants.</summary>
     */
    private enum SynteticPatterns {
        SUBPRODUCTION_1 = 3001,
        SUBPRODUCTION_2 = 3002,
        SUBPRODUCTION_3 = 3003,
        SUBPRODUCTION_4 = 3004,
        SUBPRODUCTION_5 = 3005,
        SUBPRODUCTION_6 = 3006,
        SUBPRODUCTION_7 = 3007,
        SUBPRODUCTION_8 = 3008,
        SUBPRODUCTION_9 = 3009,
        SUBPRODUCTION_10 = 3010,
        SUBPRODUCTION_11 = 3011,
        SUBPRODUCTION_12 = 3012,
        SUBPRODUCTION_13 = 3013,
        SUBPRODUCTION_14 = 3014,
        SUBPRODUCTION_15 = 3015,
        SUBPRODUCTION_16 = 3016,
        SUBPRODUCTION_17 = 3017,
        SUBPRODUCTION_18 = 3018,
        SUBPRODUCTION_19 = 3019,
        SUBPRODUCTION_20 = 3020,
        SUBPRODUCTION_21 = 3021,
        SUBPRODUCTION_22 = 3022,
        SUBPRODUCTION_23 = 3023,
        SUBPRODUCTION_24 = 3024,
        SUBPRODUCTION_25 = 3025,
        SUBPRODUCTION_26 = 3026,
        SUBPRODUCTION_27 = 3027,
        SUBPRODUCTION_28 = 3028,
        SUBPRODUCTION_29 = 3029,
        SUBPRODUCTION_30 = 3030,
        SUBPRODUCTION_31 = 3031,
        SUBPRODUCTION_32 = 3032,
        SUBPRODUCTION_33 = 3033,
        SUBPRODUCTION_34 = 3034,
        SUBPRODUCTION_35 = 3035,
        SUBPRODUCTION_36 = 3036,
        SUBPRODUCTION_37 = 3037,
        SUBPRODUCTION_38 = 3038,
        SUBPRODUCTION_39 = 3039,
        SUBPRODUCTION_40 = 3040,
        SUBPRODUCTION_41 = 3041,
        SUBPRODUCTION_42 = 3042,
        SUBPRODUCTION_43 = 3043,
        SUBPRODUCTION_44 = 3044
    }

    /**
     * <summary>Creates a new parser with a default analyzer.</summary>
     *
     * <param name='input'>the input stream to read from</param>
     *
     * <exception cref='ParserCreationException'>if the parser
     * couldn't be initialized correctly</exception>
     */
    public MUTDODQLParser(TextReader input)
        : base(input) {

        CreatePatterns();
    }

    /**
     * <summary>Creates a new parser.</summary>
     *
     * <param name='input'>the input stream to read from</param>
     *
     * <param name='analyzer'>the analyzer to parse with</param>
     *
     * <exception cref='ParserCreationException'>if the parser
     * couldn't be initialized correctly</exception>
     */
    public MUTDODQLParser(TextReader input, MUTDODQLAnalyzer analyzer)
        : base(input, analyzer) {

        CreatePatterns();
    }

    /**
     * <summary>Creates a new tokenizer for this parser. Can be overridden
     * by a subclass to provide a custom implementation.</summary>
     *
     * <param name='input'>the input stream to read from</param>
     *
     * <returns>the tokenizer created</returns>
     *
     * <exception cref='ParserCreationException'>if the tokenizer
     * couldn't be initialized correctly</exception>
     */
    protected override Tokenizer NewTokenizer(TextReader input) {
        return new MUTDODQLTokenizer(input);
    }

    /**
     * <summary>Initializes the parser by creating all the production
     * patterns.</summary>
     *
     * <exception cref='ParserCreationException'>if the parser
     * couldn't be initialized correctly</exception>
     */
    private void CreatePatterns() {
        ProductionPattern             pattern;
        ProductionPatternAlternative  alt;

        pattern = new ProductionPattern((int) MUTDODQLConstants.STATEMENT,
                                        "STATEMENT");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_1, 1, -1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.DATA_TYPE,
                                        "DATA_TYPE");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.BYTE_TYPE, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.SHORT_TYPE, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.INT_TYPE, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.LONG_TYPE, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.FLOAT_TYPE, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.DOUBLE_TYPE, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.CHAR_TYPE, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.STRING_TYPE, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.BOOL_TYPE, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.NAME, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.CARDINALITY,
                                        "CARDINALITY");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.ZERO_ONE, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.ONE_ONE, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.ZERO_INFINITY, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.ONE_INFINITY, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.OPERATOR,
                                        "OPERATOR");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.ARITHMETIC_OPERATOR, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.COMPARISON_OPERATOR, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.LOGICAL_OPERATOR, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.ARITHMETIC_OPERATOR,
                                        "ARITHMETIC_OPERATOR");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.ADD, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.SUB, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.MUL, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.DIV, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.MOD, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.COMPARISON_OPERATOR,
                                        "COMPARISON_OPERATOR");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.GREATER, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.LESS, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.GREATER_EQUAL, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.LESS_EQUAL, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.ISEQUAL, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.NOT_EQUAL, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.LOGICAL_OPERATOR,
                                        "LOGICAL_OPERATOR");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.AND, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.OR, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.XOR, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.COND_AND, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.COND_OR, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.ASSIGN_OPERATOR,
                                        "ASSIGN_OPERATOR");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.ASSIGN, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.ADD_ASSIGN, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.SUB_ASSIGN, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.MUL_ASSIGN, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.DIV_ASSIGN, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.MOD_ASSIGN, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.CLASS_TYPE,
                                        "CLASS_TYPE");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.STATIC_CLS, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.CONSTANT_CLS, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.INVARIANT_CLS, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.AGGREGATE_FUNCTION,
                                        "AGGREGATE_FUNCTION");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.COUNT, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.SUM, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.AVERAGE, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.MIN, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.MAX, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.FIRST, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.LAST, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.ARRAY_SIZE,
                                        "ARRAY_SIZE");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.O_BRACK, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.POS_INTEGER, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.C_BRACK, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.ARRAY_DECLARATION,
                                        "ARRAY_DECLARATION");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.K_ARRAY, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.DATA_TYPE, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.ARRAY_SIZE, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.NAME, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.VARIABLE_DECLARATION,
                                        "VARIABLE_DECLARATION");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.DATA_TYPE, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.NAME, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.ASSIGN_TAIL, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.SYSTEM_OPERATION,
                                        "SYSTEM_OPERATION");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.GET_SYSTEM_INFO, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.CREATE_DATABASE, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.GET_SYSTEM_INFO,
                                        "GET_SYSTEM_INFO");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.PARAM, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.SYS_INFO, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.CREATE_DATABASE,
                                        "CREATE_DATABASE");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.PARAM, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.CREATE_DB, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.NAME, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.NEW_OBJECT,
                                        "NEW_OBJECT");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.K_NEW, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.CLASS_NAME, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.O_CURLY, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.OBJECT_INITIALIZATION_ATTRIBUTES_LIST, 0, 1);
        alt.AddToken((int) MUTDODQLConstants.C_CURLY, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.OBJECT_INITIALIZATION_ATTRIBUTES_LIST,
                                        "OBJECT_INITIALIZATION_ATTRIBUTES_LIST");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.OBJECT_INITIALIZATION_ELEMENT, 1, 1);
        alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_2, 0, -1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.OBJECT_INITIALIZATION_ELEMENT,
                                        "OBJECT_INITIALIZATION_ELEMENT");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.ATTRIBUTE_NAME, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.ASSIGN, 1, 1);
        alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_4, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.SET_OPERATION,
                                        "SET_OPERATION");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.UNION, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.INTERSECTION, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.DIFFERENCE, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.GET,
                                        "GET");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.GET_STM, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.CAST_STM, 0, 1);
        alt.AddToken((int) MUTDODQLConstants.O_PAREN, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.GET_STM, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.C_PAREN, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.GET_TAIL, 0, -1);
        alt.AddProduction((int) MUTDODQLConstants.ORDER_CAUSE, 0, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_5, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.CONDITIONAL_GET, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.CONDITIONAL_GET,
                                        "CONDITIONAL_GET");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.MUL, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.GET_STM, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.CAST_STM,
                                        "CAST_STM");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.O_PAREN, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.CLASS_NAME, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.C_PAREN, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.GET_STM,
                                        "GET_STM");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.K_DEREF, 0, 1);
        alt.AddProduction((int) MUTDODQLConstants.GET_HEADER, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.ONTIME_STM, 0, 1);
        alt.AddProduction((int) MUTDODQLConstants.WHERE_CLAUSE, 0, 1);
        alt.AddProduction((int) MUTDODQLConstants.GET_TAIL, 0, 1);
        alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_6, 0, -1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.GET_HEADER,
                                        "GET_HEADER");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.CLASS_NAME, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_7, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_8, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.GET_ELEMENTS,
                                        "GET_ELEMENTS");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.GET_ELEMENTS_ATOM, 1, 1);
        alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_9, 0, -1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.GET_ELEMENTS_ATOM,
                                        "GET_ELEMENTS_ATOM");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.CLASS_NAME, 1, 1);
        alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_10, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.GET_TAIL,
                                        "GET_TAIL");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.SELECTION, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.GET_ATTRIBUTES, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.GET_ATTRIBUTES,
                                        "GET_ATTRIBUTES");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.GET_ATTRIBUTES_ATOM, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_11, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.ATTIBUTES_LIST,
                                        "ATTIBUTES_LIST");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.COMMA, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.GET_ATTRIBUTES_ATOM, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.GET_ATTRIBUTES_ATOM,
                                        "GET_ATTRIBUTES_ATOM");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_12, 0, -1);
        alt.AddProduction((int) MUTDODQLConstants.ATTRIBUTE_NAME, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_13, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.WHERE_CLAUSE,
                                        "WHERE_CLAUSE");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.K_WHERE, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.CLAUSE, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.AND_OR_CLAUSE, 0, -1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.AND_OR_CLAUSE,
                                        "AND_OR_CLAUSE");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_14, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.CLAUSE, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.AND_OR_CLAUSE, 0, -1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.CLAUSE,
                                        "CLAUSE");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.O_PAREN, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.WHERE_OPERATION, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.AND_OR_CLAUSE, 0, 1);
        alt.AddToken((int) MUTDODQLConstants.C_PAREN, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.WHERE_OPERATION, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.AND_OR_CLAUSE, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.WHERE_OPERATION,
                                        "WHERE_OPERATION");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.WHERE_VALUE, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.WHERE_TAIL, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.WHERE_TAIL,
                                        "WHERE_TAIL");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.WHERE_OPERATOR, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.WHERE_VALUE, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.WHERE_VALUE,
                                        "WHERE_VALUE");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.LITERAL, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_15, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.PARAM, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.GET, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.WHERE_SUBELEMENT,
                                        "WHERE_SUBELEMENT");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.SELECTION, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.NAME, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.WHERE_OPERATOR,
                                        "WHERE_OPERATOR");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.IS_NULL, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.IS_NOT_NULL, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.K_BETWEEN, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.O_PAREN, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.NUMBER, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.COMMA, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.NUMBER, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.C_PAREN, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.COMPARISON_OPERATOR, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.ORDER_CAUSE,
                                        "ORDER_CAUSE");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.K_ORDER, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.NAME, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.ORDER_SUBELEMENT, 0, -1);
        alt.AddProduction((int) MUTDODQLConstants.ORDER_TYPE, 0, 1);
        alt.AddProduction((int) MUTDODQLConstants.ORDER_ELEMENT, 0, -1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.ORDER_SUBELEMENT,
                                        "ORDER_SUBELEMENT");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.SELECTION, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.NAME, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.ORDER_ELEMENT,
                                        "ORDER_ELEMENT");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.COMMA, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.NAME, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.ORDER_SUBELEMENT, 0, -1);
        alt.AddProduction((int) MUTDODQLConstants.ORDER_TYPE, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.ORDER_TYPE,
                                        "ORDER_TYPE");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.K_ASC, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.K_DESC, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.ONTIME_STM,
                                        "ONTIME_STM");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.K_ONTIME, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.O_PAREN, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.STRING_VALUE, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.C_PAREN, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.DELETE_OBJECT,
                                        "DELETE_OBJECT");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.K_DELETE, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.O_PAREN, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.GET, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.C_PAREN, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.DROP_STM,
                                        "DROP_STM");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.K_DROP, 1, 1);
        alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_16, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.CLASS_NAME, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.UPDATE_OBJECT,
                                        "UPDATE_OBJECT");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.K_UPDATE, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.O_PAREN, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.GET, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.C_PAREN, 1, 1);
        alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_17, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.O_CURLY, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.ATTRIBUTES_LIST, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.C_CURLY, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.ATTRIBUTES_LIST,
                                        "ATTRIBUTES_LIST");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.ATTRIBUTE_NAME, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.ASSIGN, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.LITERAL, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.ATTRIBUTES_LIST_ELEMENT, 0, -1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.ATTRIBUTES_LIST_ELEMENT,
                                        "ATTRIBUTES_LIST_ELEMENT");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.COMMA, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.ATTRIBUTE_NAME, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.ASSIGN, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.LITERAL, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.INTERFACE_DECLARATION,
                                        "INTERFACE_DECLARATION");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.K_INTERFACE, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.K_TEMPORAL, 0, 1);
        alt.AddToken((int) MUTDODQLConstants.NAME, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.PARENT_TYPE, 0, -1);
        alt.AddToken((int) MUTDODQLConstants.O_CURLY, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.ATTRIBUTE_DEC_STM, 0, -1);
        alt.AddProduction((int) MUTDODQLConstants.METHOD_DEC_STM, 0, -1);
        alt.AddProduction((int) MUTDODQLConstants.RELATION_DEC_STM, 0, -1);
        alt.AddToken((int) MUTDODQLConstants.C_CURLY, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.PARENT_TYPE,
                                        "PARENT_TYPE");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.COLON, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.NAME, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.ATTRIBUTE_DEC_STM,
                                        "ATTRIBUTE_DEC_STM");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.K_ATTRIBUTE, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.DATA_TYPE, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.NAME, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.SEMICOLON, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.RELATION_DEC_STM,
                                        "RELATION_DEC_STM");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.K_RELATION, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.DATA_TYPE, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.CARDINALITY, 0, 1);
        alt.AddToken((int) MUTDODQLConstants.NAME, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.SEMICOLON, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.METHOD_DEC_STM,
                                        "METHOD_DEC_STM");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.K_METHOD, 1, 1);
        alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_18, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.NAME, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.METHOD_PARAMS, 0, 1);
        alt.AddToken((int) MUTDODQLConstants.SEMICOLON, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.METHOD_PARAMS,
                                        "METHOD_PARAMS");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.O_BRACK, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.PARAMS_LIST, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.C_BRACK, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.PARAMS_TAIL,
                                        "PARAMS_TAIL");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.COMMA, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.PARAMS_LIST, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.PARAMS_LIST,
                                        "PARAMS_LIST");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_19, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.DATA_TYPE, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.NAME, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.PARAMS_TAIL, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.CLASS_DECLARATION,
                                        "CLASS_DECLARATION");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.K_CLASS, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.CLASS_TYPE, 0, 1);
        alt.AddToken((int) MUTDODQLConstants.K_TEMPORAL, 0, 1);
        alt.AddProduction((int) MUTDODQLConstants.CARDINALITY, 0, 1);
        alt.AddProduction((int) MUTDODQLConstants.CLASS_NAME, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.PARENT_TYPE, 0, -1);
        alt.AddToken((int) MUTDODQLConstants.O_CURLY, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.CLS_ATTRIBUTE_DEC_STM, 0, -1);
        alt.AddProduction((int) MUTDODQLConstants.CLS_METHOD_DEC_STM, 0, -1);
        alt.AddProduction((int) MUTDODQLConstants.CLS_RELATION_DEC_STM, 0, -1);
        alt.AddToken((int) MUTDODQLConstants.C_CURLY, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.CLS_ATTRIBUTE_DEC_STM,
                                        "CLS_ATTRIBUTE_DEC_STM");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.K_ATTRIBUTE, 1, 1);
        alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_22, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.SEMICOLON, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.CLS_METHOD_DEC_STM,
                                        "CLS_METHOD_DEC_STM");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.K_METHOD, 1, 1);
        alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_23, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.METHOD_NAME, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.METHOD_PARAMS, 0, 1);
        alt.AddProduction((int) MUTDODQLConstants.METHOD_BODY, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.METHOD_BODY,
                                        "METHOD_BODY");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.O_CURLY, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.OPERATION, 0, -1);
        alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_24, 0, 1);
        alt.AddToken((int) MUTDODQLConstants.C_CURLY, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.CLS_RELATION_DEC_STM,
                                        "CLS_RELATION_DEC_STM");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.K_RELATION, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.DATA_TYPE, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.CARDINALITY, 0, 1);
        alt.AddToken((int) MUTDODQLConstants.NAME, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.SEMICOLON, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.CLASS_ASSIGN_OPERATION,
                                        "CLASS_ASSIGN_OPERATION");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.DATA_TYPE, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.NAME, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.ASSIGN_TAIL, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.DROLE_DECLARATION,
                                        "DROLE_DECLARATION");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.K_DROLE, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.K_TEMPORAL, 0, 1);
        alt.AddProduction((int) MUTDODQLConstants.DROLE_NAME, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.PARENT_TYPE, 0, -1);
        alt.AddToken((int) MUTDODQLConstants.O_CURLY, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.DROLE_ATTRIBUTE_DEC_STM, 0, -1);
        alt.AddProduction((int) MUTDODQLConstants.DROLE_METHOD_DEC_STM, 0, -1);
        alt.AddProduction((int) MUTDODQLConstants.DROLE_RELATION_DEC_STM, 0, -1);
        alt.AddToken((int) MUTDODQLConstants.C_CURLY, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.DROLE_ATTRIBUTE_DEC_STM,
                                        "DROLE_ATTRIBUTE_DEC_STM");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.K_ATTRIBUTE, 1, 1);
        alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_27, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.SEMICOLON, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.DROLE_METHOD_DEC_STM,
                                        "DROLE_METHOD_DEC_STM");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.K_METHOD, 1, 1);
        alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_28, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.METHOD_NAME, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.METHOD_PARAMS, 0, 1);
        alt.AddProduction((int) MUTDODQLConstants.METHOD_BODY, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.DROLE_RELATION_DEC_STM,
                                        "DROLE_RELATION_DEC_STM");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.K_RELATION, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.DATA_TYPE, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.CARDINALITY, 0, 1);
        alt.AddToken((int) MUTDODQLConstants.NAME, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.SEMICOLON, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.DROLE_ASSIGN_OPERATION,
                                        "DROLE_ASSIGN_OPERATION");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.DATA_TYPE, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.CARDINALITY, 0, 1);
        alt.AddToken((int) MUTDODQLConstants.NAME, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.ASSIGN_TAIL, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.DR_OPERATIONS,
                                        "DR_OPERATIONS");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.ADD_DR_STM, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.IS_IN_DR_STM, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.REMOVE_DR_STM, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.REMOVE_ALL_STM, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.ADD_DR_STM,
                                        "ADD_DR_STM");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.K_ADD_DR, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.O_BRACK, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.DROLE_NAME, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.O_PAREN, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.OBJECT_INITIALIZATION_ATTRIBUTES_LIST, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.C_PAREN, 1, 1);
        alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_29, 0, -1);
        alt.AddToken((int) MUTDODQLConstants.C_BRACK, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.COLON, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.GET_STM, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.IS_IN_DR_STM,
                                        "IS_IN_DR_STM");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.K_IS_IN_DR, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.O_BRACK, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.DROLE_NAME, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.C_BRACK, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.COLON, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.GET_STM, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.REMOVE_DR_STM,
                                        "REMOVE_DR_STM");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.K_REMOVE_DR, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.O_BRACK, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.DROLE_NAME, 1, 1);
        alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_30, 0, -1);
        alt.AddToken((int) MUTDODQLConstants.C_BRACK, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.COLON, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.GET_STM, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.REMOVE_ALL_STM,
                                        "REMOVE_ALL_STM");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.K_REMOVE_ALL_DR, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.COLON, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.GET_STM, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.LOOP,
                                        "LOOP");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.LOOP_FOREACH, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.LOOP_FOR, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.LOOP_WHILE, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.LOOP_FOREACH,
                                        "LOOP_FOREACH");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.K_FOREACH, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.O_PAREN, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.GET_STM, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.C_PAREN, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.K_AS, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.NAME, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.K_DO, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.DO_STM, 0, -1);
        alt.AddToken((int) MUTDODQLConstants.K_ENDFOREACH, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.DO_STM,
                                        "DO_STM");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.OPERATION, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_31, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.LOOP_FOR,
                                        "LOOP_FOR");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.K_FOR, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.ITERATOR_DEC, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.K_DO, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.DO_STM, 0, -1);
        alt.AddToken((int) MUTDODQLConstants.K_ENDFOR, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.ITERATOR_DEC,
                                        "ITERATOR_DEC");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.O_PAREN, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.DATA_TYPE, 0, 1);
        alt.AddToken((int) MUTDODQLConstants.NAME, 1, 1);
        alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_32, 0, 1);
        alt.AddToken((int) MUTDODQLConstants.SEMICOLON, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.LITERAL, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.C_PAREN, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.LOOP_WHILE,
                                        "LOOP_WHILE");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.K_WHILE, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.CONDITIONAL_QUERY, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.K_DO, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.DO_STM, 0, -1);
        alt.AddToken((int) MUTDODQLConstants.K_ENDWHILE, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.CONDITIONAL_QUERY,
                                        "CONDITIONAL_QUERY");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.CONDITIONAL_GET, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.COND_QUERY, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.COND_QUERY,
                                        "COND_QUERY");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.COND_QUERY_ARGUMENT, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.COMPARISON_OPERATOR, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.COND_QUERY_ARGUMENT, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.COND_QUERY_ARGUMENT,
                                        "COND_QUERY_ARGUMENT");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_35, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_36, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.IF_STM,
                                        "IF_STM");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.K_IF, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.CONDITIONAL_QUERY, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.K_THEN, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.DO_STM, 0, -1);
        alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_37, 0, 1);
        alt.AddToken((int) MUTDODQLConstants.K_ENDIF, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.SWITCH_STM,
                                        "SWITCH_STM");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.K_SWITCH, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.SWITCH_HEADER, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.SWITCH_BODY, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.K_ENDSWITCH, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.SWITCH_HEADER,
                                        "SWITCH_HEADER");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.O_PAREN, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.GET, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.C_PAREN, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.COLON, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.VARIABLE,
                                        "VARIABLE");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.NAME, 1, 1);
        alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_38, 0, -1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.SWITCH_BODY,
                                        "SWITCH_BODY");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.CASE_QUERY, 1, -1);
        alt.AddProduction((int) MUTDODQLConstants.DEFAULT_QUERY, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.CASE_QUERY,
                                        "CASE_QUERY");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.K_CASE, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.LITERAL, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.COLON, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.DO_STM, 0, -1);
        alt.AddProduction((int) MUTDODQLConstants.BREAK, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.DEFAULT_QUERY,
                                        "DEFAULT_QUERY");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.K_DEFAULT, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.COLON, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.DO_STM, 0, -1);
        alt.AddProduction((int) MUTDODQLConstants.BREAK, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.BREAK,
                                        "BREAK");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.K_BREAK, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.SEMICOLON, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.INTEGRAL_BLOCK_STM,
                                        "INTEGRAL_BLOCK_STM");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.K_INTEGRAL_BLOCK, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.O_PAREN, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.ITG_PARAMS, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.C_PAREN, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.K_DO, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.WORK, 0, 1);
        alt.AddToken((int) MUTDODQLConstants.K_END, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.ITG_PARAMS,
                                        "ITG_PARAMS");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.ITG_PARAM, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.COLON, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.LITERAL, 1, 1);
        alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_39, 0, -1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.ITG_PARAM,
                                        "ITG_PARAM");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.PARAM, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.TASKS, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.PARAM, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.PARALLEL_MTD, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.WORK,
                                        "WORK");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_40, 1, -1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.OPERATION,
                                        "OPERATION");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_41, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.SEMICOLON, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.ASSIGN_OPERATION,
                                        "ASSIGN_OPERATION");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.DATA_TYPE, 0, 1);
        alt.AddToken((int) MUTDODQLConstants.NAME, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.NAME_TAIL, 0, -1);
        alt.AddProduction((int) MUTDODQLConstants.ASSIGN_TAIL, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.ASSIGN_TAIL,
                                        "ASSIGN_TAIL");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.ASSIGN_OPERATOR, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.COERCION, 0, 1);
        alt.AddProduction((int) MUTDODQLConstants.MATH_OPERATION, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.MATH_OPERATION,
                                        "MATH_OPERATION");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_43, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.OPERATION_TAIL,
                                        "OPERATION_TAIL");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.OPERATOR, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.COERCION, 0, 1);
        alt.AddProduction((int) MUTDODQLConstants.VALUE, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.OPERATION_TAIL, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.ARRAY_VALUE,
                                        "ARRAY_VALUE");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.ARRAY_NAME, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.O_BRACK, 1, 1);
        alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_44, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.C_BRACK, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.NAME_TAIL, 0, -1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.COERCION,
                                        "COERCION");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.O_PAREN, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.DATA_TYPE, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.C_PAREN, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.VALUE,
                                        "VALUE");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.NUMBER, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.NAME, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.NAME_TAIL,
                                        "NAME_TAIL");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.SELECTION, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.NAME, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.NUMBER,
                                        "NUMBER");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.INTEGER, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.FLOAT_PRESICION, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.FLOAT_PRESICION,
                                        "FLOAT_PRESICION");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.SELECTION, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.DIGIT, 1, -1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.INTEGER,
                                        "INTEGER");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.POS_INTEGER, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.NONPOS_INTEGER, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.POS_INTEGER,
                                        "POS_INTEGER");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.DIGIT, 1, -1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.NONPOS_INTEGER,
                                        "NONPOS_INTEGER");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.SUB, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.DIGIT, 1, -1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.LITERAL,
                                        "LITERAL");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.NUMBER, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.STRING_VALUE, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.BOOL_VALUE, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.NULL_VALUE, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.OBJECT_NAME,
                                        "OBJECT_NAME");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.NAME, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.CLASS_NAME,
                                        "CLASS_NAME");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.NAME, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.ATTRIBUTE_NAME,
                                        "ATTRIBUTE_NAME");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.NAME, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.METHOD_NAME,
                                        "METHOD_NAME");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.NAME, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.ALIAS_NAME,
                                        "ALIAS_NAME");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.NAME, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.DROLE_NAME,
                                        "DROLE_NAME");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.NAME, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) MUTDODQLConstants.ARRAY_NAME,
                                        "ARRAY_NAME");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.NAME, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_1,
                                        "Subproduction1");
        pattern.Synthetic = true;
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.INTERFACE_DECLARATION, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.SEMICOLON, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.CLASS_DECLARATION, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.SEMICOLON, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.NEW_OBJECT, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.SEMICOLON, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.UPDATE_OBJECT, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.SEMICOLON, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.DROP_STM, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.SEMICOLON, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.GET, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.SEMICOLON, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.DELETE_OBJECT, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.SEMICOLON, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.LOOP, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.SEMICOLON, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.IF_STM, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.SEMICOLON, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.SWITCH_STM, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.SEMICOLON, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.SYSTEM_OPERATION, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.SEMICOLON, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.INTEGRAL_BLOCK_STM, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.SEMICOLON, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.DROLE_DECLARATION, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.SEMICOLON, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.DR_OPERATIONS, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.SEMICOLON, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_2,
                                        "Subproduction2");
        pattern.Synthetic = true;
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.COMMA, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.OBJECT_INITIALIZATION_ELEMENT, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_3,
                                        "Subproduction3");
        pattern.Synthetic = true;
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.O_PAREN, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.GET_STM, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.C_PAREN, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_4,
                                        "Subproduction4");
        pattern.Synthetic = true;
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.LITERAL, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_3, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_5,
                                        "Subproduction5");
        pattern.Synthetic = true;
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.AGGREGATE_FUNCTION, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.O_PAREN, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.GET_STM, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.C_PAREN, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_6,
                                        "Subproduction6");
        pattern.Synthetic = true;
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.SET_OPERATION, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.GET_STM, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_7,
                                        "Subproduction7");
        pattern.Synthetic = true;
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.O_CURLY, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.GET_ELEMENTS, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.C_CURLY, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_8,
                                        "Subproduction8");
        pattern.Synthetic = true;
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.K_EXPLICIT, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.O_PAREN, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.CLASS_NAME, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.C_PAREN, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_9,
                                        "Subproduction9");
        pattern.Synthetic = true;
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.COMMA, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.GET_ELEMENTS_ATOM, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_10,
                                        "Subproduction10");
        pattern.Synthetic = true;
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.K_AS, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.ALIAS_NAME, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_11,
                                        "Subproduction11");
        pattern.Synthetic = true;
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.O_PAREN, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.GET_ATTRIBUTES_ATOM, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.ATTIBUTES_LIST, 0, -1);
        alt.AddToken((int) MUTDODQLConstants.C_PAREN, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_12,
                                        "Subproduction12");
        pattern.Synthetic = true;
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.ATTRIBUTE_NAME, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.SELECTION, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_13,
                                        "Subproduction13");
        pattern.Synthetic = true;
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.AGGREGATE_FUNCTION, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.O_PAREN, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.GET_ATTRIBUTES_ATOM, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.C_PAREN, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_14,
                                        "Subproduction14");
        pattern.Synthetic = true;
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.AND, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.OR, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_15,
                                        "Subproduction15");
        pattern.Synthetic = true;
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.NAME, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.WHERE_SUBELEMENT, 0, -1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_16,
                                        "Subproduction16");
        pattern.Synthetic = true;
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.K_CLASS, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.K_INTERFACE, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_17,
                                        "Subproduction17");
        pattern.Synthetic = true;
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.K_SET, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.K_ADD, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_18,
                                        "Subproduction18");
        pattern.Synthetic = true;
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.DATA_TYPE, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.VOID_TYPE, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_19,
                                        "Subproduction19");
        pattern.Synthetic = true;
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.K_IN, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.K_OUT, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_20,
                                        "Subproduction20");
        pattern.Synthetic = true;
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.ATTRIBUTE_NAME, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.CLASS_ASSIGN_OPERATION, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_21,
                                        "Subproduction21");
        pattern.Synthetic = true;
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.DATA_TYPE, 1, 1);
        alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_20, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_22,
                                        "Subproduction22");
        pattern.Synthetic = true;
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_21, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.ARRAY_DECLARATION, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_23,
                                        "Subproduction23");
        pattern.Synthetic = true;
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.DATA_TYPE, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.VOID_TYPE, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_24,
                                        "Subproduction24");
        pattern.Synthetic = true;
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.K_RETURN, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.OPERATION, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_25,
                                        "Subproduction25");
        pattern.Synthetic = true;
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.ATTRIBUTE_NAME, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.DROLE_ASSIGN_OPERATION, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_26,
                                        "Subproduction26");
        pattern.Synthetic = true;
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.DATA_TYPE, 1, 1);
        alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_25, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_27,
                                        "Subproduction27");
        pattern.Synthetic = true;
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_26, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.ARRAY_DECLARATION, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_28,
                                        "Subproduction28");
        pattern.Synthetic = true;
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.DATA_TYPE, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.VOID_TYPE, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_29,
                                        "Subproduction29");
        pattern.Synthetic = true;
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.COMMA, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.DROLE_NAME, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.O_PAREN, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.OBJECT_INITIALIZATION_ATTRIBUTES_LIST, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.C_PAREN, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_30,
                                        "Subproduction30");
        pattern.Synthetic = true;
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.COMMA, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.DROLE_NAME, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_31,
                                        "Subproduction31");
        pattern.Synthetic = true;
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.UPDATE_OBJECT, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.SEMICOLON, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_32,
                                        "Subproduction32");
        pattern.Synthetic = true;
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.ASSIGN_OPERATOR, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.MATH_OPERATION, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_33,
                                        "Subproduction33");
        pattern.Synthetic = true;
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.SELECTION, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.NAME, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_34,
                                        "Subproduction34");
        pattern.Synthetic = true;
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.NAME, 1, 1);
        alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_33, 0, -1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_35,
                                        "Subproduction35");
        pattern.Synthetic = true;
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.LITERAL, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_34, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_36,
                                        "Subproduction36");
        pattern.Synthetic = true;
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.AGGREGATE_FUNCTION, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.O_PAREN, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.GET_STM, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.C_PAREN, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_37,
                                        "Subproduction37");
        pattern.Synthetic = true;
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.K_ELSE, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.DO_STM, 0, -1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_38,
                                        "Subproduction38");
        pattern.Synthetic = true;
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.SELECTION, 1, 1);
        alt.AddToken((int) MUTDODQLConstants.NAME, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_39,
                                        "Subproduction39");
        pattern.Synthetic = true;
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.COMMA, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.ITG_PARAMS, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_40,
                                        "Subproduction40");
        pattern.Synthetic = true;
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.LOOP_FOREACH, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.OPERATION, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_41,
                                        "Subproduction41");
        pattern.Synthetic = true;
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.MATH_OPERATION, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.ASSIGN_OPERATION, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_42,
                                        "Subproduction42");
        pattern.Synthetic = true;
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.VALUE, 1, 1);
        alt.AddProduction((int) MUTDODQLConstants.OPERATION_TAIL, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_43,
                                        "Subproduction43");
        pattern.Synthetic = true;
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_42, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.STRING_VALUE, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.BOOL_VALUE, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.ARRAY_VALUE, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_44,
                                        "Subproduction44");
        pattern.Synthetic = true;
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) MUTDODQLConstants.NAME, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) MUTDODQLConstants.LITERAL, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);
    }
}
