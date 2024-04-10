using Leagueinator.Printer.Elements;
using Leagueinator.Printer.Utility;
using System.Diagnostics;
using System.Drawing.Drawing2D;

namespace Leagueinator.Printer.Styles {
    public class RenderNode(Element element) : TreeNode<RenderNode> {
        public readonly Style Style = element.Style;
        public readonly Element Element = element;

        public SizeF Size = new();
        public Cardinal<float> Margin = [];
        public Cardinal<float> BorderSize = [];
        public Cardinal<float> Padding = [];
        public PointF Translation = new();
        public int Page { get; internal set; } = 0;

        internal RectangleF ContentBox() {
            return new(
                this.Translation.X + this.Margin!.Left + this.BorderSize!.Left + this.Padding!.Left,
                this.Translation.Y + this.Margin!.Top + this.BorderSize!.Top + this.Padding!.Top,
                this.Size.Width,
                this.Size.Height
            );
        }

        internal RectangleF PaddingBox() {
            return new(
                this.Translation.X + this.Margin!.Left + this.BorderSize!.Left,
                this.Translation.Y + this.Margin!.Top + this.BorderSize!.Top,
                this.Size.Width + Padding!.Left + Padding!.Right,
                this.Size.Height + Padding!.Top + Padding!.Bottom
            );
        }

        internal RectangleF BorderBox() {
            return new(
                this.Translation.X + this.Margin!.Left,
                this.Translation.Y + this.Margin!.Top,
                this.Size.Width + Padding!.Left + Padding!.Right + BorderSize!.Left + BorderSize!.Right,
                this.Size.Height + Padding!.Top + Padding!.Bottom + BorderSize!.Top + BorderSize!.Bottom
            );
        }

        internal RectangleF OuterBox() {
            return new(
                this.Translation.X,
                this.Translation.Y,
                this.Size.Width + Padding!.Left + Padding!.Right + BorderSize!.Left + BorderSize!.Right + this.Margin!.Left + this.Margin.Right,
                this.Size.Height + Padding!.Top + Padding!.Bottom + BorderSize!.Top + BorderSize!.Bottom + this.Margin!.Top + this.Margin.Bottom
            );
        }

        public virtual void Draw(Graphics g, int page) {
            Stack<RenderNode> stack = [];
            stack.Push(this);

            while (stack.Count > 0) {
                RenderNode current = stack.Pop();

                current.DoDrawBackground(g);
                current.DoDrawBorders(g);

                if (current.Style.Overflow == Styles.Enums.Overflow.Visible) {
                    foreach (RenderNode child in current.Children) stack.Push(child);
                }
                else if (current.Style.Overflow == Styles.Enums.Overflow.Paged) {
                    foreach (RenderNode child in current.Children) {
                        if (child.Style.Position == Styles.Enums.Position.Absolute) stack.Push(child);
                        else if (child.Style.Position == Styles.Enums.Position.Fixed) stack.Push(child);
                        else if (child.Page == page) stack.Push(child);
                        //else if (child.TagName == TextElement.TAG_NAME) stack.Push(child);
                    }
                }
            }
        }

        public void DoDrawBackground(Graphics g) {
            if (this.Style.MarginColor != null) {
                g.FillRectangle(new SolidBrush((Color)this.Style.MarginColor), this.OuterBox());
            }

            if (this.Style.PaddingColor != null) {
                g.FillRectangle(new SolidBrush((Color)this.Style.PaddingColor), this.PaddingBox());

                if (this.Style.BackgroundColor != null) {
                    g.FillRectangle(new SolidBrush((Color)this.Style.BackgroundColor), this.ContentBox());
                }
            }
            else if (this.Style.BackgroundColor != null) {
                g.FillRectangle(new SolidBrush((Color)this.Style.BackgroundColor), this.PaddingBox());
            }
        }

        public void DoDrawBorders(Graphics g) {
            if (this.Style.BorderColor is null) return;
            this.Style.BorderStyle ??= new(DashStyle.Solid);

            if (this.Style.BorderColor.Top != default && this.BorderSize.Top > 0) {
                using Pen pen = new Pen(this.Style.BorderColor.Top);
                pen.Width = this.BorderSize.Top;
                pen.DashStyle = this.Style.BorderStyle.Top;

                g.DrawLine(
                    pen,
                    this.BorderBox().TopLeft().Translate(0, this.BorderSize.Top / 2),
                    this.BorderBox().TopRight().Translate(0, this.BorderSize.Top / 2)
                );
            }
            if (this.Style.BorderColor.Right != default && this.BorderSize.Right > 0) {
                using Pen pen = new Pen(this.Style.BorderColor.Right);
                pen.Width = this.BorderSize.Right;
                pen.DashStyle = this.Style.BorderStyle.Right;

                g.DrawLine(
                    pen,
                    this.BorderBox().TopRight().Translate(-this.BorderSize.Right / 2, 0),
                    this.BorderBox().BottomRight().Translate(-this.BorderSize.Right / 2, 0)
                );
            }
            if (this.Style.BorderColor.Bottom != default && this.BorderSize.Bottom > 0) {
                using Pen pen = new Pen(this.Style.BorderColor.Bottom);
                pen.Width = this.BorderSize.Bottom;
                pen.DashStyle = this.Style.BorderStyle.Bottom;

                g.DrawLine(
                    pen,
                    this.BorderBox().BottomRight().Translate(0, -this.BorderSize.Bottom / 2),
                    this.BorderBox().BottomLeft().Translate(0, -this.BorderSize.Bottom / 2)
                );
            }
            if (this.Style.BorderColor.Left != default && this.BorderSize.Left > 0) {
                using Pen pen = new Pen(this.Style.BorderColor.Left);
                pen.Width = this.BorderSize.Left;
                pen.DashStyle = this.Style.BorderStyle.Left;

                g.DrawLine(
                    pen,
                    this.BorderBox().BottomLeft().Translate(this.BorderSize.Left / 2, 0),
                    this.BorderBox().TopLeft().Translate(this.BorderSize.Left / 2, 0)
                );
            }
        }

        public override string ToString() => this.Element.ToString();


    }
}
