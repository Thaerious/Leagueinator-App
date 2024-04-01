using Leagueinator.Printer.Elements;
using System.Drawing.Drawing2D;
using Leagueinator.Printer.Utility;
using System.Diagnostics;
using static Leagueinator.Printer.UnitFloat;

namespace Leagueinator.Printer.Styles {

    public static class ElementExt {

        public static float Evaluate(this UnitFloat unitFloat, float source) {
            if (unitFloat.Unit.Equals("px")) return unitFloat.Factor;
            if (unitFloat.Unit.Equals("%")) {
                return source * unitFloat.Factor / 100;
            }

            return 0f;
        }

        /// <summary>
        /// Return a percent _value of a parent's dimension.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="dim"></param>
        public static float ByParent(this Element element, Dim dim) {
            if (dim == Dim.WIDTH) {
                return (float)element.Parent!.Style.ContentBox().Width;
            }
            else {
                return (float)element.Parent!.Style.ContentBox().Height;
            }
        }

        /// <summary>
        /// Return a percent _value of a parent's dimension.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="dim"></param>
        public static float ByPercent(this Element element, Dim dim) {
            RectangleF contentBox = element.Parent!.Style.ContentBox();

            if (dim == Dim.WIDTH) {
                return (float)contentBox.Width * element.Style.Width!.Factor / 100;
            }
            else {
                return (float)contentBox.Height * element.Style.Height!.Factor / 100;
            }
        }

        /// <summary>
        /// Return the SUM of all child dimensions.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="dim"></param>
        /// <returns></returns>
        public static float BySum(this Element element, Dim dim) {
            if (dim == Dim.WIDTH) {
                return element.Children.Sum(c => (float)c.Style.OuterBox().Width);
            }
            else {
                return element.Children.Sum(c => (float)c.Style.OuterBox().Height);
            }
        }

        public static bool IsAuto(this Element element, Dim dim) {
            if (dim == Dim.WIDTH) {
                return element.Style.Width!.Unit.Equals("auto");
            }
            else {
                return element.Style.Height!.Unit.Equals("auto");
            }
        }
    }

    public class Flex : Style {
        public Element Element { get; }
        private PointF Translation = new();
        private int pageCount = 1;
        private bool isReady = false;

        public Flex(Element owner) {
            this.Element = owner;
        }

        /// <summary>
        /// Called the first time DoLayout is invoked.
        /// </summary>
        private void MakeReady() {
            if (!isReady) {
                this.Height!.ValueChange += this.HeightValueChange;
                this.Width!.ValueChange += this.WidthValueChange;
                this.Margin ??= new();
                this.BorderSize ??= new();
                this.Padding ??= new();
                this.isReady = true;
            }

            foreach (Element child in Element.Children) child.Style.MakeReady();
        }

        /// <summary>
        /// Begin the layout process, typcially only called on the root element.
        /// </summary>
        /// <returns></returns>
        public int DoLayout() {
            MakeReady();

            if (this.Flex_Axis == Enums.Flex_Axis.Column) {
                EvaluateMajor(this.Element, Dim.WIDTH);
                EvaluateMinor(this.Element, Dim.HEIGHT);
            }
            else {
                EvaluateMinor(this.Element, Dim.WIDTH);
                EvaluateMajor(this.Element, Dim.HEIGHT);
            }

            new ElementQueue(this.Element).Walk(ele => {
                if (ele.IsRoot) return;
                RectangleF contentBox = ele.Parent!.Style.ContentBox();
                ele.Style.Padding!.Top.Value = ele.Style.Padding.Top.Evaluate(contentBox.Width);
                ele.Style.Padding!.Right.Value = ele.Style.Padding.Top.Evaluate(contentBox.Width);
                ele.Style.Padding!.Bottom.Value = ele.Style.Padding.Top.Evaluate(contentBox.Width);
                ele.Style.Padding!.Left.Value = ele.Style.Padding.Top.Evaluate(contentBox.Width);
            });

            this.pageCount = this.AssignPages();
            this.DoPos();

            this.AssignInvokes();
            this.Element.Invalid = false;

            Debug.WriteLine($" : {pageCount}");
            return pageCount;
        }

        private void HeightValueChange(float value) {
            Debug.WriteLine($" - {this.Element}.HeightValueChange({value});");
        }

