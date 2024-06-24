using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Leagueinator.Printer;
using Leagueinator.Printer.Query;
using Leagueinator.Printer.Styles;
using Leagueinator.Utility;
using System.IO;
using System.Reflection;
using System.Text;
using static StyleParser;

namespace Leagueinator.CSSParser {
    internal class StyleListener(StyleSheet stylesheet) : StyleParserBaseListener {
        public readonly StyleSheet StyleSheet = stylesheet;
        private readonly List<Style> currentStyles = new();

        /// <summary>
        /// Retrieves the text for the input context with spaces between the tokens.
        /// </summary>
        /// <param name="inputContext"></param>
        /// <param name="sb"></param>
        /// <returns></returns>
        public static string GetText(RuleContext inputContext, StringBuilder? sb = null) {
            sb ??= new();

            if (inputContext.ChildCount == 0) {
                return string.Empty;
            }

            for (int i = 0; i < inputContext.ChildCount; i++) {
                if (inputContext.GetChild(i) is RuleContext childContext) {
                    GetText(childContext, sb);
                }
                else {
                    sb.Append(inputContext.GetChild(i).GetText());
                    sb.Append(' ');
                }
            }

            return sb.ToString().Trim();
        }

        public override void EnterImport_dir([NotNull] Import_dirContext context) {
            this.StyleSheet.Imports.Add(context.GetChild(1).GetText());
        }

        /// <summary>
        /// Treat the Style selector as a commas seperated selectorList and extract each Style name.
        /// </summary>
        /// <param name="context"></param>
        public override void EnterStyle([NotNull] global::StyleParser.StyleContext context) {
            List<SelectorContext> selectorList = [];
            SelectorsContext current = context.selectors();

            while (current != null) {
                selectorList.Add(current.selector());
                current = current.selectors();
            }

            selectorList.Reverse();

            foreach (SelectorContext selector in selectorList) {
                string selectorText = GetText(selector);

                var style = new Style(null) {
                    Selector = selectorText,
                    Specificity = QueryEngine.Specificity(selectorText, -this.StyleSheet.Count)
                };

                this.StyleSheet.Add(style);
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
                    throw new Exception(
                        $"Line {context.Start.Line}:{context.Start.Column}\n" +
                        $"Unknown property {key}"
                    );
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

