using Leagueinator.Printer;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Reflection;

namespace Leagueinator.CSSParser {
    /// <summary>
    /// A collection of styles with the query string as the key.
    /// </summary>
    public class LoadedStyles : SortedList<Style, Style> {

        public void Add(Style style) => base.Add(style, style);

        public void ApplyTo(Element root) {
            var sortedKeys = this
                .OrderBy(pair => pair.Value)
                .Select(pair => pair.Key)
                .ToList();


            // apply defined styles
            foreach (Style style in this.Keys) {
                foreach (Element element in root[style.Selector]) {
                    Style.MergeStyles(element.Style, style);
                }
            }

            this.ApplyParentStyles(root);

            // apply default styles
            foreach (Element element in root["*"]) {
                Style.MergeStyles(element.Style, Style.Default);
            }
        }

        private void ApplyParentStyles(Element root) {
            Queue<Element> queue = new Queue<Element>();
            queue.Enqueue(root);

            while (queue.Count > 0) {
                Element parent = queue.Dequeue();
                foreach (Element child in parent.Children) {
                    Style.MergeStyles(child.Style, parent.Style, true);
                    queue.Enqueue(child);
                }
            }
        }
    }
}
