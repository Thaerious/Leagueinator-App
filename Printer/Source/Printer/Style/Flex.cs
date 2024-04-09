using Leagueinator.Printer.Aspects;
using Leagueinator.Printer.Elements;
using Leagueinator.Printer.Utility;
using System.Diagnostics;

namespace Leagueinator.Printer.Styles {
    //[DebugTrace]
    public class Flex {
        private int pageCount = 1;
        private Queue<RenderNode> deferred = [];

        /// <summary>
        /// Begin the layout process, typcially only called on the node node.
        /// </summary>
        /// <returns></returns>
        public (int, RenderNode) DoLayout(Element element) {
            deferred = [];
            RenderNode root = BuildRenderTree(element, null);
            FirstPass(root);
            SecondPass();

            this.pageCount = this.AssignPages(root);
            this.DoPos(root);

            return (pageCount, root);
        }

        private RenderNode BuildRenderTree(Element element, RenderNode? node) {
            node ??= new(element);

            foreach (Element childElement in element.Children) {
                RenderNode childNode = new(childElement);
                node.AddChild(childNode);
                BuildRenderTree(childElement, childNode);
            }

            return node;
        }

        private void FirstPass(RenderNode root) {
            foreach (RenderNode node in root.AsList()) {
                if (node.Style.Flex_Axis == Enums.Flex_Axis.Row) {
                    EvaluateMajorWidth(node);
                    EvaluateMinorHeight(node);
                }
                else {
                    EvaluateMinorWidth(node);
                    EvaluateMajorHeight(node);
                }
            }
        }

        private void SecondPass() {
            foreach (RenderNode node in this.deferred) {
                if (node.Style.Flex_Axis == Enums.Flex_Axis.Column) {
                    node.Size.Height = node.Children.Sum(c => c.Size.Height);
                }
                else {
                    node.Size.Width = node.Children.Sum(c => c.Size.Width);
                }
            }
        }

        public void EvaluateMajorWidth(RenderNode renderNode) {
            var styleWidth = renderNode.Style.Width ?? new();

            if (styleWidth.Unit.Equals("px")) {
                renderNode.Size.Width = styleWidth.Factor;
            }
            else if (styleWidth.Unit.Equals("auto")) {
                if (renderNode.IsLeaf) {
                    renderNode.Size.Width = 0f;
                }
                else {
                    this.deferred.Enqueue(renderNode);
                }
            }
            else if (renderNode.IsRoot) {
                renderNode.Size.Width = 0;
            }
            else if (renderNode.IsLeaf && renderNode.Parent!.Style.Width!.Factor.Equals("auto")) {
                renderNode.Size.Width = 0;
            }
            else {
                renderNode.Size.Width = renderNode.Parent!.Size.Width * styleWidth.Factor / 100;
            }
        }

        public void EvaluateMinorWidth(RenderNode renderNode) {
            var unitFloat = renderNode.Style.Width ?? new();

            if (unitFloat.Unit.Equals("px")) {
                renderNode.Size.Width = unitFloat.Factor;
            }
            else if (renderNode.IsRoot) {
                renderNode.Size.Width = 0f;
            }
            else if (unitFloat.Unit.Equals("%")) { // branch and leaf
                renderNode.Size.Width = renderNode.Parent!.Size.Width * unitFloat.Factor / 100;
            }
            else {
                renderNode.Size.Width = renderNode.Parent!.Size.Width; // need to acommidate padding etc
            }
        }

        public void EvaluateMajorHeight(RenderNode renderNode) {
            var unitFloat = renderNode.Style.Height ?? new();

            if (unitFloat.Unit.Equals("px")) {
                renderNode.Size.Height = unitFloat.Factor;
            }
            else if (unitFloat.Unit.Equals("auto")) {
                if (renderNode.IsLeaf) {
                    renderNode.Size.Height = 0f;
                }
                else {
                    this.deferred.Enqueue(renderNode);
                    return;
                }
            }
            else if (renderNode.IsRoot) {
                renderNode.Size.Height = 0;
            }
            else if (renderNode.IsLeaf && renderNode.Parent!.Style.Height!.Factor.Equals("auto")) {
                renderNode.Size.Height = 0;
            }
            else {
                renderNode.Size.Height = renderNode.Parent!.Size.Height * unitFloat.Factor / 100;
            }
        }

