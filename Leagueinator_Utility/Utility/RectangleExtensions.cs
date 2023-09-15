using System.Drawing;

namespace Leagueinator.Utility {
    public static class RectangleExtensions {
        public static Rectangle[] SplitHorz(this Rectangle rect, params int[] percents) {
            var rectangles = new Rectangle[percents.Length];

            int left = rect.Left;

            for (int i = 0; i < rectangles.Length; i++) {
                int width = rect.Width * percents[i] / 100;
                rectangles[i] = new Rectangle(left, rect.Top, width, rect.Height);
                left += width;
            }

            return rectangles;
        }

        public static Rectangle[] SplitHorz(this Rectangle rect, int count) {
            var rectangles = new Rectangle[count];

            int left = rect.Left;

            for (int i = 0; i < rectangles.Length; i++) {
                int width = rect.Width / count;
                rectangles[i] = new Rectangle(left, rect.Top, width, rect.Height);
                left += width;
            }

            return rectangles;
        }

        public static Rectangle[] SplitVert(this Rectangle rect, params int[] percents) {
            var rectangles = new Rectangle[percents.Length];

            int top = rect.Top;

            for (int i = 0; i < rectangles.Length; i++) {
                int height = rect.Height * percents[i] / 100;
                rectangles[i] = new Rectangle(rect.Left, top, rect.Width, height);
                top += height;
            }

            return rectangles;
        }

        public static Rectangle[] SplitVert(this Rectangle rect, int count) {
            var rectangles = new Rectangle[count];

            int top = rect.Top;

            for (int i = 0; i < rectangles.Length; i++) {
                int height = rect.Height / count;
                rectangles[i] = new Rectangle(rect.Left, top, rect.Width, height);
                top += height;
            }

            return rectangles;
        }

        /// <summary>
        /// Create a new rectangle that encompases source all rectangles.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Rectangle Merge(params Rectangle[] source) {
            int top = int.MaxValue; int left = int.MaxValue; int right = 0; int bottom = 0;

            foreach (Rectangle r in source) {
                if (r.Top < top) top = r.Top;
                if (r.Left < left) left = r.Left;
                if (r.Right > right) right = r.Right;
                if (r.Bottom > bottom) bottom = r.Bottom;
            }

            return new Rectangle(top, left, right - left, bottom - top);
        }

        /// <summary>
        /// Reduce each dimension by amount.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static Rectangle[] Shrink(this Rectangle[] source, int amount) {
            var rectangles = new Rectangle[source.Length];

            for (int i = 0; i < rectangles.Length; i++) {
                Rectangle r = source[i];
                rectangles[i] = new Rectangle(
                    r.Left + (amount / 2),
                    r.Top + (amount / 2),
                    r.Width - amount,
                    r.Height - amount
                );
            }
            return rectangles;
        }

        public static Rectangle Shrink(this Rectangle source, int amount) {
            return new Rectangle(
                source.Left + (amount / 2),
                source.Top + (amount / 2),
                source.Width - amount,
                source.Height - amount
            );
        }

        public static Rectangle Below(this Rectangle source, int height) {
            return new Rectangle(
                source.Left,
                source.Top + source.Height,
                source.Width,
                height
            );
        }

        public static Rectangle MoveDown(this Rectangle source, int amount) {
            return new Rectangle(
                source.Left,
                source.Top + amount,
                source.Width,
                source.Height
            );
        }
    }
}
