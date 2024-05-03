using Leagueinator.Printer.Elements;

namespace Leagueinator.Printer.Styles {
    public partial class RenderNode(Element element) : TreeNode<RenderNode> {
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

        public RenderNode(RenderNode that) : this(that.Element) {
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

        public FlexRect OuterBox => new(
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

        /// <summary>
        /// Create a new render tree omitting nodes that aren't page 0 or the indicated page.
        /// All child nodes of a node that doesn't qualify are also omitted.
        /// </summary>
        /// <param name="page"></param>
        internal RenderNode CloneTree(int page) {
            RenderNode @new = new(this);

            foreach (RenderNode child in this.Children) {
                if (child.Page < 0 || child.Page == page) {
                    @new.AddChild(child.CloneTree(page));
                }
            }

            return @new;
        }

        public override string ToString() => this.Element.ToString();
    }
}
