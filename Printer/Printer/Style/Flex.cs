﻿using Printer.Printer;
using System.Diagnostics;
using System.Drawing.Drawing2D;

namespace Leagueinator.Printer {
    public class Flex : Style {

        public override void DoSize(Element element) {
            if (element.IsRoot) {
                element.OuterSize = new(this.Width, this.Height);
                element.BorderSize = new(this.Width, this.Height);
                element.ContentSize = new(this.Width, this.Height);
                element.ContainerRect = new(0, 0, this.Width, this.Height);
                foreach (Element child in element.Children) child.Style.DoSize(child);
            } 
            else {
                this.DoChildSize(element);
            }
        }

        private void DoChildSize(Element element) {
            this.SetDefaultSize(element);

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

            this.BorderSize ??= new();
            this.Padding ??= new();
            this.Margin ??= new();

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
            if (this.Width != null) this.Width.Factor = element.Parent?.ContentRect.Width ?? 0f;
            if (this.Height != null) this.Height.Factor = element.Parent?.ContentRect.Height ?? 0f;
            element.ContentSize = new SizeF(this.Width ?? 0f, this.Height ?? 0f);
        }

        private void DoPosPage(Element element, int page) {
            var children = this.CollectChildren(element, page);
            if (children.Count == 0) return;

            foreach (Element child in children) {
                child.Translation = new();
            }

            this.JustifyContent(element, children);
            this.AlignItems(element, children);

            foreach (Element child in children) {
                child.Translate(child.Parent?.ContentRect.TopLeft() ?? new());
                child.Translate(child.Style.Location ?? new());
                child.Style.DoPos(child);
            }
        }

        private void DoPosAbsolute(Element element) {
            element.Children
                   .Where(child => child.Style.Position == Enums.Position.Absolute)
                   .ToList()
                   .ForEach(child => {
                       float x = 0f, y = 0f;
                       var cStyle = child.Style;

                       if (cStyle.Left != null) {
                           cStyle.Left.Factor = element.ContentRect.Width;
                           x = cStyle.Left;
                       }
                       if (cStyle.Right != null) {
                           cStyle.Right.Factor = element.ContentRect.Width;
                           x = element.ContentRect.Width - child.OuterSize.Width - cStyle.Right;
                       }
                       if (cStyle.Top != null) {
                           cStyle.Top.Factor = element.ContentRect.Height;
                           y = cStyle.Top;
                       }
                       if (cStyle.Bottom != null) {
                           cStyle.Bottom.Factor = element.ContentRect.Height;
                           y = element.ContentRect.Height - child.OuterSize.Height - cStyle.Bottom;
                       }

                       child.Translation = child.Parent?.ContentRect.TopLeft() ?? new();
                       child.Translate(new(x, y));
                       child.Translate(cStyle.Location ?? new());
                       child.Style.DoPos(child);
                   });
        }

        private void DoPosFixed(Element element) {
            element.Children
                   .Where(child => child.Style.Position == Enums.Position.Fixed)
                   .ToList()
                   .ForEach(child => {
                       child.Translate(child.Style.Location ?? new());
                       child.Style.DoPos(child);
                   });
        }

        public override int DoPos(Element element) {
            int pageCount = this.AssignPages(element);
            for (int page = 0; page < pageCount; page++) {
                this.DoPosPage(element, page);
            }
            this.DoPosAbsolute(element);
            this.DoPosFixed(element);
            return pageCount;
        }

        /// <summary>
        /// Layout all child nodes without taking into account alignment.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="children"></param>
        private void JustifyContent(Element element, List<Element> children) {
            int count = children.Count;

            switch (this.Justify_Content) {
                default:
                case Enums.Justify_Content.Flex_start: {
                        this.LayoutChildren(children, new PointF(0, 0));
                        break;
                    }

                case Enums.Justify_Content.Flex_end: {
                        PointF from;

                        if (this.Flex_Axis == Enums.Flex_Axis.Row) {
                            from = new PointF(element.WidthRemaining(children), 0);
                        }
                        else {
                            from = new PointF(0, element.HeightRemaining(children));
                        }

                        this.LayoutChildren(children, from);
                        break;
                    }

                case Enums.Justify_Content.Center: {
                        PointF from;

                        if (this.Flex_Axis == Enums.Flex_Axis.Row) {
                            from = new PointF(element.WidthRemaining(children) / 2, 0);
                        }
                        else {
                            from = new PointF(0, element.HeightRemaining(children) / 2);
                        }

                        this.LayoutChildren(children, from);
                        break;
                    }

                case Enums.Justify_Content.Space_evenly:
                    this.SpaceEvenly(children, element);
                    break;
                case Enums.Justify_Content.Space_between:
                    this.SpaceBetween(children, element);
                    break;

                case Enums.Justify_Content.Space_around:
                    this.SpaceAround(children, element);
                    break;
            }
        }

        private void SpaceEvenly(List<Element> children, Element element) {
            if (this.Flex_Axis == Enums.Flex_Axis.Column) {
                float spaceBetween = element.HeightRemaining(children) / (children.Count + 1);
                float dy = spaceBetween;

                foreach (var child in children) {
                    child.Translation = new(0, dy);
                    dy = dy + spaceBetween + child.OuterRect.Height;
                }
            }
            else {
                float spaceBetween = element.WidthRemaining(children) / (children.Count + 1);
                float dx = spaceBetween;

                foreach (var child in children) {
                    child.Translation = new(dx, 0);
                    dx = dx + spaceBetween + child.OuterRect.Width;
                }
            }
        }

