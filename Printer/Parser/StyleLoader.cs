using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace Leagueinator.CSSParser {

    public static class StyleLoader {
        public static LoadedStyles Load(string text) {
            var inputStream = new AntlrInputStream(text);
            var lexer = new StyleLexer(inputStream);
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new global::StyleParser(tokenStream);

            parser.AddErrorListener(new CustomErrorListener());

            var tree = parser.styles(); // Load; start at the 'styles' rule
            var walker = new ParseTreeWalker();
            var listener = new StyleListener();
            walker.Walk(listener, tree); // Walk the tree with the listener

            return listener.Styles;
        }
    }
}

