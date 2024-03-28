
using Leagueinator.Printer.Aspects;
using Leagueinator.Printer.Styles;
using System.Collections.ObjectModel;

namespace Leagueinator.Printer.Elements {


    public partial class Element {

        public Flex Style { 
            get; 
            private set; 
        }

        public bool Invalid { 
            get {
                if (this.IsRoot) return this._invalid;
                else return this.Root.Invalid;
            }
            set {
                if (this.IsRoot) this._invalid = value;
                else this.Root.Invalid = value;   
            }
        }

        public ReadOnlyCollection<Element> Children;

        public Attributes Attributes { get; init; }

        public Element Root {
            get {
                Element current = this;
                while (current.Parent != null) {
                    current = current.Parent;
                }
                return current;
            }
        }
        /// <summary>
        /// Retrieves a non-reflective list of all elements, starting with this element including all 
        /// descendants recursivly.
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
            
            [Validated] 
            private  set {
                if (this._parent != null) throw new InvalidOperationException("Child element already has a parent");
                _parent = value;
            }
        }

        public bool IsRoot { get => this.Parent == null; }

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
        private Dictionary<string, string> _attributes = [];
        private bool _invalid = true;
    }
}
