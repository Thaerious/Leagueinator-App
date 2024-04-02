using Leagueinator.Printer.Aspects;
using Leagueinator.Printer.Utility;

namespace Leagueinator.Printer.Elements {
    public partial class Element : TreeNode<Element> {
        /// <summary>
        /// The (x,y) translation of this element to account for paging.
        /// </summary>
        internal PointF PageOffset { get; set; } = new();

        /// <summary>
        /// The element's parent content rectangle.
        /// Setting this property overrides the getter and returns the set Value.
        /// Typically the rectangle is set on the root element, and not set on child elements.
        /// </summary>
        public RectangleF ContainerRect {
            get {
                if (this.Parent is null ) {
                    return new(0, 0, this.Style.Width, this.Style.Height);
                }

                return this.Parent!.Style.ContentBox();
            }
        }

        /// <summary>
        /// The area on the screen where drawing takes place for this element.
        /// This is the innermost rectangle.
        /// </summary>
        internal RectangleF ContentRect {
            get => this.Style.ContentBox();
        }

        /// <summary>
        /// The entire occupied space of this element, including padding and border.
        /// </summary>
        internal RectangleF OuterRect {
            get => this.Style.OuterBox();
        }
    }
}
