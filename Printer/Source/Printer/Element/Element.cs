using Leagueinator.Printer.Aspects;
using Leagueinator.Printer.Styles;
using Leagueinator.Utility;
using System.Diagnostics;
using System.Text;
using System.Xml.Linq;

namespace Leagueinator.Printer.Elements {

    public partial class Element : TreeNode<Element>{
        public delegate void DrawDelegate(Graphics g, Element element, int page);

        /// <summary>
        /// Create a new current with a default name and classlist.
        /// </summary>
        public Element(string name, IEnumerable<XAttribute> attributes) : base() {
            this.TagName = name;
            this.Style = new(this) {
                Owner = this
            };
            this.Attributes = new(this);

            foreach (XAttribute xattr in attributes) {
                this.Attributes[xattr.Name.ToString()] = xattr.Value;
            }
        }

        public string? InnerText {
            get {
                StringBuilder sb = new();

                new TreeWalker<Element>(this).Walk(current => {
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
        /// Remove all child elements from this child.
        /// </summary>
        public override void ClearChildren() {
            base.ClearChildren();
            this.InvalidateQueryEngine();
        }

        /// <summary>
        /// Remove a child current from this current.
        /// </summary>
        /// <param name="child"></param>
        /// <exception cref="Exception">If the child does not belong to this _parent.</exception>
        [Validated]
        public override void Detach(Element child) {
            base.Detach(child);
            this.InvalidateQueryEngine();
        }

        public override XMLStringBuilder ToXML(Action<Element, XMLStringBuilder>? action = null) {
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
    }
}
