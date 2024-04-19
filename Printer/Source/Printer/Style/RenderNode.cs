using Leagueinator.Printer.Elements;
using Leagueinator.Printer.Utility;
using Leagueinator.Utility;
using System.Diagnostics;
using System.Drawing.Drawing2D;

namespace Leagueinator.Printer.Styles {
    public class RenderNode(Element element) : TreeNode<RenderNode> {
        public readonly Style Style = element.Style;
        public readonly Element Element = element;

        /// <summary>
        /// Set the height and/or width of the content box.
        /// </summary>
        public FlexDim Size = new();
        public Cardinal<float> Margin = [];
        public Cardinal<float> BorderSize = [];
        public Cardinal<float> Padding = [];
        public PointF Translation = new();
        public int Page { get; internal set; } = -1;

        public RenderNode(RenderNode that) : this(that.Element){
            this.Size = that.Size;
            this.Margin = new(that.Margin);
            this.BorderSize = new(that.BorderSize);
            this.Padding = new(that.Padding);
            this.Translation = that.Translation;
            this.Page = that.Page;
        }

        internal FlexRect ContentBox => new(
            this.Translation.X + this.Margin!.Left + this.BorderSize!.Left + this.Padding!.Left,
            this.Translation.Y + this.Margin!.Top + this.BorderSize!.Top + this.Padding!.Top,
            this.Size.Width,
            this.Size.Height
        );

        internal FlexRect PaddingBox => new(
            this.Translation.X + this.Margin!.Left + this.BorderSize!.Left,
            this.Translation.Y + this.Margin!.Top + this.BorderSize!.Top,
            this.Size.Width + Padding!.Left + Padding!.Right,
            this.Size.Height + Padding!.Top + Padding!.Bottom
        );

        internal FlexRect BorderBox => new(
            this.Translation.X + this.Margin!.Left,
            this.Translation.Y + this.Margin!.Top,
            this.Size.Width + Padding!.Left + Padding!.Right + BorderSize!.Left + BorderSize!.Right,
            this.Size.Height + Padding!.Top + Padding!.Bottom + BorderSize!.Top + BorderSize!.Bottom
        );

        internal FlexRect OuterBox => new(
            this.Translation.X,
            this.Translation.Y,
            this.Size.Width + Padding!.Left + Padding!.Right + BorderSize!.Left + BorderSize!.Right + this.Margin!.Left + this.Margin.Right,
            this.Size.Height + Padding!.Top + Padding!.Bottom + BorderSize!.Top + BorderSize!.Bottom + this.Margin!.Top + this.Margin.Bottom
        );

        /// <summary>
        /// True if this node should to fit it's child _nodes.
        /// True when the node dimension is set to auto and the parent node does not have align stretch.
        /// </summary>
        /// <param name="dim"></param>
        /// <returns></returns>
        public bool IsFit(Dim dim) {
            UnitFloat unitFloat = dim == Dim.WIDTH ? this.Style.Width! : this.Style.Height!;
            if (!unitFloat.Unit.Equals("auto")) return false;
            if (this.IsRoot) return true;
            if (this.IsLeaf) return false;

            if (this.Parent!.IsFit(dim)) return true;
            return (this.Parent!.Style.Align_Items != Enums.Align_Items.Stretch);
        }

        /// <summary>
        /// True if this node should stretch to fit it's parent node.
        /// This is only true when the node dimension is set to auto and the parent has align of stretch (or auto).
        /// If the parent node isFit and this wants to stretch this changes to fit.
        /// </summary>
        /// <param name="dim"></param>
        /// <returns></returns>
        public bool IsStretch(Dim dim) {
            if (this.IsRoot) return false;
            if (this.Parent!.Style.Align_Items != Enums.Align_Items.Stretch) return false;
            UnitFloat unitFloat = dim == Dim.WIDTH ? this.Style.Width! : this.Style.Height!;
            if (!unitFloat.Unit.Equals("auto")) return false;

            if (this.Parent!.IsFit(dim)) return false;
            return true;
        }

