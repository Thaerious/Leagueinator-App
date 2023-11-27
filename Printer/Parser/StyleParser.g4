parser grammar StyleParser;
options { tokenVocab=StyleLexer; }

styles     : style* EOF
           ;

style      : selector OPAR property* CPAR           
           ;

selector   : STRING 
           | DOT STRING
           | HASH STRING
           | STAR
           ;

property   : KEY COLON VALUE SEMI
           ;