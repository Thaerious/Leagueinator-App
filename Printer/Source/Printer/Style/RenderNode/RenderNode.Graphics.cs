using Leagueinator.Printer.Elements;
using Leagueinator.Printer.Utility;
using System.Drawing.Drawing2D;

namespace Leagueinator.Printer.Styles {
    public partial class RenderNode : TreeNode<RenderNode> {
        public virtual void Draw(Graphics g) {
            TreeWalker<RenderNode>.Walk(this, current => {
                current.DoDrawBackground(g);
                current.DoDrawBorders(g);
                if (current.Element.TagName == TextElement.TAG_NAME) current.DoDrawText(g);
            });
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
    }
}
