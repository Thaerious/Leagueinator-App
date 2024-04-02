using Leagueinator.Printer.Elements;
using Leagueinator.Printer.Styles;
using System.Collections;
using System.Diagnostics;

namespace Leagueinator.CSSParser {
    /// <summary>
    /// A collection of styles with the query string as the key.
    /// </summary>
    public class LoadedStyles: IEnumerable<Style>{
        private readonly SortedList<Style, Style> inner = [];

        public Style this[int index] {
            get => this.inner.Keys[index];
        }

        public void Add(Style style) => inner.Add(style, style);

        public void ApplyTo(Element root) {
            // apply defined styles
            foreach (Style style in inner.Keys) {
                foreach (Element element in root[style.Selector]) {
                    element.Style.MergeWith(style);
                }
            }

            this.ApplyParentStyles(root);

            // apply default styles
            foreach (Element element in root.AsList()) {
                element.Style.MergeWith(Style.Default);
            }
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

        public IList<Style> AsList() {
            return inner.Keys;
        }

        public IEnumerable<Style> AsEnumerable() {
            return inner.Keys;
        }

        public IEnumerator<Style> GetEnumerator() {
            return inner.Keys.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return inner.Keys.GetEnumerator();
        }

        public int Count => this.inner.Keys.Count;
    }
}
