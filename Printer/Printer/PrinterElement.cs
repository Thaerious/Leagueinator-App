using Leagueinator.Utility;
using System.Drawing;
using System.Text;
using System.Xml.Linq;

namespace Leagueinator.Printer {

    public class PrinterElementList : List<PrinterElement> {

        public PrinterElementList this[string id] => this.QuerySelectorAll(id);

        public PrinterElementList() { }

        public PrinterElementList(IEnumerable<PrinterElement> collection) : base(collection) { }

        public PrinterElementList QuerySelectorAll(string query) {
            PrinterElementList result = new();
            Queue<PrinterElementList> queue = new();
            queue.Enqueue(this);

            while (queue.Count > 0) {
                PrinterElementList current = queue.Dequeue();

                if (query == "*") {
                    result.AddRange(current);
                }
                else if (query.StartsWith(".")) {
                    foreach (PrinterElement element in current) {
                        if (element.ClassList.Contains(query[1..])) result.Add(element);
                    }
                }
                else if (query.StartsWith("#")) {
                    foreach (PrinterElement element in current) {
                        if (element.Attributes.ContainsKey("id")) {
                            if (element.Attributes["id"].Equals(query[1..])) result.Add(element);
                        }
                    }
                }
                else {
                    foreach (PrinterElement element in current) {
                        if (element.Name == query) result.Add(element);
                    }
                }

                foreach (PrinterElement child in current) {
                    queue.Enqueue(child.Children);
                }
            }
            return result;
        }

        public PrinterElement? QuerySelector(string query) {
            Queue<PrinterElementList> queue = new();
            queue.Enqueue(this);

            while (queue.Count > 0) {
                PrinterElementList current = queue.Dequeue();

                if (query.StartsWith(".")) {
                    foreach (PrinterElement element in current) {
                        if (element.ClassList.Contains(query[1..])) return element;
                    }
                }
                if (query.StartsWith("#")) {
                    foreach (PrinterElement element in current) {
                        if (element.Attributes.ContainsKey("id")) {
                            if (element.Attributes["id"].Equals(query[1..])) return element;
                        }
                    }
                }
                else {
                    foreach (PrinterElement element in current) {
                        if (element.Name == query) return element;
                    }
                }

                foreach (PrinterElement child in current) {
                    queue.Enqueue(child.Children);
                }
            }

            return null;
        }
    }

    public class PrinterElement {
        public delegate void DrawDelegate(Graphics g, PrinterElement ele);
        public event DrawDelegate OnDraw = delegate { };
        internal XMLLoader? xmlLoader = null;

        public string Name = "";
        public Style Style = new Flex();
        public PrinterElementList Children => new(this._children);

        private List<string>? _classList = null;
        public List<string> ClassList {
            get {
                if (_classList == null) BuildClassList();
                return _classList!;
            }
        }

        private void BuildClassList() {
            _classList = new();
            if (this.Attributes.TryGetValue("class", out string? value)) {
                this._classList.AddRange(value.Split(" "));
            }
        }

        public Dictionary<string, string> Attributes { get; } = new();

        public PrinterElementList this[string query] {
            get {
                return this.Children.QuerySelectorAll(query);
            }
        }

