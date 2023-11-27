﻿using Leagueinator.Utility;
using System.Diagnostics;
using System.Drawing;

namespace Leagueinator.Printer {

    public class PrinterElementList : List<PrinterElement> {

        public PrinterElementList this[string id] {
            get {
                return this.QuerySelectorAll(id);
            }
        }

        public PrinterElementList() { }

        public PrinterElementList(IEnumerable<PrinterElement> collection) : base(collection) { }

        public PrinterElementList QuerySelectorAll(string query, PrinterElementList? result = null) {
            result ??= new();

            if (query.StartsWith(".")) {
                foreach (PrinterElement element in this) {
                    var x = element.ClassList.Count > 0 ? element.ClassList[0] : "NULL";
                    if (element.ClassList.Contains(query[1..])) result.Add(element);
                }
            }
            else {
                foreach (PrinterElement element in this) {
                    if (element.Name == query) result.Add(element);
                }
            }

            foreach (PrinterElement element in this) {
                element.Children.QuerySelectorAll(query, result);
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
                var intermediate = element.Children.QuerySelector(query);
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
        public PrinterElement? Parent {
            get => _parent;
            private set => this._parent = value;
        }

        /// <summary>
        /// The (x,y) translation of this element relative to it's TargetElement.
        /// </summary>
        public PointF Translation {
            get; set;
        } = new();

        /// <summary>
        /// Set the drawable area of the element (excl border & padding).
        /// </summary>
        public virtual SizeF InnerSize { get; set; } = new();

        /// <summary>
        /// Set the occupied area of the element (incl border & padding).
        /// </summary>
        public virtual SizeF OuterSize { get; set; } = new();

        /// <summary>
        /// Area to determine relative location of child elements.
        /// Takes padding and border into consideration.
        /// </summary>
        public RectangleF InnerRect {
            get {
                return new RectangleF(
                    this.Location.X + this.Style.Margin.Left + this.Style.BorderSize.Left + this.Style.Padding.Left,
                    this.Location.Y + this.Style.Margin.Top + this.Style.BorderSize.Top + this.Style.Padding.Top,
                    this.InnerSize.Width,
                    this.InnerSize.Height
                );
            }
        }

        /// <summary>
        /// The entire occupied space of this element, including padding and border.
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
        /// The screen location of this element.
        /// </summary>
        public PointF Location => this.Parent == null
                    ? this.Translation
                    : new PointF(this.Parent.InnerRect.X + this.Translation.X, this.Parent.InnerRect.Y + this.Translation.Y);

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
                clone.AddChild(child.Clone());
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
            foreach (var child in children) {
                this.AddChild(child);
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
        /// Remove a child element from this.
        /// </summary>
        /// <param name="child"></param>
        /// <exception cref="Exception">If the child does not belong to this parent.</exception>
        public void RemoveChild(PrinterElement child) {
            if (child.Parent != this) throw new Exception("Attempt to remove child from non-parent");
            this._children.Remove(child);
            child.Parent = null;
        }

        public void Translate(float x, float y) => this.Translate(new PointF(x, y));

        public void Translate(PointF p) {
            this.Translation = new PointF(this.Translation.X + p.X, this.Translation.Y + p.Y);
        }

        public override string ToString() {
            return $"[\"{this.Name}\", {{{this.ClassList.DelString(".")}}}, {this.OuterRect}, {this.Children.Count}]";
        }

        private readonly PrinterElementList _children = new();
        private PrinterElement? _parent;
    }
}
