using Antlr4.Runtime.Tree;
using Antlr4.Runtime;
using Leagueinator.Printer.Elements;
using System.Collections;
using System.Diagnostics;
using Leagueinator.CSSParser;

namespace Leagueinator.Printer.Styles {
    public class LoadedStyles : IEnumerable<Style> {
        private readonly Dictionary<string, StyleSheet> Loaded = [];

        public static LoadedStyles LoadFromString(string path, string text) {
            LoadedStyles loadedStyles = new();
            loadedStyles.Loaded[path] = new StyleSheet().LoadFromString(text);
            return loadedStyles;
        }

        public static LoadedStyles LoadFromFile(string path) {
            LoadedStyles loadedStyles = new();
            string? dir = Path.GetDirectoryName(path) ?? throw new FileNotFoundException($"Unknown path: {path}");
            loadedStyles.Loaded[path] = new StyleSheet().LoadFromString(File.ReadAllText(path));

            foreach (string import in loadedStyles.Loaded[path].Imports) {
                string sub = import.Substring(1, import.Length - 2);
                string importPath = Path.Combine(dir, sub);

                if (loadedStyles.Loaded.ContainsKey(importPath)) continue;
                loadedStyles.Loaded[importPath] = new StyleSheet().LoadFromString(File.ReadAllText(importPath));
            }

            return loadedStyles;
        }

        public void ApplyTo(Element root) {
            // apply defined styles
            foreach (Style style in this) {
                foreach (Element element in root[style.Selector]) {
                    element.Style.MergeWith(style);
                }
            }

            ApplyParentStyles(root);

            // apply default styles
            TreeWalker<Element>.Walk(root, ele => {
                ele.Style.MergeWith(Style.Default);
            });
        }

        private static void ApplyParentStyles(Element root) {
            Queue<Element> queue = new Queue<Element>();
            queue.Enqueue(root);

            while (queue.Count > 0) {
                Element parent = queue.Dequeue();
                foreach (Element child in parent.Children) {
                    Style.MergeInheritedStyles(child.Style, parent.Style);
                    queue.Enqueue(child);
                }
            }
        }

        public int Count => Loaded.Values.Sum(styleSheet => styleSheet.Count);

        public IEnumerable<Style> AsEnumerable() {
            foreach (StyleSheet stylesheet in this.Loaded.Values) {
                foreach (Style style in stylesheet) {
                    yield return style;
                }
            }
        }

        public IEnumerator<Style> GetEnumerator() {
            return this.AsEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.AsEnumerable().GetEnumerator();
        }
    }


    /// <summary>
    /// A collection of styles with the query string as the key.
    /// </summary>
    internal class StyleSheet : IEnumerable<Style> {
        internal List<string> Imports = [];
        private readonly SortedList<Style, Style> Inner = [];

        public Style this[int index] {
            get => this.Inner.Keys[index];
        }

        internal void Add(Style style) => Inner.Add(style, style);

        public StyleSheet LoadFromString(string text) {
            var inputStream = new AntlrInputStream(text);
            var lexer = new StyleLexer(inputStream);
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new global::StyleParser(tokenStream);

            parser.AddErrorListener(new CustomErrorListener());

            var tree = parser.styles(); // LoadFromString; start at the 'styles' rule
            var walker = new ParseTreeWalker();
            var listener = new StyleListener(this);
            walker.Walk(listener, tree); // Walk the tree with the listener

            return this;
        }

        public void ApplyTo(Element root) {
            // apply defined styles
            foreach (Style style in Inner.Keys) {
                foreach (Element element in root[style.Selector]) {
                    element.Style.MergeWith(style);
                }
            }

            this.ApplyParentStyles(root);

            // apply default styles
            TreeWalker<Element>.Walk(root, ele => {
                ele.Style.MergeWith(Style.Default);
            });
        }

        private void ApplyParentStyles(Element root) {
            Queue<Element> queue = new Queue<Element>();
            queue.Enqueue(root);

            while (queue.Count > 0) {
                Element parent = queue.Dequeue();
                foreach (Element child in parent.Children) {
                    Style.MergeInheritedStyles(child.Style, parent.Style);
                    queue.Enqueue(child);
                }
            }
        }

        public IEnumerable<Style> AsEnumerable() {
            return Inner.Keys;
        }

        public IEnumerator<Style> GetEnumerator() {
            return Inner.Keys.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return Inner.Keys.GetEnumerator();
        }

        public int Count => this.Inner.Keys.Count;
    }
}
