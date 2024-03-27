using Leagueinator.Printer.Aspects;
using Leagueinator.Printer.Utility;

namespace Leagueinator.Printer.Elements {
    public partial class Element {
        /// <summary>
        /// The (x,y) translation of this element relative to it's _parent element.
        /// </summary>
        internal PointF Translation { get; set; } = new();

        /// <summary>
        /// The (x,y) translation of this element to account for paging.
        /// </summary>
        internal PointF PageOffset { get; set; } = new();

        /// <summary>
        /// The rectable the border will be printed in.
        /// </summary>
        internal virtual SizeF BorderSize { get; set; } = new();

        ///// <summary>
        ///// The rectangle _parent elements will use for child size.
        ///// </summary>
        //internal virtual SizeF OuterSize { get; set; } = new();


        /// <summary>
        /// The element's parent content rectangle.
        /// Setting this property overrides the getter and returns the set value.
        /// Typically the rectangle is set on the root element, and not set on child elements.
        /// </summary>
        public RectangleF ContainerRect {
            get {
                if (this.Parent is null && this._containerRect is null) {
                    throw new NullReferenceException($"On element {this.Identifier} neither parent nor container is set.");
                }

                if (this._containerRect is not null) return (RectangleF)this._containerRect;
                return this.Parent!.ContentRect;
            }
            internal set => this._containerRect = value;
        }

        private RectangleF? _containerRect = null;

        internal void Translate(float x, float y) {
            this.Translate(new PointF(x, y));
        }

        internal void Translate(PointF p) {
            this.ContentRect = this.ContentRect.Translate(p);
            this.OuterRect = this.OuterRect.Translate(p);
        }

        /// <summary>
        /// The area on the screen where drawing takes place for this element.
        /// This is the innermost rectangle.
        /// </summary>
        internal RectangleF ContentRect {
            get; set;
        }

        internal void SetContentRect(float width, float height) {
            this.SetContentRect(width, height, ContentRect.TopLeft());
        }

        internal void SetContentRect(float width, float height, PointF location) {
            Cardinal<UnitFloat> margin = this.Style.Margin ?? new();
            Cardinal<UnitFloat> border = this.Style.BorderSize ?? new();
            Cardinal<UnitFloat> padding = this.Style.Padding ?? new();

            this.ContentRect = new(
                location.X + margin.Left + border.Left + padding.Left,
                location.Y + margin.Top + border.Top + padding.Top,
                width,
                height
            );
        }

        /// <summary>
        /// The entire occupied space of this element, including padding and border.
        /// </summary>
        internal RectangleF OuterRect {
            get; set;
        }

        /// <summary>
        /// The entire occupied space of this child, including padding and border.
        /// </summary>
        public RectangleF BorderRect {
            get {
                Cardinal<UnitFloat> margin = this.Style.Margin ?? new();

                return new RectangleF(
                    this.Location.X + margin.Left,
                    this.Location.Y + margin.Top,
                    this.BorderSize.Width,
                    this.BorderSize.Height
                );
            }
        }

        /// <summary>
        /// The top-left screen location of this element.
        /// </summary>
        public virtual PointF Location => new(
            this.ContainerRect.X + this.Translation.X + this.PageOffset.X,
            this.ContainerRect.Y + this.Translation.Y + this.PageOffset.Y
        );
    }
}
