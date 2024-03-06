using Leagueinator.Utility;
using System.Diagnostics;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace Leagueinator.Printer {

    public class Element : HasContentRect {
        public delegate void DrawDelegate(Graphics g, Element ele); // TODO remove element?
        public event DrawDelegate OnDraw = delegate { };

        public string TagName { get; init; } = "";

        public Style Style { get; internal set; } = new Flex();

        public PrinterElementList Children => new(this._children);

        public Element? Parent { get; internal set; }

        public List<string> ClassList {
            get {
                if (this.Attributes.TryGetValue("class", out string? value)) {
                    return value.Split().ToList();
                }
                return new();
            }
        }

        public Dictionary<string, string> Attributes { get; init; } = new();

        public PrinterElementList this[string query] {
            get {
                return this.Children.QueryAll(query);
            }
        }

        public Element this[int index] {
            get {
                return this.Children[index];
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

        private RectangleF? _containerRect = null;
        public RectangleF ContainerRect {
            get => this._containerRect is null ? this.Parent.ContentRect : (RectangleF)this._containerRect;
            set => this._containerRect = value;
        }

        /// <summary>
        /// The (x,y) translation of this child relative to it's TargetElement.
        /// </summary>
        public PointF Translation {
            get; set;
        } = new();

        /// <summary>
        /// The rectangle print actions will take place in.
        /// This is defined by style width and height.
        /// </summary>
        internal virtual SizeF ContentSize { get; set; } = new();

        /// <summary>
        /// The rectable the border will be printed in.
        /// </summary>
        internal virtual SizeF BorderSize { get; set; } = new();

        /// <summary>
        /// The rectangle parent elements will use for child size.
        /// </summary>
        internal virtual SizeF OuterSize { get; set; } = new();

        /// <summary>
        /// The area on the screen where drawing takes place for this element.
        /// </summary>
        public RectangleF ContentRect {
            get {
                Cardinal<UnitFloat> margin = this.Style.Margin;
                Cardinal<UnitFloat> border = this.Style.BorderSize;
                Cardinal<UnitFloat> padding = this.Style.Padding;

                return new RectangleF(
                    this.Location.X + margin.Left + border.Left + padding.Left,
                    this.Location.Y + margin.Top + border.Top + padding.Top,
                    this.ContentSize.Width,
                    this.ContentSize.Height
                );
            }
        }

        /// <summary>
        /// The entire occupied space of this child, including padding and border.
        /// </summary>
        public RectangleF OuterRect {
            get {
                return new RectangleF(
                    this.Location.X,
                    this.Location.Y,
                    this.OuterSize.Width,
                    this.OuterSize.Height
                );
            }
        }
        /// <summary>
        /// The entire occupied space of this child, including padding and border.
        /// </summary>
        public RectangleF BorderRect {
            get {
                Cardinal<UnitFloat> margin = this.Style.Margin;

                return new RectangleF(
                    this.Location.X + margin.Left,
                    this.Location.Y + margin.Top,
                    this.BorderSize.Width,
                    this.BorderSize.Height
                );
            }
        }

        /// <summary>
        /// The screen location of this child.
        /// </summary>
        public virtual PointF Location => new(this.ContainerRect.X + this.Translation.X, this.ContainerRect.Y + this.Translation.Y);


        /// <summary>
        /// Create a new printer child with a default name and classlist.
        /// </summary>
        public Element(IEnumerable<XAttribute> attributes) : base() {
            foreach (XAttribute xattr in attributes) {
                this.Attributes[xattr.Name.ToString()] = xattr.Value;
            }
        }

        /// <summary>
        /// Create a new printer child with the specified name and classlist.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="classes"></param>
        public Element(string name) {
            this.Style = new Flex();
            this.TagName = name;
        }

        /// <summary>
        /// Create a new child with the name and currentStyles of this child.
        /// </summary>
        /// <returns></returns>
        public virtual Element Clone() {
            Element clone = new(this.TagName) {
                Style = this.Style,
                Attributes = new(this.Attributes)
            };

            clone.ClassList.AddRange(this.ClassList);

            foreach (Element child in this.Children) {
                clone.AddChild(child.Clone());
            }
            return clone;
        }

        /// <summary>
        /// Dray this child in the graphsics object.
        /// Draws occur in the following order:
        /// 1) Call the style Draw method
        /// 2) Invoke all OnDraw event listeners
        /// 3) Call the Draw method for all child elements
        /// </summary>
        /// <param name="g"></param>
        public virtual void Draw(Graphics g) {
            this.Style.Draw(this, g);
            this.OnDraw.Invoke(g, this);
            this.Children.ForEach(child => child.Draw(g));
        }

        /// <summary>
        /// Add multiple children this this child.
        /// If the children already have a parent child, the parent child will
        /// be updated to this child.
        /// </summary>
        /// <param name="children"></param>
        /// <returns></returns>
        public PrinterElementList AddChildren(PrinterElementList children, bool applyStyle = true) {
            foreach (Element child in children) {
                this.AddChild(child, applyStyle);
            }
            return children;
        }
        /// <summary>
        /// Add a single child child to this.
        /// If the children already have a parent child, the parent child will
        /// be updated to this child.
        /// </summary>
        /// <param name="child"></param>
        /// <returns></returns>
        public Element AddChild(Element child, bool applyStyle = true) {
            if (this._children.Contains(child)) return child;
            this._children.Add(child);
            return child;
        }

        /// <summary>
        /// Remove all child nodes from this child.
        /// </summary>
        public void ClearChildren() {
            foreach (Element child in this.Children) {
                this.RemoveChild(child);
            }
        }

        /// <summary>
        /// Remove a child child from this.
        /// </summary>
        /// <param name="child"></param>
        /// <exception cref="Exception">If the child does not belong to this parent.</exception>
        public void RemoveChild(Element child) {
            this._children.Remove(child);
        }

        public void Translate(float x, float y) {
            this.Translate(new PointF(x, y));
        }

        public void Translate(PointF p) {
            this.Translation = new PointF(this.Translation.X + p.X, this.Translation.Y + p.Y);
        }

        public override string ToString() {
            return $"[\"{this.TagName}\", {{{this.ClassList.DelString(".")}}}, {this.OuterRect}, {this.Children.Count}]";
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

        public string DiffHash() {
            string rawData = this.ToXML().ToString();

            using (SHA256 sha256Hash = SHA256.Create()) {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++) {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private readonly PrinterElementList _children = new();
    }
}
