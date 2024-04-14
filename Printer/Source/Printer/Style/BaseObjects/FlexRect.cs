
namespace Leagueinator.Printer.Styles {
    public struct FlexRect(float x, float y, float width, float height) {
        public float Width { get; set; } = width;
        public float Height { get; set; } = height;

        public float X { get; set; } = x;

        public float Y { get; set; } = y;

        public readonly float Left => x;

        public readonly float Top => y;

        public readonly float Right => X + Width;

        public readonly float Bottom => Y + Height;

        public readonly PointF TopLeft => new(X, Y);

        public readonly PointF TopRight => new(Right, Top);

        public readonly PointF BottomRight => new(Right, Bottom);

        public readonly PointF BottomLeft => new(Left, Bottom);

        public static explicit operator RectangleF(FlexRect fd) => new(fd.X, fd.Y, fd.Width, fd.Height);

        public float this[Dim dim] {
            readonly get {
                return dim == Dim.WIDTH ? this.Width : this.Height;
            }
            set {
                if (dim == Dim.WIDTH) this.Width = value;
                else this.Height = value;
            }
        }

        public FlexRect(RectangleF that) : this(that.X, that.Y, that.Width, that.Height) { }

        public override string ToString() {
            return ((RectangleF)this).ToString();
        }
    }
}
