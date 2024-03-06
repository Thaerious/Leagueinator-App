using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Leagueinator.Printer;
using System.Diagnostics;

namespace Leagueinator.CSSParser {
    public static class StyleLoader {

        internal static Dictionary<string, NullableStyle> Load(string text) {
            var inputStream = new AntlrInputStream(text);
            var lexer = new StyleLexer(inputStream);
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new StyleParser(tokenStream);

            parser.AddErrorListener(new CustomErrorListener());

            var tree = parser.styles(); // Parse; start at the 'styles' rule
            var walker = new ParseTreeWalker();
            var listener = new StyleListener();
            walker.Walk(listener, tree); // Walk the tree with the listener

            return listener.Styles;
        }
    }
}

