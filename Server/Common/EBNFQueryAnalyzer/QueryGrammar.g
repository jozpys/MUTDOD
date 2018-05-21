grammar QueryGrammar;

start: statement SEMICOLON;

statement: PARAM system_operation
		 | get_stmt
		 | new_object
		 | update_object
		 | delete_object
		 | interface_declaration
		 | class_delcaration
		 | alter_interface
		 | alter_class
		 | drop_stmt
		 ;

system_operation: op = SYS_INFO
				| op = CREATE_DB db_name = NAME
				| op = RENAME_DB db_name = NAME COMMA db_new_name = NAME
				| op = DROP_DB db_name = NAME
				;

get_stmt: get_header where_clause?
		| O_PAREN get_stmt C_PAREN child_value
		| K_DEREF get_stmt
		;

get_header: class_name;

where_clause: K_WHERE clause;

and_or_clause: (AND|OR) clause;

clause: where_operation and_or_clause?
	  | O_PAREN clause C_PAREN
	  ;

where_operation: left=where_value where_operator right=where_value?;

where_value: literal
		   | NAME child_value?;

child_value: (SELECTION NAME)+;

literal: NUMBER 
	   | STRING_VALUE 
	   | BOOL_VALUE 
       | NULL_VALUE
	   ;

where_operator: is_null
			  | is_not_null
			  |	comparison_operator;

new_object: K_NEW class_name O_CURLY object_initialization_attributes_list C_CURLY;

object_initialization_attributes_list: object_initialization_element (COMMA object_initialization_element)* ;

object_initialization_element: NAME ASSIGN (literal | O_PAREN get_stmt C_PAREN | object_initialization_array);

object_initialization_array: O_CURLY ( literal_list? | O_PAREN get_stmt C_PAREN ) C_CURLY;

literal_list: literal ( COMMA literal )*;


update_object: K_UPDATE O_PAREN get_stmt C_PAREN K_SET O_CURLY object_update_attributes_list C_CURLY;

object_update_attributes_list: object_update_element (COMMA object_update_element)* ;

object_update_element: NAME ASSIGN (literal | O_PAREN get_stmt C_PAREN );


delete_object: K_DELETE O_PAREN get_stmt C_PAREN;


interface_declaration: K_INTERFACE K_TEMPORAL? NAME parent_type? O_CURLY attribute_dec_stm* method_dec_stm* relation_dec_stm* C_CURLY;

attribute_dec_stm: K_ATTRIBUTE K_ARRAY? dataType NAME SEMICOLON;

method_dec_stm: K_METHOD (dataType|VOID_TYPE) NAME O_BRACK method_params? C_BRACK SEMICOLON;

method_params: method_param ( COMMA method_param )*;

method_param: (K_IN|K_OUT) dataType NAME;

relation_dec_stm: K_RELATION dataType cardinalyty? NAME SEMICOLON;


class_delcaration: K_CLASS classType? K_TEMPORAL? cardinalyty? class_name parent_type? O_CURLY cls_attribute_dec_stm* cls_method_dec_stm* cls_relation_dec_stm* C_CURLY;

cls_attribute_dec_stm: K_ATTRIBUTE K_ARRAY? dataType NAME SEMICOLON;

cls_method_dec_stm: K_METHOD (dataType|VOID_TYPE) NAME method_params method_body;

method_body: O_CURLY operation* (K_RETURN operation)? C_CURLY;

cls_relation_dec_stm: K_RELATION dataType cardinalyty? NAME SEMICOLON;

parent_type: COLON NAME (COMMA NAME)*;


alter_interface: K_ALTER K_INTERFACE class_name O_CURLY (add_attribute_dec_stm | drop_attribute_dec_stm)* (add_method_dec_stm | drop_method_dec_stm)* (add_relation_dec_stm | drop_relation_dec_stm)* C_CURLY;

add_attribute_dec_stm: K_ADD attribute_dec_stm;

drop_attribute_dec_stm: K_DROP K_ATTRIBUTE NAME SEMICOLON;

add_method_dec_stm: K_ADD method_dec_stm;

drop_method_dec_stm: K_DROP K_METHOD NAME method_params SEMICOLON;

add_relation_dec_stm: K_ADD relation_dec_stm;

drop_relation_dec_stm: K_RELATION cardinalyty? NAME SEMICOLON;


