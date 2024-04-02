namespace Leagueinator.Printer.Utility {
    public static class RectExt {

        public static PointF BottomLeft(this RectangleF rect) {
            return new PointF(rect.Left, rect.Bottom);
        }

        public static PointF BottomRight(this RectangleF rect) {
            return new PointF(rect.Right, rect.Bottom);
        }

        public static PointF TopRight(this RectangleF rect) {
            return new PointF(rect.Right, rect.Top);
        }

        public static PointF TopLeft(this RectangleF rect) {
            return new PointF(rect.Left, rect.Top);
        }

        public static PointF Translate(this PointF target, PointF source) {
            return new(target.X + source.X, target.Y + source.Y);
        }

        public static PointF Translate(this PointF target, float x, float y) {
            return target.Translate(new PointF(x, y));
        }

        public static PointF Scale(this PointF left, float amount) {
            return new(left.X * amount, left.Y * amount);
        }

        public static PointF Scale(this PointF left, PointF amount) {
            return new(left.X * amount.X, left.Y * amount.Y);
        }

        public static RectangleF Translate(this RectangleF rect, PointF point) {
            return new(rect.X + point.X, rect.Y + point.Y, rect.Width, rect.Height );
        }

        public static RectangleF Translate(this RectangleF rect, float x, float y) {
            return rect.Translate(new PointF(x, y));
        }
    }
}
