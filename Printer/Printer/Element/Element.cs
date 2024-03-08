using Leagueinator.Utility;
using System.Diagnostics;
using System.Text;
using System.Xml.Linq;

namespace Leagueinator.Printer {

    public partial class Element {
        public delegate void DrawDelegate(Graphics g, Element element);

        private DrawDelegate _onDraw = delegate{};
        public event DrawDelegate OnDraw {
            add {
                foreach (Delegate hnd in _onDraw.GetInvocationList()) {
                    if (hnd.Equals(value)) return;
                }

                _onDraw += value;
            }
            remove {
                _onDraw -= value;
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

        /// <summary>
        /// Create a new element with the specified name and classlist.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="classes"></param>
        public Element(string name) {
            this.TagName = name;
            this.Style = new Flex();
        }

        public ElementList this[string query] {
            get {
                return this.Children.QueryAll(query);
            }
        }

        public string? InnerText {
            get {
                if (this.Children.Count == 0) return null;
                Queue<Element> queue = new();
                queue.Enqueue(this);

                StringBuilder sb = new();
                while (queue.Count > 0) {
                    Element current = queue.Dequeue();
                    foreach (var child in current.Children) {
                        if (child is TextElement textElement) {
                            sb.Append(textElement.Text);
                        }
                        if (child is Element element) {
                            queue.Enqueue(element);
                        }
                    }
                }

                return sb.ToString();
            }
            set {
                this.ClearChildren();
                if (value == null) return;
                this.AddChild(new TextElement(value));
            }
        }

        /// <summary>
        /// Draw this element in the graphics object.
        /// Draws occur in the following order:
        /// 1) Invoke all OnDraw event listeners
        /// </summary>
        /// <param name="g"></param>
        public virtual void Draw(Graphics g) {
            this._onDraw.Invoke(g, this);
        }

        /// <summary>
        /// Add a single child child to this.
        /// If the child node already has a parent an exception will be thrown.
        /// be updated to this child.
        /// </summary>
        /// <param name="child"></param>
        /// <returns></returns>
        public void AddChild(Element child) {
            if (child.Parent is not null) throw new InvalidOperationException("Child node already has a parent");
            this._children.Add(child);
            child.Parent = this;
        }

        /// <summary>
        /// Remove all child nodes from this child.
        /// </summary>
        public void ClearChildren() {
            foreach (Element child in this.Children) {
                this.Detach(child);
            }
        }

        /// <summary>
        /// Remove a child child from this.
        /// </summary>
        /// <param name="child"></param>
        /// <exception cref="Exception">If the child does not belong to this parent.</exception>
        public void Detach(Element child) {
            this._children.Remove(child);
            child.Parent = null;
        }

        public override string ToString() {
            return this.Identifier;
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

        private readonly ElementList _children = new();
    }
}
