using Leagueinator.Printer.Elements;
using System.Drawing.Drawing2D;
using Leagueinator.Printer.Utility;

namespace Leagueinator.Printer.Styles {
    public class Flex(Element owner) : Style {
        public Element Element { get; } = owner;

        private RectangleF PaddingBox = new();
        private RectangleF BorderBox = new();

        public int DoLayout() {
            this.DoSize();
            int pageCount = this.DoPos();
            this.AssignInvokes();
            this.Element.Invalid = false;
            return pageCount;
        }

        internal void DoSize() {
            if (this.Element.IsRoot) this.DoRootSize();
            else this.DoChildSize();
        }

        private void DoRootSize() {
            this.Element.OuterRect = new(0, 0, this.Width, this.Height);
            this.Element.ContentRect = new(0, 0, this.Width, this.Height);
            foreach (Element child in this.Element.Children) child.Style.DoSize();
        }

        private void DoChildSize() {
            float maxWidth = 0f;
            float maxHeight = 0f;
            float sumWidth = 0f;
            float sumHeight = 0f;

            foreach (Element child in this.Element.Children) {
                child.Style.DoSize();
                maxWidth = child.OuterRect.Width > maxWidth ? maxWidth = child.OuterRect.Width : maxWidth;
                maxHeight = child.OuterRect.Height > maxHeight ? maxHeight = child.OuterRect.Height : maxHeight;
                sumWidth += child.OuterRect.Width;
                sumHeight += child.OuterRect.Height;
            }

            float contentWidth = 0f, contentHeight = 0f;

            if (this.Element is TextElement textElement) {
                SizeF size = textElement.Size();
                contentWidth = size.Width;
                contentHeight = size.Height;
            }
            else {
                switch (this.Flex_Axis) {
                    case Enums.Flex_Axis.Row:
                        contentWidth = this.Width ?? sumWidth;
                        contentHeight = this.Height ?? maxHeight;
                        break;
                    case Enums.Flex_Axis.Column:
                        contentWidth = this.Width ?? maxWidth;
                        contentHeight = this.Height ?? sumHeight;
                        break;
                }
            }

            this.Element.ContentRect = new(
                this.Margin!.Left + this.BorderSize!.Left + this.Padding!.Left,
                this.Margin!.Top + this.BorderSize!.Top + this.Padding!.Top,
                contentWidth,
                contentHeight
            );

            this.PaddingBox = new(
                 this.Margin!.Left + this.BorderSize!.Left,
                 this.Margin!.Top + this.BorderSize!.Top,
                 this.Element.ContentRect.Width + Padding.Right + Padding.Left,
                 this.Element.ContentRect.Height + Padding.Top + Padding.Bottom
            );

            this.BorderBox = new(
                this.Margin!.Left,
                this.Margin!.Top,
                this.PaddingBox.Width + BorderSize.Left + BorderSize.Right,
                this.PaddingBox.Height + BorderSize.Top + BorderSize.Bottom
            );

            this.Element.OuterRect = new(
                0, 0,
                this.BorderBox.Width + this.Margin!.Left + this.Margin.Right,
                this.BorderBox.Height + this.Margin!.Top + this.Margin.Bottom
            );
        }

        internal int DoPos() {
            int pageCount = this.AssignPages();

            if (this.Translate != null) {
                this.Transform(this.Translate.X, this.Translate.Y);
            }

            if (this.Position != Enums.Position.Fixed) {
                this.Transform(this.Element.Parent?.ContentRect.TopLeft() ?? new());
            }

            for (int page = 0; page < pageCount; page++) {
                this.DoPosFlex(page);
            }

            this.DoPosAbsolute(this.Element.ContentRect, Enums.Position.Absolute);
            this.DoPosAbsolute(this.Element.Root.ContentRect, Enums.Position.Fixed);

            return pageCount;
        }

        private void DoPosFlex(int page) {
            var children = this.CollectChildren(page);
            if (children.Count == 0) return;

            this.JustifyContent(children);
            this.AlignItems(children);

            foreach (Element child in children) child.Style.DoPos();
        }