        private void SpaceBetween(List<Element> children, Element element) {
            if (this.Flex_Axis == Enums.Flex_Axis.Row) {
                float spaceBetween = element.WidthRemaining(children) / (children.Count - 1);
                float dx = 0;

                foreach (var child in children) {
                    child.Translation = new(dx, child.Translation.Y);
                    dx = dx + spaceBetween + child.OuterRect.Width;
                }
            }
            else {
                float spaceBetween = element.HeightRemaining(children) / (children.Count - 1);
                float dy = 0;

                foreach (var child in children) {
                    child.Translation = new(child.Translation.X, dy);
                    dy = dy + spaceBetween + child.OuterRect.Height;
                }
            }
        }

        private void SpaceAround(List<Element> children, Element element) {
            if (this.Flex_Axis == Enums.Flex_Axis.Row) {
                float spaceAround = element.WidthRemaining(children) / (children.Count * 2);
                float dx = spaceAround;

                foreach (var child in children) {
                    child.Translation = new(dx, child.Translation.Y);
                    dx = dx + (2 * spaceAround) + child.OuterRect.Width;
                }
            }
            else {
                float spaceAround = element.HeightRemaining(children) / (children.Count * 2);
                float dy = spaceAround;

                foreach (var child in children) {
                    child.Translation = new(child.Translation.X, dy);
                    dy = dy + (2 * spaceAround) + child.OuterRect.Height;
                }
            }
        }

        public override void AssignInvokes(Element element) {
            element.OnDraw += this.DoDrawBackground;
            element.OnDraw += this.DoDrawBorders;

            foreach (Element child in element.Children) child.Style.AssignInvokes(child);
        }

        /// <summary>
        /// This is the main entry point when drawing the element to a graphics object.
        /// DoLayout() must be called before calling Draw().
        /// </summary>
        /// <param name="g"></param>
        /// <param name="root"></param>
        /// <param name="page"></param>
        public override void Draw(Graphics g, Element root, int page) {           
            Stack<Element> stack = [];
            stack.Push(root);

            while (stack.Count > 0) {
                Element element = stack.Pop();
                element.InvokeDrawHandlers(g, page);

                if (element.Style.Overflow == Enums.Overflow.Visible) {
                    foreach (Element child in element.Children) stack.Push(child);
                }
                else if (element.Style.Overflow == Enums.Overflow.Paged) {
                    foreach (Element child in element.Children) {
                        if (child.Style.Position == Enums.Position.Absolute) stack.Push(child);
                        else if (child.Style.Position == Enums.Position.Fixed) stack.Push(child);
                        else if (child.Style.Page == page) stack.Push(child);
                    }
                }
            }
        }

        public void DoDrawBackground(Graphics g, Element element, int page) {
            if (this.BackgroundColor != null) {
                g.FillRectangle(new SolidBrush((Color)this.BackgroundColor), element.BorderRect);
            }
        }

        public void DoDrawBorders(Graphics g, Element element, int page) {
            if (this.BorderColor is null) return;
            this.BorderSize ??= new();
            this.BorderStyle ??= new(DashStyle.Solid);

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
        /// Collect all child nodes with a flex position and the specified page.
        /// </summary>
        /// <param name = "element" ></ param >
        /// <returns></returns>
        private List<Element> CollectChildren(Element element, int page) {
            var children = element.Children
                                  .Where(ele => ele.Style.Position == Enums.Position.Flex)
                                  .Where(ele => ele.Style.Page == page)
                                  .ToList();

            if (this.Flex_Direction == Enums.Direction.Reverse) {
                children.Reverse();
            }

            return children;
        }

        private void LayoutChildren(List<Element> children, PointF from) {
            PointF vector;
            if (this.Flex_Axis == Enums.Flex_Axis.Column) {
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
            switch (this.Flex_Axis) {
                case Enums.Flex_Axis.Row:
                    switch (this.Align_Items) {
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

            children.ForEach(c => Debug.WriteLine($"{c.Identifier} {c.OuterRect}"));
        }

        /// <summary>
        /// Calculates and assigns page numbers to child elements of a given parent element based on the 
        /// parent's content size and the heights of the children. Returns the total page count for 
        /// the parent element.
        /// </summary>
        /// <param name="element">The parent element.</param>
        /// <returns>Total page count for all child elements.</returns>
        private int AssignPages(Element element) {
            if (element.Style.Overflow != Enums.Overflow.Paged) return 1;
            Queue<Element> children = new(element.Children);

            int page = -1;

            while (children.Count > 0) {
                page++;
                float heightRemaining = element.ContentSize.Height;
                var child = children.Dequeue();
                heightRemaining -= child.OuterSize.Height;
                child.Style.Page = page;

                while (children.Count > 0 && heightRemaining - children.Peek().OuterSize.Height > 0) {
                    child = children.Dequeue();
                    heightRemaining -= child.OuterSize.Height;
                    child.Style.Page = page;
                }
            }

            return page + 1;
        }
    }

    public static class FlexElememntExtensions {

        /// <summary>
        ///  The amount of width left over when all children are taken into account.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static float WidthRemaining(this Element element, List<Element> children) {
            children ??= element.Children;
            float widthRemaining = element.ContentSize.Width;

            foreach (Element child in children) {
                widthRemaining -= child.OuterSize.Width;
            }

            return widthRemaining;
        }

        /// <summary>
        ///  The amount of height left over when all children are taken into account.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static float HeightRemaining(this Element element, List<Element> children) {
            children ??= element.Children;
            float heightRemaining = element.ContentSize.Height;

            foreach (Element child in children) {
                heightRemaining -= child.OuterSize.Height;
            }

            return heightRemaining;
        }
    }

}
