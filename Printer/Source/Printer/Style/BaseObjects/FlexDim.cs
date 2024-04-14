
namespace Leagueinator.Printer.Styles {

    public struct FlexDim(float width, float height) {
        public float Width { get; set; } = width;
        public float Height { get; set; } = height;

        public static explicit operator SizeF(FlexDim fd) => new(fd.Width, fd.Height);

        public float this[Dim dim] {
            readonly get {
                return dim == Dim.WIDTH ? this.Width : this.Height;
            }
            set {
                if (dim == Dim.WIDTH) this.Width = value;
                else this.Height = value;
            }
        }

        public FlexDim(SizeF that) : this(that.Width, that.Height) { }
    }
}
