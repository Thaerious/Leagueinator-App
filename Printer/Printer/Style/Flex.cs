using Printer.Printer;
using System.Diagnostics;
using System.Drawing;

namespace Leagueinator.Printer {
    public class Flex : Style {

        public Flex() { }

        public override void DoSize(Element element) {
            this.SetDefaultSize(element);

            // reset parent (this) width.
            float maxWidth = 0f;
            float maxHeight = 0f;
            float sumWidth = 0f;
            float sumHeight = 0f;

            foreach (Element child in element.Children) {
                child.Style.DoSize(child);
                maxWidth = child.OuterSize.Width > maxWidth ? maxWidth = child.OuterSize.Width : maxWidth;
                maxHeight = child.OuterSize.Height > maxHeight ? maxHeight = child.OuterSize.Height : maxHeight;
                sumWidth += child.OuterSize.Width;
                sumHeight += child.OuterSize.Height;
            }

            float contentWidth = 0f, contentHeight = 0f;

            switch (this.Flex_Major) {
                case Flex_Direction.Default:
                case Flex_Direction.Row:
                    contentWidth = this.Width.HasValue ? this.Width : sumWidth;
                    contentHeight = this.Height.HasValue ? this.Height : maxHeight;
                    break;
                case Flex_Direction.Column:
                    contentWidth = this.Width.HasValue ? this.Width : maxWidth;
                    contentHeight = this.Height.HasValue ? this.Height : sumHeight;
                    break;
            }

            var paddingWidth = contentWidth + this.Padding.Left + this.Padding.Right;
            var paddingHeight = contentHeight + this.Padding.Top + this.Padding.Bottom;

            var borderWidth = paddingWidth + this.BorderSize.Left + this.BorderSize.Right;
            var borderHeight = paddingHeight + this.BorderSize.Top + this.BorderSize.Bottom;

            var outerWidth = borderWidth + this.Margin.Left + this.Margin.Right;
            var outerHeight = borderHeight + this.Margin.Top + this.Margin.Bottom;

            element.OuterSize = new SizeF(outerWidth, outerHeight);
            element.BorderSize = new SizeF(borderWidth, borderHeight);
            element.ContentSize = new SizeF(contentWidth, contentHeight);
        }

        void SetDefaultSize(Element element) {
            this.Width.Factor = element.ContainerRect.Width;
            this.Height.Factor = element.ContainerRect.Height;
            element.ContentSize = new SizeF(this.Width, this.Height);
        }

        public override void DoPos(Element element) {
            var children = this.CollectChildren(element);
            if (children.Count == 0) return;

            this.ResetTranslates(children);
            this.JustifyContent(element, children);
            this.AlignItems(element, children);

            foreach (Element child in element.Children) {
                child.Style.DoPos(child);
            }
        }

        public override void Draw(Element element, Graphics g) {
            this.DoDrawBackground(element, g);
            this.DoDrawBorders(element, g);
        }

        public void DoDrawBackground(Element element, Graphics g) {
            if (this.BackgroundColor != null) {
                g.FillRectangle(new SolidBrush((Color)this.BackgroundColor), element.BorderRect);
            }
        }

        public void DoDrawBorders(Element element, Graphics g) {
            if (this.BorderColor is null) return;

            if (this.BorderColor.Top != default) {
                using Pen pen = new Pen(this.BorderColor.Top);
                pen.Width = this.BorderSize.Top;
                pen.DashStyle = this.BorderStyle.Top;

                g.DrawLine(
                    pen,
                    element.BorderRect.TopLeft().Translate(0, this.BorderSize.Top / 2),
                    element.BorderRect.TopRight().Translate(0, this.BorderSize.Top / 2)
                );
            }
            if (this.BorderColor.Right != default) {
                using Pen pen = new Pen(this.BorderColor.Right);
                pen.Width = this.BorderSize.Right;
                pen.DashStyle = this.BorderStyle.Right;

                g.DrawLine(
                    pen,
                    element.BorderRect.TopRight().Translate(-this.BorderSize.Right / 2, 0),
                    element.BorderRect.BottomRight().Translate(-this.BorderSize.Right / 2, 0)
                );
            }
            if (this.BorderColor.Bottom != default) {
                using Pen pen = new Pen(this.BorderColor.Bottom);
                pen.Width = this.BorderSize.Bottom;
                pen.DashStyle = this.BorderStyle.Bottom;

                g.DrawLine(
                    pen,
                    element.BorderRect.BottomRight().Translate(0, -this.BorderSize.Bottom / 2),
                    element.BorderRect.BottomLeft().Translate(0, -this.BorderSize.Bottom / 2)
                );
            }
            if (this.BorderColor.Left != default) {
                using Pen pen = new Pen(this.BorderColor.Left);
                pen.Width = this.BorderSize.Left;
                pen.DashStyle = this.BorderStyle.Left;

                g.DrawLine(
                    pen,
                    element.BorderRect.BottomLeft().Translate(this.BorderSize.Left / 2, 0),
                    element.BorderRect.TopLeft().Translate(this.BorderSize.Left / 2, 0)
                );
            }
        }

        private void ResetTranslates(List<Element> children) {
            foreach (Element child in children) {
                if (child.Style.Position == Position.Static) {
                    child.Translation = new PointF(0, 0);
                }
                else {
                    child.Translation = child.Style.Location;
                }
            }
        }

        /// <summary>
        /// Collect all child nodes that don't have fixed position.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private List<Element> CollectChildren(Element element) {
            var children = element.Children.Where(c => c.Style.Position != Position.Fixed).ToList();
            if (this.Flex_Major_Direction == Direction.Reverse) children.Reverse();
            return children;
        }

