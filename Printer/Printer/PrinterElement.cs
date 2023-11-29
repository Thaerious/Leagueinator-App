using Leagueinator.Printer;
using Leagueinator.Utility;
using System.Drawing;

namespace Printer.Printer {

    public class PrinterElementList : List<PrinterElement> {

        public PrinterElementList this[string id] => this.QuerySelectorAll(id);

        public PrinterElementList() { }

        public PrinterElementList(IEnumerable<PrinterElement> collection) : base(collection) { }

        public PrinterElementList QuerySelectorAll(string query, PrinterElementList? result = null) {
            result ??= new();

            if (query.StartsWith(".")) {
                foreach (PrinterElement element in this) {
                    _ = element.ClassList.Count > 0 ? element.ClassList[0] : "NULL";
                    if (element.ClassList.Contains(query[1..])) result.Add(element);
                }
            }
            else {
                foreach (PrinterElement element in this) {
                    if (element.Name == query) result.Add(element);
                }
            }

            foreach (PrinterElement element in this) {
                _ = element.Children.QuerySelectorAll(query, result);
            }

            return result;
        }

        public PrinterElement? QuerySelector(string query) {
            if (query.StartsWith(".")) {
                foreach (PrinterElement element in this) {
                    if (element.ClassList.Contains(query[1..])) return element;
                }
            }
            else {
                foreach (PrinterElement element in this) {
                    if (element.Name == query) return element;
                }
            }

            foreach (PrinterElement element in this) {
                PrinterElement? intermediate = element.Children.QuerySelector(query);
                if (intermediate != null) return intermediate;
            }

            return null;
        }
    }

    public class PrinterElement {
        public delegate void DrawDelegate(Graphics g, PrinterElement ele);
        public event DrawDelegate OnDraw = delegate { };

        public string Name = "";
        public Style Style = new Flex();
        public PrinterElementList Children => new(this._children);
        public readonly List<string> ClassList = new();

        /// <summary>
        /// Set parent element and fallback style.
        /// </summary>
        public PrinterElement? Parent { get; private set; }

        /// <summary>
        /// The (x,y) translation of this element relative to it's TargetElement.
        /// </summary>
        public PointF Translation {
            get; set;
        } = new();

        public virtual SizeF ContentSize { get; set; } = new();

        public virtual SizeF BorderSize { get; set; } = new();

        public virtual SizeF OuterSize { get; set; } = new();

        /// <summary>
        /// Area to determine relative location of child elements.
        /// Takes padding and border into consideration.
        /// </summary>
        public RectangleF ContentRect {
            get {
                Cardinal<float> margin = this.Style.Margin;

                return new RectangleF(
                    this.Location.X + margin.Left + this.Style.BorderSize.Left + this.Style.Padding.Left,
                    this.Location.Y + margin.Top + this.Style.BorderSize.Top + this.Style.Padding.Top,
                    this.ContentSize.Width,
                    this.ContentSize.Height
                );
            }
        }

        /// <summary>
        /// The entire occupied space of this element, including padding and border.
        /// </summary>
        public RectangleF OuterRect => new(
                    this.Location.X,
                    this.Location.Y,
                    this.OuterSize.Width,
                    this.OuterSize.Height
                );

        /// <summary>
        /// The entire occupied space of this element, including padding and border.
        /// </summary>
        public RectangleF BorderRect {
            get {
                Cardinal<float> margin = new();

                return new RectangleF(
                    this.Location.X + margin.Left,
                    this.Location.Y + margin.Top,
                    this.BorderSize.Width + this.Style.Padding.Left + this.Style.Padding.Right,
                    this.BorderSize.Height + this.Style.Padding.Top + this.Style.Padding.Bottom
                );
            }
        }

        /// <summary>
        /// The screen location of this element.
        /// </summary>
        public virtual PointF Location => this.Parent == null
                    ? this.Translation
                    : new PointF(this.Parent.ContentRect.X + this.Translation.X, this.Parent.ContentRect.Y + this.Translation.Y);

        /// <summary>
        /// Create a new printer element with a default name and classlist.
        /// </summary>
        public PrinterElement() {
            this.Style = new Flex();
            this.Name = this.GetHashCode().ToString("X");
        }

        /// <summary>
        /// Create a new printer element with the specified name and classlist.
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
        /// Create a new element with the name and styles of this element.
        /// </summary>
        /// <returns></returns>
        public virtual PrinterElement Clone() {
            PrinterElement clone = new() {
                Style = this.Style,
                Name = this.Name
            };

            clone.ClassList.AddRange(this.ClassList);

            foreach (PrinterElement child in this.Children) {
                _ = clone.AddChild(child.Clone());
            }
            return clone;
        }

        /// <summary>
        /// Perform all size and layout opertions for this element's style.
        /// </summary>
        public void Update() {
            this.Style.DoSize(this);
            this.Style.DoLayout(this);
        }

        /// <summary>
        /// Dray this element in the graphsics object.
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
        /// Add multiple children this this element.
        /// If the children already have a parent element, the parent element will
        /// be updated to this element.
        /// </summary>
        /// <param name="children"></param>
        /// <returns></returns>
        public PrinterElementList AddChildren(PrinterElementList children) {
            foreach (PrinterElement child in children) {
                _ = this.AddChild(child);
            }
            return children;
        }
        /// <summary>
        /// Add a single child element to this.
        /// If the children already have a parent element, the parent element will
        /// be updated to this element.
        /// </summary>
        /// <param name="that"></param>
        /// <returns></returns>
        public PrinterElement AddChild(PrinterElement that) {
            this._children.Add(that);
            that.Parent = this;
            return that;
        }

        /// <summary>
        /// Remove all child nodes from this element.
        /// </summary>
        public void ClearChildren() {
            foreach (PrinterElement child in this.Children) {
                this.RemoveChild(child);
            }
        }

        /// <summary>
        /// Remove a child element from this.
        /// </summary>
        /// <param name="child"></param>
        /// <exception cref="Exception">If the child does not belong to this parent.</exception>
        public void RemoveChild(PrinterElement child) {
            if (child.Parent != this) throw new Exception("Attempt to remove child from non-parent");
            _ = this._children.Remove(child);
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

            _ = xml.OpenTag(this.Name);
            foreach (PrinterElement child in this.Children) {
                _ = xml.AppendXML(child.ToXML());
            }
            _ = xml.CloseTag();

            return xml;
        }

        private readonly PrinterElementList _children = new();
    }
}