        private void WidthValueChange(float value) {
            Debug.WriteLine($" - {this.Element}.WidthValueChange({value});");

            this.Element.Children.SelectMany(child =>
                    child.Style.Padding!
                    .Concat(child.Style.Margin!)
                    .Concat(child.Style.BorderSize!)
            ).ToList()
            .ForEach(uf => {
                uf.ApplySource(value);
            });
        }

        public static void EvaluateMajor(Element element, Dim dim) {
            UnitFloat unitFloat = dim == Dim.WIDTH ? element.Style.Width! : element.Style.Height!;

            if (unitFloat.Unit.Equals("auto")) {
                if (element.IsLeaf) {
                    unitFloat.Value = 0;
                }
                else {
                    foreach (Element child in element.Children) EvaluateMajor(child, dim);
                    unitFloat.Value = element.BySum(dim);
                    Debug.WriteLine($"EvaluateMajor({element}, {dim}) : {unitFloat}");
                    return;
                }
            }

            if (unitFloat.Unit.Equals("px")) {
                unitFloat.Value = unitFloat.Factor;
            }
            else /* % */ {
                if (element.IsRoot) {
                    unitFloat.Value = 0;
                }
                else if (element.IsLeaf && element.Parent!.IsAuto(dim)) {
                    unitFloat.Value = 0;
                }
                else {
                    unitFloat.Value = element.ByPercent(dim);
                }
            }

            Debug.WriteLine($"EvaluateMajor({element}, {dim}) : {unitFloat}");
            foreach (Element child in element.Children) EvaluateMajor(child, dim);
        }

        public static void EvaluateMinor(Element element, Dim dim) {
            UnitFloat unitFloat = dim == Dim.WIDTH ? element.Style.Width! : element.Style.Height!;

            if (unitFloat.Unit.Equals("px")) {
                unitFloat.Value = unitFloat.Factor;
            }
            else if (element.IsRoot) {
                unitFloat.Value = 0f;
            }
            else if (unitFloat.Unit.Equals("%")) {
                unitFloat.Value = element.ByPercent(dim);
            }
            else /* special case: branch & leaf -> auto */ {
                if (dim == Dim.WIDTH) {
                    float w = (float)element.Parent!.Style.ContentBox().Width;
                    unitFloat.Value = w
                        - element.Style.Padding!.Left - element.Style.Padding!.Right
                        - element.Style.BorderSize!.Left - element.Style.BorderSize!.Right
                        - element.Style.Margin!.Left - element.Style.Margin!.Right;
                }
                else {
                    float h = (float)element.Parent!.Style.ContentBox().Width;
                    unitFloat.Value = h
                        - element.Style.Padding!.Top - element.Style.Padding!.Bottom
                        - element.Style.BorderSize!.Top - element.Style.BorderSize!.Bottom
                        - element.Style.Margin!.Top - element.Style.Margin!.Bottom;
                }
            }

            Debug.WriteLine($"EvaluateMinor({element}, {dim}) : {unitFloat}");
            foreach (Element child in element.Children) EvaluateMinor(child, dim);
        }

        internal RectangleF ContentBox() {
            return new(
                this.Translation.X + this.Margin!.Left + this.BorderSize!.Left + this.Padding!.Left,
                this.Translation.Y + this.Margin!.Top + this.BorderSize!.Top + this.Padding!.Top,
                this.Width,
                this.Height
            );
        }

        internal RectangleF PaddingBox() {
            return new(
                this.Translation.X + this.Margin!.Left + this.BorderSize!.Left,
                this.Translation.Y + this.Margin!.Top + this.BorderSize!.Top,
                this.Width + Padding!.Left + Padding!.Right,
                this.Height + Padding!.Top + Padding!.Bottom
            );
        }

        internal RectangleF BorderBox() {
            var paddingBox = this.PaddingBox();

            return new(
                this.Translation.X + this.Margin!.Left,
                this.Translation.Y + this.Margin!.Top,
                this.Width + Padding!.Left + Padding!.Right + BorderSize!.Left + BorderSize!.Right,
                this.Height + Padding!.Top + Padding!.Bottom + BorderSize!.Top + BorderSize!.Bottom
            );
        }