        private void DoPosAbsolute(RectangleF reference, Enums.Position position) {
            this.Element.Children
                   .Where(child => child.Style.Position == position)
                   .ToList()
                   .ForEach(child => {
                       float x = 0f, y = 0f;
                       var cStyle = child.Style;

                       if (cStyle.Left != null) {
                           cStyle.Left.Factor = reference.Width;
                           x = cStyle.Left;
                       }
                       if (cStyle.Right != null) {
                           cStyle.Right.Factor = reference.Width;
                           x = reference.Width - child.OuterRect.Width - cStyle.Right;
                       }
                       if (cStyle.Top != null) {
                           cStyle.Top.Factor = reference.Height;
                           y = cStyle.Top;
                       }
                       if (cStyle.Bottom != null) {
                           cStyle.Bottom.Factor = reference.Height;
                           y = reference.Height - child.OuterRect.Height - cStyle.Bottom;
                       }

                       child.Style.Transform(x, y);
                       child.Style.DoPos();
                   });
        }

        /// <summary>
        /// Layout all this.Element nodes without taking into account alignment.
        /// </summary>
        /// <param name="this.Element"></param>
        /// <param name="children"></param>
        private void JustifyContent(List<Element> children) {
            switch (this.Justify_Content) {
                default:
                case Enums.Justify_Content.Flex_start: {
                        this.LineupElements(children, new PointF(0, 0));
                        break;
                    }

                case Enums.Justify_Content.Flex_end: {
                        PointF from;

                        if (this.Flex_Axis == Enums.Flex_Axis.Row) {
                            from = new PointF(this.Element.WidthRemaining(children), 0);
                        }
                        else {
                            from = new PointF(0, this.Element.HeightRemaining(children));
                        }

                        this.LineupElements(children, from);
                        break;
                    }

                case Enums.Justify_Content.Center: {
                        PointF from;

                        if (this.Flex_Axis == Enums.Flex_Axis.Row) {
                            from = new PointF(this.Element.WidthRemaining(children) / 2, 0);
                        }
                        else {
                            from = new PointF(0, this.Element.HeightRemaining(children) / 2);
                        }

                        this.LineupElements(children, from);
                        break;
                    }

                case Enums.Justify_Content.Space_evenly:
                    this.SpaceEvenly(children);
                    break;
                case Enums.Justify_Content.Space_between:
                    this.SpaceBetween(children);
                    break;

                case Enums.Justify_Content.Space_around:
                    this.SpaceAround(children);
                    break;
            }
        }

        private void SpaceEvenly(List<Element> children) {
            if (this.Flex_Axis == Enums.Flex_Axis.Column) {
                float spaceBetween = this.Element.HeightRemaining(children) / (children.Count + 1);
                float dy = spaceBetween;

                foreach (var child in children) {
                    child.Style.Transform(0, dy);
                    dy = dy + spaceBetween + child.OuterRect.Height;
                }
            }
            else {
                float spaceBetween = this.Element.WidthRemaining(children) / (children.Count + 1);
                float dx = spaceBetween;

                foreach (var child in children) {
                    child.Style.Transform(dx, 0);
                    dx = dx + spaceBetween + child.OuterRect.Width;
                }
            }
        }

        private void SpaceBetween(List<Element> children) {
            if (this.Flex_Axis == Enums.Flex_Axis.Row) {
                float spaceBetween = this.Element.WidthRemaining(children) / (children.Count - 1);
                float dx = 0;

                foreach (var child in children) {
                    child.Style.Transform(dx, 0);
                    dx = dx + spaceBetween + child.OuterRect.Width;
                }
            }
            else {
                float spaceBetween = this.Element.HeightRemaining(children) / (children.Count - 1);
                float dy = 0;

                foreach (var child in children) {
                    child.Style.Transform(0, dy);
                    dy = dy + spaceBetween + child.OuterRect.Height;
                }
            }
        }