alter_class: K_ALTER K_CLASS class_name O_CURLY (add_cls_attribute_dec_stm | drop_cls_attribute_dec_stm)* (add_cls_method_dec_stm | drop_cls_method_dec_stm)* (add_cls_relation_dec_stm | drop_cls_relation_dec_stm )* C_CURLY;

add_cls_attribute_dec_stm: K_ADD cls_attribute_dec_stm;

drop_cls_attribute_dec_stm: K_DROP K_ATTRIBUTE NAME SEMICOLON;

add_cls_method_dec_stm: K_ADD cls_method_dec_stm;

drop_cls_method_dec_stm: K_DROP K_METHOD NAME method_params SEMICOLON;

add_cls_relation_dec_stm: K_ADD cls_relation_dec_stm;

drop_cls_relation_dec_stm: K_RELATION cardinalyty? NAME SEMICOLON;



drop_stmt: K_DROP (K_CLASS|K_INTERFACE) class_name;



operation: (math_operation | assign_operation) SEMICOLON;

assign_operation : dataType? NAME (SELECTION NAME)* assign_operator coercion? math_operation;

math_operation: operation_value
			  | STRING_VALUE
			  | BOOL_VALUE
			  | array_value
			  ;

operation_value: value (operator coercion? value)*;

array_value: arr_name=NAME O_BRACK ((NAME (SELECTION NAME)*)|literal) C_BRACK;

coercion: O_PAREN dataType C_PAREN;

value: NUMBER 
     | NAME;


class_name: NAME;

dataType: BYTE_TYPE
		|SHORT_TYPE
		|INT_TYPE
		|LONG_TYPE
		|FLOAT_TYPE
		|DOUBLE_TYPE
		|CHAR_TYPE
		|STRING_TYPE
		|BOOL_TYPE
		|NAME
		;

BYTE_TYPE:	'BYTE'|'byte' ;
SHORT_TYPE:	'SHORT'|'short' ;
INT_TYPE:	'INT'|'int' ;
LONG_TYPE:	'LONG'|'long';
FLOAT_TYPE: 'FLOAT'|'float';
DOUBLE_TYPE:'DOUBLE'|'double';
CHAR_TYPE:	'CHAR'|'char';
STRING_TYPE:'STRING'|'string';
BOOL_TYPE:	'BOOL'|'bool';
VOID_TYPE:	'VOID'|'VOID';

cardinalyty: ZERO_ONE
		   | ONE_ONE
		   | ZERO_INFINITY
		   | ONE_INFINITY;

ZERO_ONE:		'<0..1>';
ONE_ONE:		'<1..1>';
ZERO_INFINITY:	'<0..*>';
ONE_INFINITY:	'<1..*>';

classType:	STATIC_CLS
			|CONSTANT_CLS
			|INVARIANT_CLS;

STATIC_CLS:			'STATIC'|'static';
CONSTANT_CLS:		'CONSTANT'|'constant';
INVARIANT_CLS:		'INVARIANT'|'invariant';

K_ADD:		'ADD'|'add';
K_ALL:		'ALL'|'all';
K_ALTER:	'ALTER'|'alter';
K_ARRAY:	'ARRAY'|'array';
K_AS:		'AS'|'as';
K_ASC:		'ASC'|'acs';
K_ATTRIBUTE:'ATTRIBUTE'|'attribute';
K_BETWEEN:	'BETWEEN'|'between';
K_BLOCK:	'BLOCK'|'block';
K_BREAK:	'BREAK'|'break';
K_CASE:		'CASE'|'case';
K_CLASS:	'CLASS'|'class';
K_DEFAULT:	'DEFAULT'|'default';
K_DELETE:	'DELETE'|'delete';
K_DEREF:	'DEREF'|'deref';
K_DESC:		'DESC'|'desc';
K_DO:		'DO'|'do';
K_DROP:		'DROP'|'drop';
K_DYNAMIC:	'DYNAMIC'|'dynamic';
K_ELSE:		'ELSE'|'else';
K_END:		'END'|'end';
K_ENDFOR:	'ENDFOR'|'endfor';
K_ENDFOREACH:'ENDFOREACH'|'endforeach';
K_ENDIF:	'ENDIF'|'endif';
K_ENDSWITCH:'ENDSWITCH'|'endswitch';
K_ENDWHILE:	'ENDWHILE'|'endwhile';
K_EXPLICIT:	'EXPLICIT'|'explicit';
K_FOR:		'FOR'|'for';
K_FOREACH:	'FOREACH'|'foreach';
K_FROM:		'FROM'|'from';
K_GROUP:	'GROUP BY'|'group by';
K_IF:		'IF'|'if';
K_IN:		'IN'|'in';
K_INTEGRAL:	'INTEGRAL'|'integral';
K_INTERFACE:'INTERFACE'|'interface';
K_IS:		'IS'|'is';
K_METHOD:	'METHOD'|'method';
K_NEW:		'NEW'|'new';
K_NOT:		'NOT'|'not';
K_NULL:		'NULL'|'null';
K_ONTIME:	'ONTIME'|'ontime';
K_ORDER:	'ORDER BY'|'order by';
K_OUT:		'OUT'|'out';
K_RELATION:	'RELATION'|'relation';
K_REMOVE:	'REMOVE'|'remove';
K_RETURN:	'RETURN'|'return';
K_ROLE:		'ROLE'|'role';
K_SELECT:	'SELECT'|'select';
K_SET:		'SET'|'set';
K_SWITCH:	'SWITCH'|'switch';
K_TEMPORAL:	'TEMPORAL'|'temporal';
K_THEN:		'THEN'|'then';
K_UNIQUE:	'UNIQUE'|'unique';
K_UPDATE:	'UPDATE'|'update';
K_WHERE:	'WHERE'|'where';
K_WHILE:	'WHILE'|'while';

