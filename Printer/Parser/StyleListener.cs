using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Leagueinator.Printer;
using Leagueinator.Utility;
using System.Diagnostics;
using System.Reflection;

namespace Leagueinator.CSSParser {
    internal class StyleListener : StyleParserBaseListener {
        public readonly Dictionary<string, NullableStyle> Styles = new();
        private readonly List<NullableStyle> currentStyles = new();

        /// <summary>
        /// Treat the style selector as a commas seperated list and extract each style name.
        /// </summary>
        /// <param name="context"></param>
        public override void EnterStyle([NotNull] StyleParser.StyleContext context) {
            var selectors = context.selectors().GetText();

            foreach (var selector in selectors.Split(",")) {
                if (!Styles.ContainsKey(selector)) Styles[selector] = new NullableStyle(selector);
                var style = Styles[selector];
                currentStyles.Add(style);
            }
        }

        public override void ExitStyle([NotNull] StyleParser.StyleContext context) {
            currentStyles.Clear();
        }

        private void SetStyleField(FieldInfo field, object? newObject) {
            foreach (var style in currentStyles) {
                field.SetValue(style, newObject);
            }
        }

        private void SetStyleProperty(PropertyInfo prop, object? newObject) {
            foreach (var style in currentStyles) {
                prop.SetValue(style, newObject);
            }
        }

        public override void EnterProperty([NotNull] StyleParser.PropertyContext context) {
            var key = context.children[0].GetText().ToFlatCase();
            var val = context.children[2].GetText();

            try {
                Debug.WriteLine($"Enter Property {key} {Style.Fields.ContainsKey(key)} {Style.Properties.ContainsKey(key)}");
                if (Style.Fields.ContainsKey(key)) {
                    var field = NullableStyle.Fields[key];
                    var r = MultiParse.TryParse(val.Trim(), field.FieldType, out object? newObject);
                    Debug.WriteLine($" : result {r} {newObject}");
                    SetStyleField(field, newObject);
                }
                else if (Style.Properties.ContainsKey(key)) {
                    var prop = NullableStyle.Properties[key];
                    MultiParse.TryParse(val.Trim(), prop.PropertyType, out object? newObject);
                    SetStyleProperty(prop, newObject);
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
                Debug.WriteLine("\n");
                Debug.WriteLine(ex);
                Debug.WriteLine("\n");
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

