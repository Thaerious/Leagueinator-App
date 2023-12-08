parser grammar StyleParser;
options { tokenVocab=StyleLexer; }

styles     : style* EOF
           ;

style      : selectors OPAR property* CPAR           
           ;

selectors  : selector
           | selector COMMA selector
           ;

selector   : STRING 
           | DOT STRING
           | HASH STRING
           | STAR
           ;

property   : KEY COLON VALUE SEMI
           ;