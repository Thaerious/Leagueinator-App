lexer grammar StyleLexer;

OPAR  : '{' -> pushMode(KEY_MODE)       ;
DOT   : '.'                             ;
HASH  : '#'                             ;
STAR  : '*'                             ;
COMMA : ','                             ;

STRING     : [a-zA-Z_-]+                ;
WS         : [ \t\r\n]+ -> skip         ;
 
mode KEY_MODE;
COMMENT : '/' '/' -> pushMode(COMMENT_MODE) ;
KEY     : ([a-zA-Z]|'_'|'-')+               ;
COLON   : ':' -> pushMode(VALUE_MODE)       ;
CPAR    : '}' -> popMode                    ; 
MM_WS   : [ \t\r\n]+ -> skip                ;

mode VALUE_MODE;
SEMI       : ';' -> popMode             ;
VALUE      : ~[\t\r\n;]+                ;
VM_WS      : [\t\r\n]+ -> skip          ;

mode COMMENT_MODE;
NEWLINE        : [\n]+ -> popMode       ;
COMMENT_VALUE  : ~[\n]+                 ;