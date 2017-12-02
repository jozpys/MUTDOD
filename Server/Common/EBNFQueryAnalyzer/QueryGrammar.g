grammar QueryGrammar;

start: statement SEMICOLON;

statement: PARAM system_operation;

system_operation: op = SYS_INFO
				| op = CREATE_DB
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

K_ADD:		'ADD'|'add';
K_ALL:		'ALL'|'all';
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

K_ADD_DR:	K_ADD K_DYNAMIC K_ROLE;
K_DROLE:	K_DYNAMIC K_ROLE;
K_INTEGRAL_BLOCK:	K_INTEGRAL K_BLOCK;
K_IS_IN_DR:	K_IS K_IN K_ROLE;
IS_NULL:	K_IS K_NULL;
IS_NOT_NULL:K_IS K_NOT K_NULL;
K_REMOVE_ALL_DR:	K_REMOVE K_ALL K_ROLE;
K_REMOVE_DR:K_REMOVE K_ROLE;

SELECTION:	'.';
COMMA:		',';
COLON:		':';
SEMICOLON:	';';

PARAM:		'@';
SYS_INFO:	'SystemInfo';
CREATE_DB:	'CreateDatabase';
TASKS:		'tasks';
PARALLEL_MTD:'parallel_method';


WS : [ \t\r\n]+ -> skip ;