        private void LayoutChildren(List<Element> children, PointF from) {
            PointF vector;
            if (this.Flex_Major == Flex_Direction.Column) {
                vector = new(0, 1);
            }
            else {
                vector = new(1, 0);
            }

            PointF current = from;
            foreach (Element child in children) {
                child.Translate(current);
                var diff = new PointF(child.OuterSize.Width, child.OuterSize.Height).Scale(vector);
                current = current.Translate(diff);
            }
        }

        /// <summary>
        /// Layout all child nodes without taking into account alignment.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="children"></param>
        private void JustifyContent(Element element, List<Element> children) {
            float widthRemaining = element.ContentSize.Width;

            foreach (Element child in children) {
                widthRemaining -= child.OuterSize.Width;
            }


            float heightRemaining = element.ContentSize.Height;
            foreach (Element child in children) {
                heightRemaining -= child.OuterSize.Height;
            }

            if (this.Flex_Major_Direction == Direction.Reverse) children.Reverse();
            int count = children.Count;

            switch (this.Justify_Content) {
                case Justify_Content.Default:
                case Justify_Content.Flex_start: {
                        PointF from;

                        if (this.Flex_Major_Direction == Direction.Forward) {
                            from = new PointF(0, 0);
                        }
                        else if (this.Flex_Major == Flex_Direction.Row) {
                            from = new PointF(widthRemaining, 0);
                        }
                        else {
                            from = new PointF(0, heightRemaining);
                        }

                        LayoutChildren(children, from);
                        break;
                    }

                case Justify_Content.Flex_end: {
                        PointF from;

                        if (this.Flex_Major_Direction == Direction.Reverse) {
                            from = new PointF(0, 0);
                        }
                        else if (this.Flex_Major == Flex_Direction.Row) {
                            from = new PointF(widthRemaining, 0);
                        }
                        else {
                            from = new PointF(0, heightRemaining);
                        }

                        LayoutChildren(children, from);
                        break;
                    }

                case Justify_Content.Center: {
                        PointF from;

                        if (this.Flex_Major == Flex_Direction.Row) {
                            from = new PointF(widthRemaining / 2, 0);
                        }
                        else {
                            from = new PointF(0, heightRemaining / 2);
                        }

                        LayoutChildren(children, from);
                        break;
                    }

                case Justify_Content.Space_evenly: {
                        if (this.Flex_Major == Flex_Direction.Row) {
                            float spaceBetween = widthRemaining / (count + 1);
                            float dx = spaceBetween;

                            foreach (var child in children) {
                                child.Translation = new(dx, child.Translation.Y);
                                dx = dx + spaceBetween + child.OuterRect.Width;
                            }
                        }
                        else {
                            float spaceBetween = heightRemaining / (count + 1);
                            float dy = spaceBetween;

                            foreach (var child in children) {
                                child.Translation = new(child.Translation.X, dy);
                                dy = dy + spaceBetween + child.OuterRect.Height;
                            }
                        }
                        break;
                    }

                case Justify_Content.Space_between: {
                        if (this.Flex_Major == Flex_Direction.Row) {
                            float spaceBetween = widthRemaining / (count - 1);
                            float dx = 0;

                            foreach (var child in children) {
                                child.Translation = new(dx, child.Translation.Y);
                                dx = dx + spaceBetween + child.OuterRect.Width;
                            }
                        }
                        else {
                            float spaceBetween = heightRemaining / (count - 1);
                            float dy = 0;

                            foreach (var child in children) {
                                child.Translation = new(child.Translation.X, dy);
                                dy = dy + spaceBetween + child.OuterRect.Height;
                            }
                        }
                        break;
                    }

                case Justify_Content.Space_around: {
                        if (this.Flex_Major == Flex_Direction.Row) {
                            float spaceAround = widthRemaining / (count * 2);
                            float dx = spaceAround;

                            foreach (var child in children) {
                                child.Translation = new(dx, child.Translation.Y);
                                dx = dx + (2 * spaceAround) + child.OuterRect.Width;
                            }
                        }
                        else {
                            float spaceAround = heightRemaining / (count * 2);
                            float dy = spaceAround;

                            foreach (var child in children) {
                                child.Translation = new(child.Translation.X, dy);
                                dy = dy + (2 * spaceAround) + child.OuterRect.Height;
                            }
                        }
                        break;
                    }
            }
        }
        private void AlignItems(Element element, List<Element> children) {
            switch (this.Flex_Major) {
                case Flex_Direction.Row:
                    switch (this.Align_Items) {
                        case Align_Items.Default:
                        case Align_Items.Flex_start:
                            break;
                        case Align_Items.Flex_end:
                            children.ForEach(c => c.Translate(0, element.ContentRect.Height - c.OuterSize.Height));
                            break;
                        case Align_Items.Center:
                            children.ForEach(c => c.Translate(0, (element.ContentRect.Height / 2) - (c.OuterSize.Height / 2)));
                            break;
                    }
                    break;
                case Flex_Direction.Column:
                    switch (this.Align_Items) {
                        case Align_Items.Default:
                        case Align_Items.Flex_start:
                            break;
                        case Align_Items.Flex_end:
                            children.ForEach(c => c.Translate(element.ContentRect.Width - c.OuterSize.Width, 0));
                            break;
                        case Align_Items.Center:
                            children.ForEach(c => c.Translate((element.ContentRect.Width / 2) - (c.OuterSize.Width / 2), 0));
                            break;
                    }
                    break;
            }
        }
    }
}
