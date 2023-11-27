using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Printer.Printer {
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

        public static PointF Translate(this PointF left, PointF right) {
            return new(left.X + right.X, left.Y + right.Y);
        }

        public static PointF Translate(this PointF left, float x, float y) {
            return left.Translate(new PointF(x, y));
        }

        public static PointF Scale(this PointF left, float amount) {
            return new(left.X * amount, left.Y * amount);
        }

        public static PointF Scale(this PointF left, PointF amount) {
            return new(left.X * amount.X, left.Y * amount.Y);
        }
    }
}
