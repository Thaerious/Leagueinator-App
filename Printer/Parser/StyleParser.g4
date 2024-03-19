parser grammar StyleParser;
options { tokenVocab=StyleLexer; }

styles     : style* EOF
           ;

style      : selectors OPAR line* CPAR           
           ;

selectors  : selector
           | selector COMMA selector
           ;

selector   : STRING 
           | DOT STRING
           | HASH STRING
           | AT STRING
           | STAR
           | selector GT selector
           | selector selector
           ;

line       : property
           | comment
           ;

comment    : COMMENT COMMENT_VALUE NEWLINE
           ;

property   : KEY COLON VALUE SEMI
           ;