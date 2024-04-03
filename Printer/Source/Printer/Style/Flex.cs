using Leagueinator.Printer.Elements;
using Leagueinator.Printer.Utility;
using System.Diagnostics;

namespace Leagueinator.Printer.Styles {
    public class Flex {
        private int pageCount = 1;
        private Queue<RenderNode> deferred = [];

        /// <summary>
        /// Begin the layout process, typcially only called on the node node.
        /// </summary>
        /// <returns></returns>
        public (int, RenderNode) DoLayout(Element element) {
            TabbedDebug.StartBlock($"Flex.DoLayout()");
            deferred = [];
            RenderNode root = BuildRenderTree(element, null);
            FirstPass(root);
            SecondPass();

            Debug.WriteLine(root.ToXML((node, xml) => {
                xml.AppendLine(node.Size.ToString());
            }));

            this.pageCount = this.AssignPages(root);
            this.DoPos(root);

            TabbedDebug.EndBlock();
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
            TabbedDebug.StartBlock($"Flex.FirstPass()");

            foreach (RenderNode node in root.AsList()) {
                if (node.Style.Flex_Axis == Enums.Flex_Axis.Row) {
                    EvaluateMajorWidth(node);
                    TabbedDebug.EndBlock();
                    EvaluateMinorHeight(node);
                    TabbedDebug.EndBlock();
                }
                else {
                    EvaluateMinorWidth(node);
                    TabbedDebug.EndBlock();
                    EvaluateMajorHeight(node);
                    TabbedDebug.EndBlock();
                }
            }

            TabbedDebug.EndBlock();
        }

        private void SecondPass() {
            TabbedDebug.StartBlock($"Flex.SecondPass()");
            foreach (RenderNode node in this.deferred) {
                TabbedDebug.WriteLine($"{node}");
                if (node.Style.Flex_Axis == Enums.Flex_Axis.Column) {
                    TabbedDebug.WriteLine($"sum height");
                    node.Size.Height = node.Children.Sum(c => c.Size.Height);
                }
                else {
                    TabbedDebug.WriteLine($"sum width");
                    node.Size.Width = node.Children.Sum(c => c.Size.Width);
                }
            }
        }