        public void EvaluateMinorHeight(RenderNode renderNode) {
            var unitFloat = renderNode.Style.Height ?? new();

            if (unitFloat.Unit.Equals("px")) {
                renderNode.Size.Height = unitFloat.Factor;
            }
            else if (renderNode.IsRoot) {
                renderNode.Size.Height = 0f;
                return;
            }
            else if (unitFloat.Unit.Equals("%")) {// branch and leaf
                renderNode.Size.Height = renderNode.Parent!.Size.Height * unitFloat.Factor / 100;
            }
            else {
                renderNode.Size.Height = renderNode.Parent!.Size.Height; // need to acommidate padding etc
            }
        }

        internal void DoPos(RenderNode root) {
            new TreeWalker<RenderNode>(root).Walk(current => {
                if (current.IsRoot) return;

                if (current.Style.Translate != null && !current.IsRoot) {
                    if (current.Style.Translate.X.Factor.Equals("%")) {
                        current.Translation.X += current.Parent!.Size.Width * current.Parent!.Style.Translate!.X.Factor / 100;
                    }
                    else {
                        current.Translation.X += current.Style.Translate!.X.Factor;
                    }
                    if (current.Style.Translate.Y.Factor.Equals("%")) {
                        current.Translation.X += current.Parent!.Size.Height * current.Parent!.Style.Translate!.Y.Factor / 100;
                    }
                    else {
                        current.Translation.Y += current.Style.Translate!.Y.Factor;
                    }
                }

                // Position flex elements by page
                for (int page = 0; page < this.pageCount; page++) {
                    this.DoPosFlex(current, page);
                }

                if (current.Style.Position == Enums.Position.Absolute) current.DoPosAbsolute();
                if (current.Style.Position == Enums.Position.Fixed) current.DoPosFixed();

                // if absolute or flex, then position relative to parent
                if (current.Style.Position != Enums.Position.Fixed) {
                    current.Translation = current.Translation.Translate(current.Parent!.ContentBox().TopLeft());
                }
            });
        }

        /// <summary>
        /// Position all child nodes of node using justify content and align items.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="page"></param>
        private void DoPosFlex(RenderNode node, int page) {
            var children = this.CollectChildren(node, page);
            if (children.Count == 0) return;

            this.JustifyContent(node, children);
            node.AlignItems(children);
        }


        /// <summary>
        /// Layout all this.Element nodes without taking into account alignment.
        /// </summary>
        /// <param name="this.Element"></param>
        /// <param name="children"></param>
        private void JustifyContent(RenderNode node, List<RenderNode> children) {
            switch (node.Style.Justify_Content) {
                default:
                case Enums.Justify_Content.Flex_start:
                    node.JustifyStart(children);
                    break;
                case Enums.Justify_Content.Flex_end:
                    node.JustifyEnd(children);
                    break;
                case Enums.Justify_Content.Center:
                    node.JustifyCenter(children);
                    break;

                case Enums.Justify_Content.Space_evenly:
                    node.SpaceEvenly(children);
                    break;
                case Enums.Justify_Content.Space_between:
                    node.SpaceBetween(children);
                    break;

                case Enums.Justify_Content.Space_around:
                    node.SpaceAround(children);
                    break;
            }
        }

        /// <summary>
        /// Collect all this.Element nodes with a flex position and the specified page.
        /// </summary>
        /// <param name = "this.Element" ></ param >
        /// <returns></returns>
        private List<RenderNode> CollectChildren(RenderNode node, int page) {
            var children = node.Children
                                  .Where(rn => rn.Style.Position == Enums.Position.Flex)
                                  .Where(rn => node.Page == page)
                                  .ToList();

            if (node.Style.Flex_Direction == Enums.Direction.Reverse) {
                children.Reverse();
            }

            return children;
        }


