
using System.Collections.ObjectModel;

namespace Leagueinator.Printer.Elements {
    public partial class Element {

        public Styles.Style Style { get; internal set; }

        public ReadOnlyCollection<Element> Children => new(this._children);

        /// <summary>
        /// Retrieves a list of all elements, starting with this element including all descendants recursivly.
        /// </summary>
        /// <remarks>
        /// This method performs a depth-first search to collect all elements in the hierarchy, starting with 
        /// the current element as the root. The result includes the current element followed by its descendants,
        /// where each child's descendants are added before moving to the next sibling.
        /// </remarks>
        /// <returns>
        /// A <see cref="List{Element}"/> containing the current element followed by all descendant 
        /// elements in the order they were encountered.
        /// </returns>
        public List<Element> AllDecendents() {
            List<Element> allElements = [];
            allElements.Add(this);

            foreach (Element child in this.Children) {
                allElements.AddRange(child.AllDecendents());
            }

            return allElements;
        }

        public Element? Parent {
            get => _parent;
            private set {
                if (this._parent != null) throw new InvalidOperationException("Child element already has a parent");
                _parent = value;
            }
        }

        public bool IsRoot { get => this.Parent == null; }

        public Dictionary<string, string> Attributes { get; init; } = new();

        public string TagName { get; init; } = "";

        public string Identifier {
            get {
                if (this.Attributes.TryGetValue("id", out string? value)) {
                    return this.TagName + "#" + value;
                }
                else {
                    return this.TagName;
                }
            }
        }

        public List<string> ClassList {
            get {
                if (this.Attributes.TryGetValue("class", out string? value)) {
                    return [.. value.Split()];
                }
                return [];
            }
        }

        private Element? _parent;
    }
}
