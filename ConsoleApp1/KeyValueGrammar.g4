grammar KeyValueGrammar;

// Parser rules
pairs : (pair ';')+    ;
pair  : KEY '=' value  ;

value : KEY | VALUE | ALL ;

// Lexer rules
KEY   : [a-zA-Z]+      {Console.WriteLine($"KEY '{Text}'");};
VALUE : [a-zA-Z0-9]+   {Console.WriteLine($"VALUE '{Text}'");};
ALL   : ~[ \t\r\n]+?   {Console.WriteLine($"ALL '{Text}'");};

// Skip whitespace
WS: [ \t\r\n]+ -> skip ;