        public void EvaluateMajorWidth(RenderNode renderNode) {
            TabbedDebug.StartBlock($"Flex.EvaluateMajorWidth({renderNode})");
            var styleWidth = renderNode.Style.Width ?? new();

            TabbedDebug.WriteLine($"{styleWidth.Unit} {renderNode.IsRoot} {renderNode.IsLeaf}");
            if (styleWidth.Unit.Equals("px")) {
                renderNode.Size.Width = styleWidth.Factor;
            }
            else if (styleWidth.Unit.Equals("auto")) {
                if (renderNode.IsLeaf) {
                    renderNode.Size.Width = 0f;
                }
                else {
                    TabbedDebug.WriteLine($"deferred");
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
            TabbedDebug.StartBlock($"Flex.EvaluateMinorWidth({renderNode})");
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
            TabbedDebug.StartBlock($"Flex.EvaluateMajorHeight({renderNode})");
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
            TabbedDebug.StartBlock($"Flex.EvaluateMinorHeight({renderNode})");
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
            if (root.IsRoot) return;

            if (root.Style.Translate != null && !root.IsRoot) {
                if (root.Style.Translate.X.Factor.Equals("%")) {
                    root.Translation.X += root.Parent!.Size.Width * root.Parent!.Style.Translate!.X.Factor / 100;
                }
                else {
                    root.Translation.X += root.Style.Translate!.X.Factor;
                }
                if (root.Style.Translate.Y.Factor.Equals("%")) {
                    root.Translation.X += root.Parent!.Size.Height * root.Parent!.Style.Translate!.Y.Factor / 100;
                }
                else {
                    root.Translation.Y += root.Style.Translate!.Y.Factor;
                }
            }

            // position if absolute or flex position relative to parent
            if (root.Style.Position != Enums.Position.Fixed) {
                root.Translation.Translate(root.Parent!.ContentBox().TopLeft());
            }

            for (int page = 0; page < this.pageCount; page++) {
                this.DoPosFlex(root, page);
            }

            //this.DoPosAbsolute(node.ContentBox(), Enums.Position.Absolute);
            //this.DoPosAbsolute(node.ContentBox(), Enums.Position.Fixed);
        }

        /// <summary>
        /// Position all child nodes of node using justify content and align items.
        /// </summary>
        /// <param name="root"></param>
        /// <param name="page"></param>
        private void DoPosFlex(RenderNode root, int page) {
            var children = this.CollectChildren(root, page);
            if (children.Count == 0) return;

            this.JustifyContent(root, children);
            root.AlignItems(children);

            //foreach (Element child in children) child.Style.DoPos(node); // todo move this
        }


        /// <summary>
        /// Layout all this.Element nodes without taking into account alignment.
        /// </summary>
        /// <param name="this.Element"></param>
        /// <param name="children"></param>
        private void JustifyContent(RenderNode root, List<RenderNode> children) {
            switch (root.Style.Justify_Content) {
                default:
                case Enums.Justify_Content.Flex_start: {
                        root.LineupElements(children, new PointF(0, 0));
                        break;
                    }

                case Enums.Justify_Content.Flex_end: {
                        PointF from;

                        if (root.Style.Flex_Axis == Enums.Flex_Axis.Row) {
                            from = new PointF(root.WidthRemaining(children), 0);
                        }
                        else {
                            from = new PointF(0, root.HeightRemaining(children));
                        }

                        root.LineupElements(children, from);
                        break;
                    }

                case Enums.Justify_Content.Center: {
                        PointF from;

                        if (root.Style.Flex_Axis == Enums.Flex_Axis.Row) {
                            from = new PointF(root.WidthRemaining(children) / 2, 0);
                        }
                        else {
                            from = new PointF(0, root.HeightRemaining(children) / 2);
                        }

                        root.LineupElements(children, from);
                        break;
                    }

                case Enums.Justify_Content.Space_evenly:
                    root.SpaceEvenly(children);
                    break;
                case Enums.Justify_Content.Space_between:
                    root.SpaceBetween(children);
                    break;

                case Enums.Justify_Content.Space_around:
                    root.SpaceAround(children);
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
            node.Translation.Translate(x, y);
        }

        public static void Translate(this RenderNode node, PointF point) {
            node.Translation.Translate(point);
        }

        private static void LineupElements(this RenderNode root, List<RenderNode> children, PointF from) {
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

                foreach (var childNode in children) {
                    childNode.Translate(0, dy);
                    dy = dy + spaceBetween + childNode.OuterBox().Height;
                }
            }
            else {
                float spaceBetween = root.WidthRemaining(children) / (children.Count + 1);
                float dx = spaceBetween;

                foreach (var childElement in children) {
                    childElement.Translate(dx, 0);
                    dx = dx + spaceBetween + childElement.OuterBox().Width;
                }
            }
        }

        private static void SpaceBetween(this RenderNode root, List<RenderNode> children) {
            if (root.Style.Flex_Axis == Enums.Flex_Axis.Row) {
                float spaceBetween = root.WidthRemaining(children) / (children.Count - 1);
                float dx = 0;

                foreach (var child in children) {
                    child.Translate(dx, 0);
                    dx = dx + spaceBetween + child.OuterBox().Width;
                }
            }
            else {
                float spaceBetween = root.HeightRemaining(children) / (children.Count - 1);
                float dy = 0;

                foreach (var child in children) {
                    child.Translate(0, dy);
                    dy = dy + spaceBetween + child.OuterBox().Height;
                }
            }
        }

        private static void SpaceAround(this RenderNode root, List<RenderNode> children) {
            if (root.Style.Flex_Axis == Enums.Flex_Axis.Row) {
                float spaceAround = root.WidthRemaining(children) / (children.Count * 2);
                float dx = spaceAround;

                foreach (var child in children) {
                    child.Translate(dx, 0);
                    dx = dx + (2 * spaceAround) + child.OuterBox().Width;
                }
            }
            else {
                float spaceAround = root.HeightRemaining(children) / (children.Count * 2);
                float dy = spaceAround;

                foreach (var child in children) {
                    child.Translate(0, dy);
                    dy = dy + (2 * spaceAround) + child.OuterBox().Height;
                }
            }
        }

        private static void DoPosAbsolute(this RenderNode root, RectangleF reference, Enums.Position position) {
            root.Children
            .Where(child => child.Style.Position == position)
            .ToList()
            .ForEach(child => {
                float x = 0f, y = 0f;
                Style cStyle = child.Style;

                if (cStyle.Left != null) {
                    if (cStyle.Left.Unit.Equals("px")) x = cStyle.Left.Factor;
                    else x = cStyle.Left.Factor * root.Size.Width / 100;
                }
                if (cStyle.Right != null) {
                    if (cStyle.Right.Unit.Equals("px")) {
                        x = root.Size.Width - child.OuterBox().Width - cStyle.Right.Factor;
                    }
                    else {
                        x = root.Size.Width - child.OuterBox().Width - (cStyle.Right.Factor * root.Size.Width / 100);
                    }
                }
                if (cStyle.Top != null) {
                    if (cStyle.Top.Unit.Equals("px")) y = cStyle.Top.Factor;
                    else y = cStyle.Top.Factor * root.Size.Height / 100;
                }
                if (cStyle.Bottom != null) {
                    if (cStyle.Bottom.Unit.Equals("px")) {
                        y = root.Size.Height - child.OuterBox().Height - cStyle.Bottom.Factor;
                    }
                    else {
                        y = root.Size.Height - child.OuterBox().Height - (cStyle.Bottom.Factor * root.Size.Height / 100);
                    }
                }

                child.Translate(x, y);
            });
        }

        private static void AlignItems(this RenderNode root, List<RenderNode> children) {
            switch (root.Style.Flex_Axis) {
                case Enums.Flex_Axis.Row:
                    switch (root.Style.Align_Items) {
                        case Enums.Align_Items.Flex_start:
                            break;
                        case Enums.Align_Items.Flex_end:
                            children.ForEach(c => c.Translate(0, root.ContentBox().Height - c.OuterBox().Height));
                            break;
                        case Enums.Align_Items.Center:
                            children.ForEach(c => c.Translate(0, (root.ContentBox().Height / 2) - (c.OuterBox().Height / 2)));
                            break;
                    }
                    break;
                case Enums.Flex_Axis.Column:
                    switch (root.Style.Align_Items) {
                        case Enums.Align_Items.Flex_start:
                            break;
                        case Enums.Align_Items.Flex_end:
                            children.ForEach(c => c.Translate(root.ContentBox().Width - c.OuterBox().Width, 0));
                            break;
                        case Enums.Align_Items.Center:
                            children.ForEach(c => c.Translate((root.ContentBox().Width / 2) - (c.OuterBox().Width / 2), 0));
                            break;
                    }
                    break;
            }
        }
    }
}
