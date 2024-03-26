using Leagueinator.Printer.Styles;
using Leagueinator.Utility;
using System.Text;
using System.Xml.Linq;

namespace Leagueinator.Printer.Elements {

    public partial class Element {
        public delegate void DrawDelegate(Graphics g, Element element, int page);

        private DrawDelegate _onDraw = delegate { };
        public event DrawDelegate OnDraw {
            add {
                foreach (Delegate hnd in this._onDraw.GetInvocationList()) {
                    if (hnd.Equals(value)) return;
                }

                this._onDraw += value;
            }
            remove {
                this._onDraw -= value;
            }
        }

        /// <summary>
        /// Create a new element with a default name and classlist.
        /// </summary>
        public Element(string name, IEnumerable<XAttribute> attributes) : base() {
            this.TagName = name;
            this.Style = new Flex();

            foreach (XAttribute xattr in attributes) {
                this.Attributes[xattr.Name.ToString()] = xattr.Value;
            }
        }

        public string? InnerText {
            get {
                StringBuilder sb = new();

                new ElementQueue(this).Process(current => {
                    if (current is TextElement textElement) {
                        sb.Append(textElement.Text);
                    }
                });

                return sb.ToString();
            }
            set {
                this.ClearChildren();
                if (value == null) return;
                this.AddChild(new TextElement(value));
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="g"></param>
        public virtual void InvokeDrawHandlers(Graphics g, int page) {
            this._onDraw.Invoke(g, this, page);
        }

        /// <summary>
        /// Add a single child child to this.
        /// If the child element already has a _parent an exception will be thrown.
        /// be updated to this child.
        /// </summary>
        /// <param name="child"></param>
        /// <returns></returns>
        public void AddChild(Element child) {
            if (child.Parent is not null) throw new InvalidOperationException("Child element already has a parent");
            this._children.Add(child);
            child.Parent = this;
            this.InvalidateQueryEngine();
        }

        /// <summary>
        /// Remove all child elements from this child.
        /// </summary>
        public void ClearChildren() {
            foreach (var child in new List<Element>(this.Children)) {
                this._children.Remove(child);
                child.Parent = null;
            }

            this.InvalidateQueryEngine();
        }

        /// <summary>
        /// Remove a child element from this element.
        /// </summary>
        /// <param name="child"></param>
        /// <exception cref="Exception">If the child does not belong to this _parent.</exception>
        public void Detach(Element child) {
            this._children.Remove(child);
            child.Parent = null;
            this.InvalidateQueryEngine();
        }

        public virtual XMLStringBuilder ToXML(Action<Element, XMLStringBuilder>? action = null) {
            action ??= (element, xml) => { };
            XMLStringBuilder xml = new();

            xml.OpenTag(this.TagName);
            action(this, xml);

            foreach (string attr in this.Attributes.Keys) {
                xml.Attribute(attr, this.Attributes[attr]);
            }

            foreach (Element child in this.Children) {
                xml.AppendXML(child.ToXML(action));
            }

            xml.CloseTag();

            return xml;
        }

        private readonly List<Element> _children = new();
    }
}
