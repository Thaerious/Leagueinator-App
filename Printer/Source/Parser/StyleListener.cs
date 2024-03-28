using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Leagueinator.Printer;
using Leagueinator.Printer.Query;
using Leagueinator.Printer.Styles;
using System.Reflection;
using System.Text;
using Leagueinator.Utility;
using static StyleParser;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Leagueinator.CSSParser {
    internal class StyleListener : StyleParserBaseListener {
        public readonly LoadedStyles Styles = new();
        private readonly List<Style> currentStyles = new();

        private static string CollectSelectorContext(SelectorContext selector) {
            string input = selector.GetText();
            string pattern = "(>)";

            string[] result = Regex.Split(input, pattern)
                                   .Where(s => !string.IsNullOrEmpty(s)) 
                                   .ToArray();

            return result.DelString(" ");
        }

        /// <summary>
        /// Treat the style selector as a commas seperated list and extract each style name.
        /// </summary>
        /// <param name="context"></param>
        public override void EnterStyle([NotNull] global::StyleParser.StyleContext context) {
            List<SelectorContext> list = [];
            SelectorsContext current = context.selectors();

            while (current != null) {
                list.Add(current.selector());
                current = current.selectors();
            }

            list.Reverse();

            foreach (SelectorContext selector in list) {
                string selectorText = CollectSelectorContext(selector);

                var style = new Style() {
                    Selector = selectorText,
                    Specificity = QueryEngine.Specificity(selectorText, -this.Styles.Count)
                };

                this.Styles.Add(style);
                this.currentStyles.Add(style);
            }
        }

        public override void ExitStyle([NotNull] global::StyleParser.StyleContext context) {
            this.currentStyles.Clear();
        }

        public override void EnterProperty([NotNull] StyleParser.PropertyContext context) {
            var key = context.children[0].GetText().ToFlatCase();
            var val = context.children[2].GetText().Trim();

            try {
                if (Style.CSSProperties.ContainsKey(key)) {
                    var prop = Style.CSSProperties[key];
                    CSS? css = prop.GetCustomAttribute<CSS>();
                    if (css is null) return;

                    foreach (var style in this.currentStyles) {
                        css.TryParse(style, val, prop);
                    }
                }
                else {
                    string msg
                        = $"Line {context.Start.Line}:{context.Start.Column}\n"
                        + $"Unknown property {key}";
                    throw new Exception(msg);
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
        }
    }

    public class CustomErrorListener : IAntlrErrorListener<IToken> {
        public void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e) {
            throw new Exception($"Line {line}:{charPositionInLine} - {msg}");
        }
    }
}

