using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Leagueinator.Printer;
using Leagueinator.Utility;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace Leagueinator.CSSParser {

    internal class StyleListener : StyleParserBaseListener {
        public readonly Dictionary<string, Style> Styles = new();
        private Style style = new();

        public override void EnterStyle([NotNull] StyleParser.StyleContext context) {
            var selector = context.selector().GetText();
            if (!Styles.ContainsKey(selector)) Styles[selector] = new(selector);
            style = new Flex(selector);
        }

        public override void EnterProperty([NotNull] StyleParser.PropertyContext context) {
            var key = context.children[0].GetText().ToPlainCase();
            var val = context.children[2].GetText();
            var field = Style.Fields[key];

            MultiParse.TryParse(val.Trim(), field.FieldType, out object? newObject);
            field.SetValue(this.style, newObject);
        }
    }

    public static class StyleLoader {

        public static Dictionary<string, Style> Load(string text) {
            var inputStream = new AntlrInputStream(text);
            var lexer = new StyleLexer(inputStream);
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new StyleParser(tokenStream);

            parser.AddErrorListener(new CustomErrorListener());

            var tree = parser.styles(); // Parse; start at the 'expr' rule
            var walker = new ParseTreeWalker();
            var listener = new StyleListener();
            walker.Walk(listener, tree); // Walk the tree with the listener

            return listener.Styles;
        }
    }

    public class CustomErrorListener : IAntlrErrorListener<IToken> {
        public void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e) {
            throw new Exception($"Line {line}:{charPositionInLine} - {msg}");
        }
    }
}

