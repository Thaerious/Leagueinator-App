using Printer.Printer;
using System.Drawing;

namespace Leagueinator.Printer {
    public class Flex : Style {

        public Flex() : base("") { }

        public Flex(string Selector) : base(Selector){
            this.Selector = Selector;
        }

        public override void DoSize(PrinterElement element) {
            float maxWidth = 0f;
            float maxHeight = 0f;
            float sumWidth = 0f;
            float sumHeight = 0f;

            foreach (PrinterElement child in element.Children) {
                child.Style.DoSize(child);
                maxWidth = child.InnerSize.Width > maxWidth ? maxWidth = child.InnerSize.Width : maxWidth;
                maxHeight = child.InnerSize.Height > maxHeight ? maxHeight = child.InnerSize.Height : maxHeight;
                sumWidth += child.InnerSize.Width;
                sumHeight += child.InnerSize.Height;
            }

            float width = 0f, height = 0f;
            switch (this.Flex_Major) {
                case Flex_Direction.Row:
                    width = (float)(this.Width ?? sumWidth);
                    height = (float)(this.Height ?? maxHeight);
                    break;
                case Flex_Direction.Column:
                    width = (float)(this.Width ?? maxWidth);
                    height = (float)(this.Height ?? sumHeight);
                    break;
            }

            element.OuterSize = new SizeF(width, height);
            width -= this.Padding.Left + this.Padding.Right + this.BorderSize.Left + this.BorderSize.Right;
            height -= this.Padding.Top + this.Padding.Bottom +  this.BorderSize.Top + this.BorderSize.Bottom;
            element.InnerSize = new SizeF(width, height);    
        }

        public override void DoLayout(PrinterElement element) {
            var children = this.CollectChildren(element);

            if (children.Count == 0) return;
            this.ResetTranslates(children);
            this.JustifyContent(element, children);
            this.AlignItems(element, children);

            foreach (PrinterElement child in element.Children) child.Style.DoLayout(child);
        }

        public override void DoDraw(PrinterElement element, Graphics g) {
            if (this.BackgroundColor != null) {
                g.FillRectangle(new SolidBrush((Color)this.BackgroundColor), element.InnerRect);
            }

            var left = Margin.Left + BorderSize.Left / 2;
            var right = -Margin.Right - BorderSize.Right / 2;
            var top = Margin.Top + BorderSize.Top / 2;
            var bottom = -Margin.Bottom - BorderSize.Top / 2;

            if (this.BorderColor.Left != default) {
                var p1 = new PointF(left, Margin.Top);
                var p2 = new PointF(left, -Margin.Bottom);

                g.DrawLine(
                    new Pen(BorderColor.Left, BorderSize.Left),
                    element.OuterRect.TopLeft().Translate(p1),
                    element.OuterRect.BottomLeft().Translate(p2)
                );
            }
            if (this.BorderColor.Right != default) {
                var p1 = new PointF(right, Margin.Top);
                var p2 = new PointF(right, -Margin.Bottom);

                g.DrawLine(
                    new Pen(BorderColor.Right, BorderSize.Right),
                    element.OuterRect.TopRight().Translate(p1),
                    element.OuterRect.BottomRight().Translate(p2)
                );
            }
            if (this.BorderColor.Top != default) {
                var p1 = new PointF(Margin.Left, top);
                var p2 = new PointF(-Margin.Right, top);

                g.DrawLine(
                    new Pen(BorderColor.Top, BorderSize.Top),
                    element.OuterRect.TopLeft().Translate(p1),
                    element.OuterRect.TopRight().Translate(p2)
                );
            }
            if (this.BorderColor.Bottom != default) {
                var p1 = new PointF(Margin.Left, bottom);
                var p2 = new PointF(-Margin.Right, bottom);

                g.DrawLine(
                    new Pen(BorderColor.Bottom, BorderSize.Bottom),
                    element.OuterRect.BottomLeft().Translate(p1),
                    element.OuterRect.BottomRight().Translate(p2)
                );
            }

            foreach (PrinterElement child in element.Children) child.Style.DoDraw(child, g);
        }

        private void ResetTranslates(List<PrinterElement> children) {
            foreach (PrinterElement child in children) {
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
        private List<PrinterElement> CollectChildren(PrinterElement element) {
            var children = element.Children.Where(c => c.Style.Position != Position.Fixed).ToList();
            if (this.Flex_Major_Direction == Direction.Reverse) children.Reverse();            
            return children;
        }

        private void LayoutChildren(List<PrinterElement> children, PointF from) {
            PointF vector;
            if (this.Flex_Major == Flex_Direction.Column) {
                vector = new(0, 1);
            }
            else {
                vector = new(1, 0);
            }

            PointF current = from;
            foreach (PrinterElement child in children) {
                child.Translate(current);
                var diff = new PointF(child.OuterSize.Width, child.OuterSize.Height).Scale(vector);
                current = current.Translate(diff);
            }
        }

        private void JustifyContent(PrinterElement element, List<PrinterElement> children) {
            float widthRemaining = element.InnerSize.Width;
            foreach (PrinterElement child in children) {
                widthRemaining -= child.OuterSize.Width;
            }

            float heightRemaining = element.InnerSize.Height;
            foreach (PrinterElement child in children) {
                heightRemaining -= child.OuterSize.Height;
            }

            if (this.Flex_Major_Direction == Direction.Reverse) children.Reverse();
            int count = children.Count;

            switch (this.Justify_Content) {
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
                                dy = dy + spaceBetween + child.OuterRect.Width;
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
                                dy = dy + spaceBetween + child.OuterRect.Width;
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
                                dy = dy + (2 * spaceAround) + child.OuterRect.Width;
                            }
                        }
                        break;
                    }
            }
        }
        private void AlignItems(PrinterElement element, List<PrinterElement> children) {
            switch (this.Flex_Major) {
                case Flex_Direction.Row:
                    switch (this.Align_Items) {
                        case Align_Items.Flex_start:
                            break;
                        case Align_Items.Flex_end:
                            children.ForEach(c => c.Translate(0, element.InnerRect.Height - c.OuterSize.Height));
                            break;
                        case Align_Items.Center:
                            children.ForEach(c => c.Translate(0, (element.InnerRect.Height / 2) - (c.OuterSize.Height / 2)));
                            break;
                    }
                    break;
                case Flex_Direction.Column:
                    switch (this.Align_Items) {
                        case Align_Items.Flex_start:
                            break;
                        case Align_Items.Flex_end:
                            children.ForEach(c => c.Translate(element.InnerRect.Width - c.OuterSize.Width, 0));
                            break;
                        case Align_Items.Center:
                            children.ForEach(c => c.Translate((element.InnerRect.Width / 2) - (c.OuterSize.Width / 2), 0));
                            break;
                    }
                    break;
            }
        }
    }
}
