using Leagueinator.Printer.Aspects;
using Leagueinator.Printer.Styles;
using Leagueinator.Utility;
using System.Diagnostics;
using System.Text;
using System.Xml.Linq;

namespace Leagueinator.Printer.Elements {

    public partial class Element {
        public delegate void DrawDelegate(Graphics g, Element element, int page);

        /// <summary>
        /// Draw (paint) handlers.  Each draw delegate get's called when the draw method is called.
        /// Adding the same delegate multiple times is ignored.
        /// </summary>
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

        public virtual void Draw(Graphics g, int page) {
            Debug.WriteLine("Element.Draw");
            Debug.WriteLine(this.Root.ToXML(
                (ele, xml) => {
                    xml.AppendLine($"border size {ele.Style.BorderSize}");
                }                
            ));

            if (this.Invalid == true) this.Style.DoLayout();
            this.Invalid = false;

            Stack<Element> stack = [];
            stack.Push(this);

            while (stack.Count > 0) {
                Element current = stack.Pop();
                current._onDraw.Invoke(g, current, page);

                if (current.Style.Overflow == Styles.Enums.Overflow.Visible) {
                    foreach (Element child in current.Children) stack.Push(child);
                }
                else if (current.Style.Overflow == Styles.Enums.Overflow.Paged) {
                    foreach (Element child in current.Children) {
                        if (child.Style.Position == Styles.Enums.Position.Absolute) stack.Push(child);
                        else if (child.Style.Position == Styles.Enums.Position.Fixed) stack.Push(child);
                        else if (child.Style.Page == page) stack.Push(child);
                        else if (child.TagName == TextElement.TAG_NAME) stack.Push(child);
                    }
                }
            }
        }

        /// <summary>
        /// Create a new current with a default name and classlist.
        /// </summary>
        public Element(string name, IEnumerable<XAttribute> attributes) : base() {
            this.TagName = name;
            this.Style = new Flex(this);
            this.Style.Owner = this;
            this.Attributes = new(this);
            this.Children = new(this._children);

            foreach (XAttribute xattr in attributes) {
                this.Attributes[xattr.Name.ToString()] = xattr.Value;
            }
        }

        public string? InnerText {
            get {
                StringBuilder sb = new();

                new ElementQueue(this).Walk(current => {
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
        /// Add a single child child to this.
        /// If the child current already has a _parent an exception will be thrown.
        /// be updated to this child.
        /// </summary>
        /// <param name="child"></param>
        /// <returns></returns>
        [Validated]
        public void AddChild(Element child) {
            if (child.Parent is not null) throw new InvalidOperationException("Child element already has a parent");
            this._children.Add(child);
            child.Parent = this;
            this.InvalidateQueryEngine();
        }

        /// <summary>
        /// Remove all child elements from this child.
        /// </summary>
        [Validated]
        public void ClearChildren() {
            foreach (var child in new List<Element>(this.Children)) {
                this._children.Remove(child);
                child.Parent = null;
            }

            this.InvalidateQueryEngine();
        }

        /// <summary>
        /// Remove a child current from this current.
        /// </summary>
        /// <param name="child"></param>
        /// <exception cref="Exception">If the child does not belong to this _parent.</exception>
        [Validated]
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

        public override string ToString() => this.Identifier;

        private readonly List<Element> _children = new();
    }
}
