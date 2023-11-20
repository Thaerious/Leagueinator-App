using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace Leagueinator.CSSParser {
    public class StyleDeclaration : Dictionary<string, string> {
        public string Selector { get; init; }

        public StyleDeclaration(string selector) {
            this.Selector = selector;
        }

        public T ParseValue<T>(string key) {
            object? parsed;
            bool success = MultiParse.TryParse(this[key], typeof(T), out parsed);
            if (!success) throw new Exception($"Unable to parse {this[key]} into type {typeof(T).Name}");
            return (T)parsed!;
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{this.Selector} {{");
            foreach (var key in this.Keys) {
                sb.AppendLine($"\t{key}: {this[key]};");
            }
            sb.AppendLine($"}}");
            return sb.ToString();
        }

        public void ApplyTo(object? target) {
            if (target == null) throw new ArgumentNullException(nameof(target));
            var type = target.GetType();

            foreach (FieldInfo field in type.GetFields()) {
                CSS? css = field.GetCustomAttribute<CSS>();
                if (css == null) continue;

                var key = css.Key != "" ? css.Key : field.Name;
                if (!this.ContainsKey(key)) continue;

                MultiParse.TryParse(this[key], field.FieldType, out object? t);
                field.SetValue(target, t);
            }
        }
    }

    internal class StyleListener : StyleParserBaseListener {
        public readonly Dictionary<string, StyleDeclaration> Styles = new();
        private StyleDeclaration? style = null;

        public override void EnterStyle([NotNull] StyleParser.StyleContext context) {
            var selector = context.selector().GetText();

            if (!Styles.ContainsKey(selector)) {
                Styles[selector] = new(selector);
            }

            style = Styles[selector];
        }

        private List<ITerminalNode> AllTokens(ParserRuleContext source) {
            List<ITerminalNode> terminalNodes = new();
            foreach (IParseTree child in source.children) {
                if (child is ITerminalNode node) {
                    terminalNodes.Add(node);
                }
                if (child is ParserRuleContext context) {
                    terminalNodes.AddRange(AllTokens(context));
                }
            }
            return terminalNodes;
        }

        public override void EnterProperty([NotNull] StyleParser.PropertyContext context) {
            if (style == null) throw new Exception("StyleDeclaration not found.");

            var key = context.children[0].GetText();
            var val = context.children[2].GetText();

            style[key] = val.Trim();
        }
    }

    public static class StyleLoader {

        public static Dictionary<string, StyleDeclaration>? Load(string text) {
            var inputStream = new AntlrInputStream(text);
            var lexer = new StyleLexer(inputStream);
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new StyleParser(tokenStream);

            parser.AddErrorListener(new CustomErrorListener());

            var tree = parser.styles(); // Parse; start at the 'expr' rule
            var walker = new ParseTreeWalker();
            var listener = new StyleListener();
            walker.Walk(listener, tree); // Walk the tree with the listener

            return listener?.Styles;
        }
    }

    public class CustomErrorListener : IAntlrErrorListener<IToken> {

        public void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e) {
            throw new Exception($"Line {line}:{charPositionInLine} - {msg}");
        }
    }
}