        private void SpaceAround(List<Element> children) {
            if (this.Flex_Axis == Enums.Flex_Axis.Row) {
                float spaceAround = this.Element.WidthRemaining(children) / (children.Count * 2);
                float dx = spaceAround;

                foreach (var child in children) {
                    child.Style.Transform(dx, 0);
                    dx = dx + (2 * spaceAround) + child.OuterRect.Width;
                }
            }
            else {
                float spaceAround = this.Element.HeightRemaining(children) / (children.Count * 2);
                float dy = spaceAround;

                foreach (var child in children) {
                    child.Style.Transform(0, dy);
                    dy = dy + (2 * spaceAround) + child.OuterRect.Height;
                }
            }
        }

        internal void AssignInvokes() {
            this.Element.OnDraw += this.DoDrawBackground;
            this.Element.OnDraw += this.DoDrawBorders;

            foreach (Element child in this.Element.Children) child.Style.AssignInvokes();
        }

        public void DoDrawBackground(Graphics g, Element __, int page) {
            if (this.MarginColor != null) {
                g.FillRectangle(new SolidBrush((Color)this.MarginColor), this.Element.OuterRect);
            }

            if (this.PaddingColor != null) {
                g.FillRectangle(new SolidBrush((Color)this.PaddingColor), this.PaddingBox);

                if (this.BackgroundColor != null) {
                    g.FillRectangle(new SolidBrush((Color)this.BackgroundColor), this.Element.ContentRect);
                }
            }
            else if (this.BackgroundColor != null) {
                    g.FillRectangle(new SolidBrush((Color)this.BackgroundColor), this.PaddingBox);
                }
            }

            public void DoDrawBorders(Graphics g, Element __, int page) {
                if (this.BorderColor is null) return;
                this.BorderSize ??= new();
                this.BorderStyle ??= new(DashStyle.Solid);

                if (this.BorderColor.Top != default) {
                    using Pen pen = new Pen(this.BorderColor.Top);
                    pen.Width = this.BorderSize.Top;
                    pen.DashStyle = this.BorderStyle.Top;

                    g.DrawLine(
                        pen,
                        this.BorderBox.TopLeft().Translate(0, this.BorderSize.Top / 2),
                        this.BorderBox.TopRight().Translate(0, this.BorderSize.Top / 2)
                    );
                }
                if (this.BorderColor.Right != default) {
                    using Pen pen = new Pen(this.BorderColor.Right);
                    pen.Width = this.BorderSize.Right;
                    pen.DashStyle = this.BorderStyle.Right;

                    g.DrawLine(
                        pen,
                        this.BorderBox.TopRight().Translate(-this.BorderSize.Right / 2, 0),
                        this.BorderBox.BottomRight().Translate(-this.BorderSize.Right / 2, 0)
                    );
                }
                if (this.BorderColor.Bottom != default) {
                    using Pen pen = new Pen(this.BorderColor.Bottom);
                    pen.Width = this.BorderSize.Bottom;
                    pen.DashStyle = this.BorderStyle.Bottom;

                    g.DrawLine(
                        pen,
                        this.BorderBox.BottomRight().Translate(0, -this.BorderSize.Bottom / 2),
                        this.BorderBox.BottomLeft().Translate(0, -this.BorderSize.Bottom / 2)
                    );
                }
                if (this.BorderColor.Left != default) {
                    using Pen pen = new Pen(this.BorderColor.Left);
                    pen.Width = this.BorderSize.Left;
                    pen.DashStyle = this.BorderStyle.Left;

                    g.DrawLine(
                        pen,
                        this.BorderBox.BottomLeft().Translate(this.BorderSize.Left / 2, 0),
                        this.BorderBox.TopLeft().Translate(this.BorderSize.Left / 2, 0)
                    );
                }
            }

            /// <summary>
            /// Collect all this.Element nodes with a flex position and the specified page.
            /// </summary>
            /// <param name = "this.Element" ></ param >
            /// <returns></returns>
            private List<Element> CollectChildren(int page) {
                var children = this.Element.Children
                                      .Where(ele => ele.Style.Position == Enums.Position.Flex)
                                      .Where(ele => ele.Style.Page == page)
                                      .ToList();

                if (this.Flex_Direction == Enums.Direction.Reverse) {
                    children.Reverse();
                }

                return children;
            }