        /// <summary>
        /// Calculates and assigns page numbers to this.s of a given _parent this.Element based on the 
        /// _parent's content Size and the heights of the children. Returns the total page count for 
        /// the _parent this.Element.
        /// </summary>
        /// <param name="this.Element">The _parent this.Element.</param>
        /// <returns>Total page count for all this.s.</returns>
        private int AssignPages(RenderNode root) {
            if (root.Style.Overflow != Enums.Overflow.Paged) return 1;
            Queue<RenderNode> children = new();

            int maxPage = 1;

            new TreeWalker<RenderNode>(root).Walk(node => {
                int page = 1;
                if (node.Children.Count == 1) {
                    node.Page = 1;
                }
                else {
                    float heightRemaining = node.ContentBox().Height;
                    foreach (RenderNode child in node.Children) {
                        child.Page = page;
                        heightRemaining -= child.OuterBox().Height;
                        if (heightRemaining <= 0) {
                            page++;
                            heightRemaining = node.ContentBox().Height;
                        }
                    }
                }
                if (page > maxPage) maxPage = page;
            });

            return maxPage;
        }
    }

    public static class RenderNodeExtension {
        /// <summary>
        ///  The amount of width left over when all children are taken into account.
        /// </summary>
        /// <param name="this.Element"></param>
        /// <returns></returns>
        public static float WidthRemaining(this RenderNode node, IList<RenderNode> children) {
            children ??= node.Children;
            float widthRemaining = node.ContentBox().Width;

            foreach (RenderNode child in children) {
                widthRemaining -= child.OuterBox().Width;
            }

            return widthRemaining;
        }

        /// <summary>
        ///  The amount of height left over when all children are taken into account.
        /// </summary>
        /// <param name="this.Element"></param>
        /// <returns></returns>
        public static float HeightRemaining(this RenderNode node, IList<RenderNode> children) {
            children ??= node.Children;
            float heightRemaining = node.ContentBox().Height;

            foreach (RenderNode child in children) {
                heightRemaining -= child.OuterBox().Height;
            }

            return heightRemaining;
        }

        public static void Translate(this RenderNode node, float x, float y) {
            node.Translate(new(x, y));
        }

        public static void Translate(this RenderNode node, PointF point) {
            node.Translation = node.Translation.Translate(point);
        }

        public static void LineupElements(this RenderNode root, List<RenderNode> children, PointF from) {
            PointF vector;
            if (root.Style.Flex_Axis == Enums.Flex_Axis.Column) {
                vector = new(0, 1);
            }
            else {
                vector = new(1, 0);
            }

            PointF current = from;
            foreach (RenderNode child in children) {
                child.Translate(current);
                var diff = new PointF(child.OuterBox().Width, child.OuterBox().Height).Scale(vector);
                current = current.Translate(diff);
            }
        }

        public static void SpaceEvenly(this RenderNode root, List<RenderNode> children) {
            if (root.Style.Flex_Axis == Enums.Flex_Axis.Column) {
                float spaceBetween = root.HeightRemaining(children) / (children.Count + 1);
                float dy = spaceBetween;

                foreach (var child in children) {
                    child.Translate(root.ContentBox().TopLeft());
                    child.Translate(0, dy);
                    dy = dy + spaceBetween + child.OuterBox().Height;
                }
            }
            else {
                float spaceBetween = root.WidthRemaining(children) / (children.Count + 1);
                float dx = spaceBetween;

                foreach (var child in children) {
                    child.Translate(root.ContentBox().TopLeft());
                    child.Translate(dx, 0);
                    dx = dx + spaceBetween + child.OuterBox().Width;
                }
            }
        }

        public static void SpaceBetween(this RenderNode root, List<RenderNode> children) {
            if (root.Style.Flex_Axis == Enums.Flex_Axis.Row) {
                float spaceBetween = root.WidthRemaining(children) / (children.Count - 1);
                float dx = 0;

                foreach (var child in children) {
                    child.Translate(root.ContentBox().TopLeft());
                    child.Translate(dx, 0);
                    dx = dx + spaceBetween + child.OuterBox().Width;
                }
            }
            else {
                float spaceBetween = root.HeightRemaining(children) / (children.Count - 1);
                float dy = 0;

                foreach (var child in children) {
                    child.Translate(root.ContentBox().TopLeft());
                    child.Translate(0, dy);
                    dy = dy + spaceBetween + child.OuterBox().Height;
                }
            }
        }

