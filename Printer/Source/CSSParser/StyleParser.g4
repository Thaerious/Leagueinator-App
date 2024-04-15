parser grammar StyleParser;
options { tokenVocab=StyleLexer; }

styles     : import_dir* style* EOF
           ;

import_dir : I_DIR QUOTED_STRING
           ;

style      : selectors OPAR line* CPAR           
           | comment
           ;

selectors  : selector
           | selectors COMMA selector
           ;

selector   : MULT_SELECTOR
           | CLASS_SELECTOR
           | ID_SELECTOR
           | TAG_SELECTOR
           | ALL_SELECTOR
           | selector GT selector
           | selector selector
           ;

line       : property
           | comment
           ;

comment    : COMMENT COMMENT_VALUE NEWLINE
           | K_COMMENT COMMENT_VALUE NEWLINE
           ;

property   : KEY COLON VALUE SEMI
           ;