            private void LineupElements(List<Element> children, PointF from) {
                PointF vector;
                if (this.Flex_Axis == Enums.Flex_Axis.Column) {
                    vector = new(0, 1);
                }
                else {
                    vector = new(1, 0);
                }

                PointF current = from;
                foreach (Element child in children) {
                    child.Style.Transform(current);
                    var diff = new PointF(child.OuterRect.Width, child.OuterRect.Height).Scale(vector);
                    current = current.Translate(diff);
                }
            }

            private void AlignItems(List<Element> children) {
                switch (this.Flex_Axis) {
                    case Enums.Flex_Axis.Row:
                        switch (this.Align_Items) {
                            case Enums.Align_Items.Flex_start:
                                break;
                            case Enums.Align_Items.Flex_end:
                                children.ForEach(c => c.Style.Transform(0, this.Element.ContentRect.Height - c.OuterRect.Height));
                                break;
                            case Enums.Align_Items.Center:
                                children.ForEach(c => c.Style.Transform(0, (this.Element.ContentRect.Height / 2) - (c.OuterRect.Height / 2)));
                                break;
                        }
                        break;
                    case Enums.Flex_Axis.Column:
                        switch (this.Align_Items) {
                            case Enums.Align_Items.Flex_start:
                                break;
                            case Enums.Align_Items.Flex_end:
                                children.ForEach(c => c.Style.Transform(this.Element.ContentRect.Width - c.OuterRect.Width, 0));
                                break;
                            case Enums.Align_Items.Center:
                                children.ForEach(c => c.Style.Transform((this.Element.ContentRect.Width / 2) - (c.OuterRect.Width / 2), 0));
                                break;
                        }
                        break;
                }
            }

            /// <summary>
            /// Calculates and assigns page numbers to this.s of a given _parent this.Element based on the 
            /// _parent's content size and the heights of the children. Returns the total page count for 
            /// the _parent this.Element.
            /// </summary>
            /// <param name="this.Element">The _parent this.Element.</param>
            /// <returns>Total page count for all this.s.</returns>
            private int AssignPages() {
                if (this.Element.Style.Overflow != Enums.Overflow.Paged) return 1;
                Queue<Element> children = new(this.Element.Children);

                int page = -1;

                while (children.Count > 0) {
                    page++;
                    float heightRemaining = this.Element.ContentRect.Height;
                    var child = children.Dequeue();
                    heightRemaining -= child.OuterRect.Height;
                    child.Style.Page = page;

                    while (children.Count > 0 && heightRemaining - children.Peek().OuterRect.Height > 0) {
                        child = children.Dequeue();
                        heightRemaining -= child.OuterRect.Height;
                        child.Style.Page = page;
                    }
                }

                return page + 1;
            }

            private void Transform(float x, float y) {
                this.Transform(new PointF(x, y));
            }

            private void Transform(PointF p) {
                this.Element.ContentRect = this.Element.ContentRect.Translate(p);
                this.PaddingBox = this.PaddingBox.Translate(p);
                this.BorderBox = this.BorderBox.Translate(p);
                this.Element.OuterRect = this.Element.OuterRect.Translate(p);
            }
        }

        public static class FlexExt {

            /// <summary>
            ///  The amount of width left over when all children are taken into account.
            /// </summary>
            /// <param name="this.Element"></param>
            /// <returns></returns>
            public static float WidthRemaining(this Element element, IList<Element> children) {
                children ??= element.Children;
                float widthRemaining = element.ContentRect.Width;

                foreach (Element child in children) {
                    widthRemaining -= child.OuterRect.Width;
                }

                return widthRemaining;
            }

            /// <summary>
            ///  The amount of height left over when all children are taken into account.
            /// </summary>
            /// <param name="this.Element"></param>
            /// <returns></returns>
            public static float HeightRemaining(this Element element, IList<Element> children) {
                children ??= element.Children;
                float heightRemaining = element.ContentRect.Height;

                foreach (Element child in children) {
                    heightRemaining -= child.OuterRect.Height;
                }

                return heightRemaining;
            }
        }
    }
