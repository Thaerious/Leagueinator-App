﻿using Printer.Printer;
using System.Diagnostics;
using System.Xml.Linq;

namespace Leagueinator.Printer {
    public class Flex : Style {
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
                case Enums.Flex_Axis.Default:
                case Enums.Flex_Axis.Row:
                    contentWidth = this.Width ?? sumWidth;
                    contentHeight = this.Height ?? maxHeight;
                    break;
                case Enums.Flex_Axis.Column:
                    contentWidth = this.Width ?? maxWidth;
                    contentHeight = this.Height ?? sumHeight;
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
            if (this.Width != null) this.Width.Factor = element.Parent.ContentRect.Width;
            if (this.Height != null) this.Height.Factor = element.Parent.ContentRect.Height;
            element.ContentSize = new SizeF(this.Width ?? 0f, this.Height ?? 0f);
        }

        public override void DoPos(Element element) {
            var children = this.CollectChildren(element);
            if (children.Count == 0) return;


            foreach (Element child in children) {
                var loc = child.Parent?.ContentRect.TopLeft() ?? new();
                child.Translation = loc;
            }

            this.JustifyContent(element, children);
            this.AlignItems(element, children);

            foreach (Element child in children) {
                var loc = child.Style.Location ?? new();
                child.Translate(loc);
                child.Style.DoPos(child);
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

            if (this.Flex_Major_Direction == Enums.Direction.Reverse) children.Reverse();
            int count = children.Count;

            switch (this.Justify_Content) {
                case Enums.Justify_Content.Default:
                case Enums.Justify_Content.Flex_start: {
                        PointF from;

                        if (this.Flex_Major_Direction == Enums.Direction.Forward) {
                            from = new PointF(0, 0);
                        }
                        else if (this.Flex_Major == Enums.Flex_Axis.Row) {
                            from = new PointF(widthRemaining, 0);
                        }
                        else {
                            from = new PointF(0, heightRemaining);
                        }

                        LayoutChildren(children, from);
                        break;
                    }

                case Enums.Justify_Content.Flex_end: {
                        PointF from;

                        if (this.Flex_Major_Direction == Enums.Direction.Reverse) {
                            from = new PointF(0, 0);
                        }
                        else if (this.Flex_Major == Enums.Flex_Axis.Row) {
                            from = new PointF(widthRemaining, 0);
                        }
                        else {
                            from = new PointF(0, heightRemaining);
                        }

                        LayoutChildren(children, from);
                        break;
                    }

                case Enums.Justify_Content.Center: {
                        PointF from;

                        if (this.Flex_Major == Enums.Flex_Axis.Row) {
                            from = new PointF(widthRemaining / 2, 0);
                        }
                        else {
                            from = new PointF(0, heightRemaining / 2);
                        }

                        LayoutChildren(children, from);
                        break;
                    }

                case Enums.Justify_Content.Space_evenly: {
                        if (this.Flex_Major == Enums.Flex_Axis.Row) {
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

                case Enums.Justify_Content.Space_between: {
                        if (this.Flex_Major == Enums.Flex_Axis.Row) {
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

                case Enums.Justify_Content.Space_around: {
                        if (this.Flex_Major == Enums.Flex_Axis.Row) {
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

        public override void AssignInvokes(Element element) {
            element.OnDraw += this.DoDrawBackground;
            element.OnDraw += this.DoDrawBorders;

            foreach (Element child in element.Children) child.Style.AssignInvokes(child);
        }

        public override void Draw(Graphics g, Element root) {
            Stack<Element> stack = [];
            stack.Push(root);

            while (stack.Count > 0) {
                Element element = stack.Pop();
                element.Draw(g);
                foreach (Element child in element.Children) stack.Push(child);
            }
        }

        public override void DrawPage(Graphics g, Element root, int page) {
            int currentPage = 0;
            foreach (Element child in root.Children) {
                if (child.OuterRect.Bottom > root.OuterRect.Bottom) {
                    currentPage++;
                }
                else {
                    child.Style.Draw(g, child);
                }
            }
        }

        public void DoDrawBackground(Graphics g, Element element) {
            if (this.BackgroundColor != null) {
                g.FillRectangle(new SolidBrush((Color)this.BackgroundColor), element.BorderRect);
            }
        }

        public void DoDrawBorders(Graphics g, Element element) {
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

        /// <summary>
        /// Collect all child nodes that don't have fixed position.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private List<Element> CollectChildren(Element element) {
            var children = element.Children;
            if (this.Flex_Major_Direction == Enums.Direction.Reverse) {
                Debug.WriteLine("REVERSE");
                children.Reverse();
            }
            return children;
        }

        private void LayoutChildren(List<Element> children, PointF from) {
            PointF vector;
            if (this.Flex_Major == Enums.Flex_Axis.Column) {
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

        private void AlignItems(Element element, List<Element> children) {
            switch (this.Flex_Major) {
                case Enums.Flex_Axis.Row:
                    switch (this.Align_Items) {
                        case Enums.Align_Items.Default:
                        case Enums.Align_Items.Flex_start:
                            break;
                        case Enums.Align_Items.Flex_end:
                            children.ForEach(c => c.Translate(0, element.ContentRect.Height - c.OuterSize.Height));
                            break;
                        case Enums.Align_Items.Center:
                            children.ForEach(c => c.Translate(0, (element.ContentRect.Height / 2) - (c.OuterSize.Height / 2)));
                            break;
                    }
                    break;
                case Enums.Flex_Axis.Column:
                    switch (this.Align_Items) {
                        case Enums.Align_Items.Default:
                        case Enums.Align_Items.Flex_start:
                            break;
                        case Enums.Align_Items.Flex_end:
                            children.ForEach(c => c.Translate(element.ContentRect.Width - c.OuterSize.Width, 0));
                            break;
                        case Enums.Align_Items.Center:
                            children.ForEach(c => c.Translate((element.ContentRect.Width / 2) - (c.OuterSize.Width / 2), 0));
                            break;
                    }
                    break;
            }
        }
    }
}