        public virtual void Draw(Graphics g, int page) {
            Stack<RenderNode> stack = [];
            stack.Push(this);

            while (stack.Count > 0) {
                RenderNode current = stack.Pop();
                if (current.Page > 0 && current.Page != page) continue;

                current.DoDrawBackground(g);
                current.DoDrawBorders(g);
                if (current.Element.TagName == TextElement.TAG_NAME) current.DoDrawText(g);

                if (current.Style.Overflow == Styles.Enums.Overflow.Visible) {
                    foreach (RenderNode child in current.Children) stack.Push(child);
                }
                else if (current.Style.Overflow == Styles.Enums.Overflow.Paged) {
                    foreach (RenderNode child in current.Children) {
                        if (child.Style.Position == Styles.Enums.Position.Absolute) stack.Push(child);
                        else if (child.Style.Position == Styles.Enums.Position.Fixed) stack.Push(child);
                        else if (child.Page == page) stack.Push(child);
                        else if (child.Element.TagName == TextElement.TAG_NAME) stack.Push(child);
                    }
                }
            }
        }

        public void DoDrawText(Graphics g) {
            if (this.Element is not TextElement textElement) return;
            if (this.Style.Font == null) return;
            Brush brush = new SolidBrush(Color.Black);
            g.DrawString(textElement.Text, this.Style.Font, brush, this.ContentBox.TopLeft);
        }

        public void DoDrawBackground(Graphics g) {
            if (this.Style.MarginColor != null) {
                g.FillRectangle(new SolidBrush((Color)this.Style.MarginColor), this.OuterBox);
            }

            if (this.Style.PaddingColor != null) {
                g.FillRectangle(new SolidBrush((Color)this.Style.PaddingColor), this.PaddingBox);

                if (this.Style.BackgroundColor != null) {
                    g.FillRectangle(new SolidBrush((Color)this.Style.BackgroundColor), this.ContentBox);
                }
            }
            else if (this.Style.BackgroundColor != null) {
                g.FillRectangle(new SolidBrush((Color)this.Style.BackgroundColor), this.PaddingBox);
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
                    this.BorderBox.TopLeft.Translate(0, this.BorderSize.Top / 2),
                    this.BorderBox.TopRight.Translate(0, this.BorderSize.Top / 2)
                );
            }
            if (this.Style.BorderColor.Right != default && this.BorderSize.Right > 0) {
                using Pen pen = new Pen(this.Style.BorderColor.Right);
                pen.Width = this.BorderSize.Right;
                pen.DashStyle = this.Style.BorderStyle.Right;

                g.DrawLine(
                    pen,
                    this.BorderBox.TopRight.Translate(-this.BorderSize.Right / 2, 0),
                    this.BorderBox.BottomRight.Translate(-this.BorderSize.Right / 2, 0)
                );
            }
            if (this.Style.BorderColor.Bottom != default && this.BorderSize.Bottom > 0) {
                using Pen pen = new Pen(this.Style.BorderColor.Bottom);
                pen.Width = this.BorderSize.Bottom;
                pen.DashStyle = this.Style.BorderStyle.Bottom;

                g.DrawLine(
                    pen,
                    this.BorderBox.BottomRight.Translate(0, -this.BorderSize.Bottom / 2),
                    this.BorderBox.BottomLeft.Translate(0, -this.BorderSize.Bottom / 2)
                );
            }
            if (this.Style.BorderColor.Left != default && this.BorderSize.Left > 0) {
                using Pen pen = new Pen(this.Style.BorderColor.Left);
                pen.Width = this.BorderSize.Left;
                pen.DashStyle = this.Style.BorderStyle.Left;

                g.DrawLine(
                    pen,
                    this.BorderBox.BottomLeft.Translate(this.BorderSize.Left / 2, 0),
                    this.BorderBox.TopLeft.Translate(this.BorderSize.Left / 2, 0)
                );
            }
        }

        /// <summary>
        /// Create a new render tree omitting nodes that aren't page 0 or the indicated page.
        /// All child nodes of a node that doesn't qualify are also omitted.
        /// </summary>
        /// <param name="page"></param>
        internal RenderNode CloneTree(int page) {            
            RenderNode @new = new(this);

            if (@new.Page < 0 || @new.Page == page) {
                foreach (RenderNode child in this.Children) {
                    @new.AddChild(child.CloneTree(page));
                }
            }

            return @new;
        }

        public override string ToString() => this.Element.ToString();
    }
}
