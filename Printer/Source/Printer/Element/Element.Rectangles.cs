namespace Leagueinator.Printer.Elements {
    public partial class Element {
        /// <summary>
        /// The (x,y) translation of this element relative to it's _parent element.
        /// </summary>
        public PointF Translation { get; set; } = new();

        /// <summary>
        /// The (x,y) translation of this element to account for paging.
        /// </summary>
        public PointF PageOffset { get; set; } = new();

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
        /// The rectangle _parent elements will use for child size.
        /// </summary>
        internal virtual SizeF OuterSize { get; set; } = new();


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
            set => this._containerRect = value;
        }

        private RectangleF? _containerRect = null;

        public void Translate(float x, float y) {
            this.Translate(new PointF(x, y));
        }

        public void Translate(PointF p) {
            this.Translation = new PointF(this.Translation.X + p.X, this.Translation.Y + p.Y);
        }

        /// <summary>
        /// The area on the screen where drawing takes place for this element.
        /// This is the innermost rectangle.
        /// </summary>
        public RectangleF ContentRect {
            get {
                Cardinal<UnitFloat> margin = this.Style.Margin ?? new();
                Cardinal<UnitFloat> border = this.Style.BorderSize ?? new();
                Cardinal<UnitFloat> padding = this.Style.Padding ?? new();

                return new RectangleF(
                    this.Location.X + margin.Left + border.Left + padding.Left,
                    this.Location.Y + margin.Top + border.Top + padding.Top,
                    this.ContentSize.Width,
                    this.ContentSize.Height
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
