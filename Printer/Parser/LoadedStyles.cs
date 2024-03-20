using Leagueinator.Printer;
using System.Diagnostics;
using System.Reflection;

namespace Leagueinator.CSSParser {
    /// <summary>
    /// A collection of styles with the query string as the key.
    /// </summary>
    public class LoadedStyles : Dictionary<string, Style> {

        public void ApplyTo(Element root) {
            foreach (string query in this.Keys) {
                foreach (Element element in root[query]) {
                    Style.MergeStyles(element.Style, this[query]);
                }
            }

            this.ApplyParentStyles(root);

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