        internal RectangleF OuterBox() {
            var borderBox = this.BorderBox();

            return new(
                this.Translation.X,
                this.Translation.Y,
                this.Width + Padding!.Left + Padding!.Right + BorderSize!.Left + BorderSize!.Right + this.Margin!.Left + this.Margin.Right,
                this.Height + Padding!.Top + Padding!.Bottom + BorderSize!.Top + BorderSize!.Bottom + this.Margin!.Top + this.Margin.Bottom
            );
        }

        internal int DoPos() {
            if (this.Translate != null) {
                this.Transform(this.Translate.X, this.Translate.Y);
            }

            // position if absolute or flex position relative to parent
            if (this.Position != Enums.Position.Fixed) {
                this.Transform(this.Element.ContainerRect.TopLeft());
            }

            for (int page = 0; page < this.pageCount; page++) {
                this.DoPosFlex(page);
            }

            this.DoPosAbsolute(this.ContentBox(), Enums.Position.Absolute);
            this.DoPosAbsolute(this.ContentBox(), Enums.Position.Fixed);

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
                           x = cStyle.Left;
                       }
                       if (cStyle.Right != null) {
                           x = reference.Width - child.OuterRect.Width - cStyle.Right;
                       }
                       if (cStyle.Top != null) {
                           y = cStyle.Top;
                       }
                       if (cStyle.Bottom != null) {
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
                g.FillRectangle(new SolidBrush((Color)this.MarginColor), this.OuterBox());
            }

            if (this.PaddingColor != null) {
                g.FillRectangle(new SolidBrush((Color)this.PaddingColor), this.PaddingBox());

                if (this.BackgroundColor != null) {
                    g.FillRectangle(new SolidBrush((Color)this.BackgroundColor), this.ContentBox());
                }
            }
            else if (this.BackgroundColor != null) {
                g.FillRectangle(new SolidBrush((Color)this.BackgroundColor), this.PaddingBox());
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
                    this.BorderBox().TopLeft().Translate(0, this.BorderSize.Top / 2),
                    this.BorderBox().TopRight().Translate(0, this.BorderSize.Top / 2)
                );
            }
            if (this.BorderColor.Right != default) {
                using Pen pen = new Pen(this.BorderColor.Right);
                pen.Width = this.BorderSize.Right;
                pen.DashStyle = this.BorderStyle.Right;

                g.DrawLine(
                    pen,
                    this.BorderBox().TopRight().Translate(-this.BorderSize.Right / 2, 0),
                    this.BorderBox().BottomRight().Translate(-this.BorderSize.Right / 2, 0)
                );
            }
            if (this.BorderColor.Bottom != default) {
                using Pen pen = new Pen(this.BorderColor.Bottom);
                pen.Width = this.BorderSize.Bottom;
                pen.DashStyle = this.BorderStyle.Bottom;

                g.DrawLine(
                    pen,
                    this.BorderBox().BottomRight().Translate(0, -this.BorderSize.Bottom / 2),
                    this.BorderBox().BottomLeft().Translate(0, -this.BorderSize.Bottom / 2)
                );
            }
            if (this.BorderColor.Left != default) {
                using Pen pen = new Pen(this.BorderColor.Left);
                pen.Width = this.BorderSize.Left;
                pen.DashStyle = this.BorderStyle.Left;

                g.DrawLine(
                    pen,
                    this.BorderBox().BottomLeft().Translate(this.BorderSize.Left / 2, 0),
                    this.BorderBox().TopLeft().Translate(this.BorderSize.Left / 2, 0)
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
                            children.ForEach(c => c.Style.Transform(0, this.ContentBox().Height - c.OuterRect.Height));
                            break;
                        case Enums.Align_Items.Center:
                            children.ForEach(c => c.Style.Transform(0, (this.ContentBox().Height / 2) - (c.OuterRect.Height / 2)));
                            break;
                    }
                    break;
                case Enums.Flex_Axis.Column:
                    switch (this.Align_Items) {
                        case Enums.Align_Items.Flex_start:
                            break;
                        case Enums.Align_Items.Flex_end:
                            children.ForEach(c => c.Style.Transform(this.ContentBox().Width - c.OuterRect.Width, 0));
                            break;
                        case Enums.Align_Items.Center:
                            children.ForEach(c => c.Style.Transform((this.ContentBox().Width / 2) - (c.OuterRect.Width / 2), 0));
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
                float heightRemaining = this.ContentBox().Height;
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
            this.Translation = this.Translation.Translate(p);
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
