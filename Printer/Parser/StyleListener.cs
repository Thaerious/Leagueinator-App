using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Leagueinator.Printer;
using Leagueinator.Utility;
using System.Reflection;

namespace Leagueinator.CSSParser {

    internal class StyleListener : StyleParserBaseListener {
        public readonly Dictionary<string, Style> Styles = new();
        private Style style = new();

        public override void EnterStyle([NotNull] StyleParser.StyleContext context) {
            var selector = context.selector().GetText();
            if (!Styles.ContainsKey(selector)) Styles[selector] = new Flex(selector);
            style = Styles[selector];
        }

        public override void EnterProperty([NotNull] StyleParser.PropertyContext context) {
            var key = context.children[0].GetText().ToFlatCase();
            var val = context.children[2].GetText();

            try {
                if (Style.Fields.ContainsKey(key)) {
                    var field = Style.Fields[key];
                    var r = MultiParse.TryParse(val.Trim(), field.FieldType, out object? newObject);
                    field.SetValue(this.style, newObject);
                }
                else if (Style.Properties.ContainsKey(key)) {
                    var prop = Style.Properties[key];
                    MultiParse.TryParse(val.Trim(), prop.PropertyType, out object? newObject);
                    prop.SetValue(this.style, newObject);
                }
            }
            catch (TargetInvocationException ex) {
                CardinalParseException? inner = ex.InnerException as CardinalParseException;
                if (inner == null) throw;

                string msg = $"Line {context.Start.Line}:{context.Start.Column}\n";
                msg += $"{inner.Message}\n";
                msg += $"Type: {inner.Type.Name}\n";
                msg += $"Source: {inner.SourceString}\n";

                throw new Exception(msg);
            }
            catch (Exception ex) {
                string msg = $"Line {context.Start.Line}:{context.Start.Column}\n";
                msg += ex.Message;
                throw new Exception(msg);
            }            
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