        public static void SpaceAround(this RenderNode root, List<RenderNode> children) {

            if (root.Style.Flex_Axis == Enums.Flex_Axis.Row) {
                float spaceAround = root.WidthRemaining(children) / (children.Count * 2);
                float dx = spaceAround;

                foreach (var child in children) {
                    child.Translate(root.ContentBox().TopLeft());
                    child.Translate(dx, 0);
                    dx = dx + (2 * spaceAround) + child.OuterBox().Width;
                }
            }
            else {
                float spaceAround = root.HeightRemaining(children) / (children.Count * 2);
                float dy = spaceAround;

                foreach (var child in children) {
                    child.Translate(root.ContentBox().TopLeft());
                    child.Translate(0, dy);
                    dy = dy + (2 * spaceAround) + child.OuterBox().Height;
                }
            }
        }

        public static void DoPosFixed(this RenderNode root) {
        }

        public static void DoPosAbsolute(this RenderNode node) {
            Debug.WriteLine($"DoPosAbsolute {node}");
            if (node.IsRoot) return;
            Debug.WriteLine(node.Parent.ContentBox());
            float x = 0f, y = 0f;

            if (node.Style.Left != null) {
                if (node.Style.Left.Unit.Equals("px")) x = node.Style.Left.Factor;
                else x = node.Style.Left.Factor * node.Parent!.ContentBox().Width / 100;
            }
            if (node.Style.Right != null) {
                if (node.Style.Right.Unit.Equals("px")) {
                    x = node.Parent!.ContentBox().Width - node.Size.Width - node.Style.Right.Factor;
                }
                else {
                    x = node.Parent!.ContentBox().Width - node.Size.Width - (node.Style.Right.Factor * node.Size.Width / 100);
                }
            }
            if (node.Style.Top != null) {
                if (node.Style.Top.Unit.Equals("px")) y = node.Style.Top.Factor;
                else y = node.Style.Top.Factor * node.Size.Height / 100;
            }
            if (node.Style.Bottom != null) {
                if (node.Style.Bottom.Unit.Equals("px")) {
                    y = node.Parent!.ContentBox().Height - node.Size.Height - node.Style.Bottom.Factor;
                }
                else {
                    y = node.Parent!.ContentBox().Height - node.Size.Height - (node.Style.Bottom.Factor * node.Size.Height / 100);
                }
            }

            node.Translate(x, y);
        }

        public static void AlignItems(this RenderNode node, List<RenderNode> children) {
            switch (node.Style.Flex_Axis) {
                case Enums.Flex_Axis.Row:
                    switch (node.Style.Align_Items) {
                        case Enums.Align_Items.Flex_start:
                            break;
                        case Enums.Align_Items.Flex_end:
                            children.ForEach(c => c.Translate(0, node.ContentBox().Height - c.OuterBox().Height));
                            break;
                        case Enums.Align_Items.Center:
                            children.ForEach(c => c.Translate(0, (node.ContentBox().Height / 2) - (c.OuterBox().Height / 2)));
                            break;
                    }
                    break;
                case Enums.Flex_Axis.Column:
                    switch (node.Style.Align_Items) {
                        case Enums.Align_Items.Flex_start:
                            break;
                        case Enums.Align_Items.Flex_end:
                            children.ForEach(c => c.Translate(node.ContentBox().Width - c.OuterBox().Width, 0));
                            break;
                        case Enums.Align_Items.Center:
                            children.ForEach(c => c.Translate((node.ContentBox().Width / 2) - (c.OuterBox().Width / 2), 0));
                            break;
                    }
                    break;
            }
        }

        //[DebugTrace]
        public static void JustifyCenter(this RenderNode node, List<RenderNode> children) {
            PointF from;

            if (node.Style.Flex_Axis == Enums.Flex_Axis.Row) {
                from = new PointF(node.WidthRemaining(children) / 2, 0);
            }
            else {
                from = new PointF(0, node.HeightRemaining(children) / 2);
            }

            from = from.Translate(node.ContentBox().TopLeft());
            node.LineupElements(children, from);
        }

        public static void JustifyEnd(this RenderNode node, List<RenderNode> children) {
            PointF from;

            if (node.Style.Flex_Axis == Enums.Flex_Axis.Row) {
                from = new PointF(node.WidthRemaining(children), 0);
            }
            else {
                from = new PointF(0, node.HeightRemaining(children));
            }

            from = from.Translate(node.ContentBox().TopLeft());
            node.LineupElements(children, from);
        }

        public static void JustifyStart(this RenderNode node, List<RenderNode> children) {
            node.LineupElements(children, node.ContentBox().TopLeft());
        }
    }
}