k_add_dr:	K_ADD K_DYNAMIC K_ROLE;
k_drole:	K_DYNAMIC K_ROLE;
k_integral_block:	K_INTEGRAL K_BLOCK;
k_is_in_dr:	K_IS K_IN K_ROLE;
is_null:	K_IS K_NULL;
is_not_null:K_IS K_NOT K_NULL;
k_remove_all_dr:	K_REMOVE K_ALL K_ROLE;
k_remove_dr:K_REMOVE K_ROLE;

O_PAREN:	'(';
C_PAREN:	')';
O_CURLY:	'{';
C_CURLY:	'}';
O_BRACK:	'[';
C_BRACK:	']';

arithmetic_operator: ADD
				   | SUB
				   | MUL
				   | DIV
				   | MOD
				   ;

operator: arithmetic_operator 
		| comparison_operator 
		| logical_operator
		;

ADD:	'+';
SUB:	'-';
MUL:	'*';
DIV:	'/';
MOD:	'%';

comparison_operator: GREATER
				   |LESS
				   |GREATER_EQUAL
				   |LESS_EQUAL
				   |ISEQUAL
				   |NOT_EQUAL;

GREATER:		'>';
LESS:			'<';
GREATER_EQUAL:	'>=';
LESS_EQUAL:		'<=';
ISEQUAL:		'==';
NOT_EQUAL:		'<>';

assign_operator: ASSIGN
			   | ADD_ASSIGN
			   | SUB_ASSIGN
			   | MUL_ASSIGN
			   | DIV_ASSIGN
			   | MOD_ASSIGN
			   ;

ASSIGN:			'=';
ADD_ASSIGN:		'+=';
SUB_ASSIGN:		'-=';
MUL_ASSIGN:		'*=';
DIV_ASSIGN:		'/=';
MOD_ASSIGN:		'%=';

logical_operator: AND
				| OR
				| XOR
				| COND_AND
				| COND_OR
				;

AND:		'&';
OR:			'|';
XOR:		'^';
COND_AND:	'&&';
COND_OR:	'||';

SELECTION:	'.';
COMMA:		',';
COLON:		':';
SEMICOLON:	';';

PARAM:		'@';
SYS_INFO:	'SystemInfo';
CREATE_DB:	'CreateDatabase';
RENAME_DB:	'RenameDatabase';
DROP_DB:	'DropDatabase';
TASKS:		'tasks';
PARALLEL_MTD:'parallel_method';

NUMBER: INTEGER FLOAT_PRESICION?;
FLOAT_PRESICION: SELECTION DIGIT+;
INTEGER: SUB? DIGIT+; 
DIGIT: '0'|'1'|'2'|'3'|'4'|'5'|'6'|'7'|'8'|'9';

STRING_VALUE: '\'' (~[\r\n\'])* '\'';
BOOL_VALUE:	'true'|'TRUE'|'false'|'FALSE';      
NULL_VALUE: 'NULL'|'null';

NAME:		[a-zA-Z][a-zA-Z_0-9]*;


WS : [ \t\r\n]+ -> skip ;