        public string? InnerText {
            get {
                if (this.Children.Count == 0) return null;
                Queue<PrinterElement> queue = new();
                queue.Enqueue(this);

                StringBuilder sb = new();
                while (queue.Count > 0) {
                    PrinterElement current = queue.Dequeue();
                    foreach (var child in current.Children) {
                        if (child is TextElement textElement) {
                            sb.Append(textElement.Text);
                        }
                        if (child is PrinterElement element) {
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
        /// Set parent child and fallback style.
        /// </summary>
        public PrinterElement? Parent { get; private set; }

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
        public virtual SizeF ContentSize { get; set; } = new();

        /// <summary>
        /// The rectable the border will be printed in.
        /// </summary>
        public virtual SizeF BorderSize { get; set; } = new();

        /// <summary>
        /// The rectangle parent elements will use for child size.
        /// </summary>
        public virtual SizeF OuterSize { get; set; } = new();

        /// <summary>
        /// Area to determine relative location of child elements.
        /// Takes padding and border into consideration.
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
        public virtual PointF Location => this.Parent == null
                    ? this.Translation
                    : new PointF(this.Parent.ContentRect.X + this.Translation.X, this.Parent.ContentRect.Y + this.Translation.Y);

        /// <summary>
        /// Create a new printer child with a default name and classlist.
        /// </summary>
        public PrinterElement() {
            this.Style = new Flex();
            this.Name = $"@child-{this.GetHashCode():X}";
        }

        /// <summary>
        /// Create a new printer child with a default name and classlist.
        /// </summary>
        public PrinterElement(IEnumerable<XAttribute> attributes) : base() {
            foreach (XAttribute xattr in attributes) {
                this.Attributes[xattr.Name.ToString()] = xattr.Value;
            }
        }

        /// <summary>
        /// Create a new printer child with the specified name and classlist.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="classes"></param>
        public PrinterElement(string name, params string[] classes) {
            this.Style = new Flex();
            this.Name = name;
            foreach (string className in classes) {
                this.ClassList.Add(className);
            }
        }

        /// <summary>
        /// Create a new child with the name and currentStyles of this child.
        /// </summary>
        /// <returns></returns>
        public virtual PrinterElement Clone() {
            PrinterElement clone = new() {
                Style = this.Style,
                Name = this.Name
            };

            clone.ClassList.AddRange(this.ClassList);

            foreach (PrinterElement child in this.Children) {
                clone.AddChild(child.Clone());
            }
            return clone;
        }

        /// <summary>
        /// Perform all size and layout opertions for this child's style.
        /// </summary>
        public void Update() {
            this.Style.DoSize(this);
            this.Style.DoLayout(this);
        }

        /// <summary>
        /// Dray this child in the graphsics object.
        /// Draws occur in the following order:
        /// 1) Call the style DoDraw method
        /// 2) Invoke all OnDraw event listeners
        /// 3) Call the Draw method for all child elements
        /// </summary>
        /// <param name="g"></param>
        public virtual void Draw(Graphics g) {
            this.Style.DoDraw(this, g);
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
            foreach (PrinterElement child in children) {
                this.AddChild(child, applyStyle);
            }
            return children;
        }
        /// <summary>
        /// Add a single child child to this.
        /// If the children already have a parent child, the parent child will
        /// be updated to this child.
        /// </summary>
        /// <param name="that"></param>
        /// <returns></returns>
        public PrinterElement AddChild(PrinterElement that, bool applyStyle = true) {
            this._children.Add(that);
            that.Parent = this;
            if (applyStyle) this.xmlLoader?.ApplyStyles(that);
            return that;
        }

        /// <summary>
        /// Remove all child nodes from this child.
        /// </summary>
        public void ClearChildren() {
            foreach (PrinterElement child in this.Children) {
                this.RemoveChild(child);
            }
        }

        /// <summary>
        /// Remove a child child from this.
        /// </summary>
        /// <param name="child"></param>
        /// <exception cref="Exception">If the child does not belong to this parent.</exception>
        public void RemoveChild(PrinterElement child) {
            if (child.Parent != this) throw new Exception("Attempt to remove child from non-parent");
            this._children.Remove(child);
            child.Parent = null;
        }

        public void Translate(float x, float y) {
            this.Translate(new PointF(x, y));
        }

        public void Translate(PointF p) {
            this.Translation = new PointF(this.Translation.X + p.X, this.Translation.Y + p.Y);
        }

        public override string ToString() {
            return $"[\"{this.Name}\", {{{this.ClassList.DelString(".")}}}, {this.OuterRect}, {this.Children.Count}]";
        }

        public virtual XMLStringBuilder ToXML() {
            XMLStringBuilder xml = new();

            xml.OpenTag(this.Name);

            foreach (string attr in this.Attributes.Keys) {
                xml.Attribute(attr, this.Attributes[attr]);
            }

            foreach (PrinterElement child in this.Children) {
                xml.AppendXML(child.ToXML());
            }
            xml.CloseTag();

            return xml;
        }

        private readonly PrinterElementList _children = new();
    }
}
