using Leagueinator.Printer.Elements;
using Leagueinator.Printer.Utility;

namespace Leagueinator.Printer.Styles {
    //[DebugTrace]
    public class Flex {
        private int pageCount = 1;
        private List<Action> deferred = [];

        /// <summary>
        /// Begin the layout process, typcially only called on the node node.
        /// </summary>
        /// <returns></returns>
        public (int, RenderNode) DoLayout(Element element) {
            deferred = [];
            RenderNode root = BuildRenderTree(element, null);

            SizeFirstPass(root);
            SizeSecondPass();
            this.pageCount = this.AssignPages(root);
            this.DoPos(root);

            return (pageCount, root);
        }

        private static RenderNode BuildRenderTree(Element element, RenderNode? node) {
            node ??= new(element);

            foreach (Element childElement in element.Children) {
                RenderNode childNode = new(childElement);
                node.AddChild(childNode);
                BuildRenderTree(childElement, childNode);
            }

            return node;
        }

        private void SizeFirstPass(RenderNode root) {
            TabbedDebug.ResetBlock($"1st Pass");

            TreeWalker<RenderNode>.Walk(root, node => {
                TabbedDebug.StartBlock($"{node}");

                if (node.Element.TagName == "@text") {
                    node.Size = new(((TextElement)node.Element).Size());
                    return;
                }

                SizeEdges(node);

                if (node.Style.Flex_Axis == Enums.Flex_Axis.Row) {
                    EvaluateMajor(node, Dim.WIDTH);
                    TabbedDebug.EndBlock();
                    EvaluateMinor(node, Dim.HEIGHT);
                    TabbedDebug.EndBlock();
                }
                else {
                    EvaluateMinor(node, Dim.WIDTH);
                    TabbedDebug.EndBlock();
                    EvaluateMajor(node, Dim.HEIGHT);
                    TabbedDebug.EndBlock();
                }

                TabbedDebug.EndBlock();
            });
        }

        /// <summary>
        /// Set size of parent to sum of child size on the major axis.
        /// </summary>
        private void SizeSecondPass() {
            TabbedDebug.ResetBlock($"2nd Pass");
            foreach (Action action in this.deferred) action();
        }

        private static float ValueFromUnit(RenderNode node, UnitFloat unitFloat) {
            if (unitFloat.Unit.Equals("px")) return unitFloat.Factor;
            if (node.IsRoot) return 0;
            return node.Parent!.ContentBox.Width * unitFloat.Factor / 100;
        }

        private static void SizeEdges(RenderNode node) {
            if (node.Style.Padding is not null) {
                node.Padding = new Cardinal<float>() {
                    Top = ValueFromUnit(node, node.Style.Padding.Top),
                    Right = ValueFromUnit(node, node.Style.Padding.Right),
                    Bottom = ValueFromUnit(node, node.Style.Padding.Bottom),
                    Left = ValueFromUnit(node, node.Style.Padding.Left)
                };
            }

            if (node.Style.BorderSize is not null) {
                node.BorderSize = new Cardinal<float>() {
                    Top = ValueFromUnit(node, node.Style.BorderSize.Top),
                    Right = ValueFromUnit(node, node.Style.BorderSize.Right),
                    Bottom = ValueFromUnit(node, node.Style.BorderSize.Bottom),
                    Left = ValueFromUnit(node, node.Style.BorderSize.Left)
                };
            }

            if (node.Style.Margin is not null) {
                node.Margin = new Cardinal<float>() {
                    Top = ValueFromUnit(node, node.Style.Margin.Top),
                    Right = ValueFromUnit(node, node.Style.Margin.Right),
                    Bottom = ValueFromUnit(node, node.Style.Margin.Bottom),
                    Left = ValueFromUnit(node, node.Style.Margin.Left)
                };
            }
        }

        public void EvaluateMajor(RenderNode node, Dim dim) {
            TabbedDebug.StartBlock($"EvaluateMajor {node} {dim}");
            UnitFloat styleVal = dim == Dim.WIDTH ? node.Style.Width! : node.Style.Height!;
            styleVal ??= new();

            if (styleVal.Unit.Equals("px")) {
                TabbedDebug.WriteLine($"{node}[{dim}] = {styleVal.Factor} px");
                node.Size[dim] = styleVal.Factor;
            }
            else if (node.IsRoot) {
                if (styleVal.Unit.Equals("%")) {
                    throw new NotImplementedException("Root size by % not permitted");
                }
                if (styleVal.Unit.Equals("auto")) {
                    TabbedDebug.WriteLine($"auto deferred");
                    this.deferred.Insert(0, () => {
                        node.Size[dim] = node.Children.Sum(c => c.OuterBox[dim]);
                        TabbedDebug.WriteLine($"{node}[{dim}] = {node.Size[dim]}");
                    });
                }
            }
            else if (node.IsLeaf) {
                if (styleVal.Unit.Equals("%")) {
                    if (styleVal.Unit.Equals("%")) {
                        node.Size[dim] = node.Parent!.Size[dim] * styleVal.Factor / 100;
                    }
                    if (styleVal.Unit.Equals("auto")) {
                        node.Size[dim] = 0f;
                    }
                }
                if (styleVal.Unit.Equals("auto")) {
                    node.Size[dim] = 0f;
                }
            }
            else {
                if (styleVal.Unit.Equals("%")) {
                    node.Size[dim] = node.Parent!.Size[dim] * styleVal.Factor / 100;
                }
                if (styleVal.Unit.Equals("auto")) {
                    this.deferred.Insert(0, () => {
                        node.Size[dim] = node.Children.Sum(c => c.OuterBox[dim]);
                        TabbedDebug.WriteLine($"{node}[{dim}] = {node.Size[dim]}");
                    });
                }
            }
        }

        public void EvaluateMinor(RenderNode node, Dim dim) {
            TabbedDebug.StartBlock($"EvaluateMinor {node} {dim}");
            UnitFloat styleVal = dim == Dim.WIDTH ? node.Style.Width! : node.Style.Height!;
            styleVal ??= new();

            if (styleVal.Unit.Equals("px")) {
                TabbedDebug.WriteLine($"{node}[{dim}] = {styleVal.Factor} px");
                node.Size[dim] = styleVal.Factor;
            }
            else if (node.IsRoot) {
                if (styleVal.Unit.Equals("%")) {
                    throw new NotImplementedException("Root size by % not permitted");
                }
                if (styleVal.Unit.Equals("auto")) {
                    TabbedDebug.WriteLine($"auto deferred");
                    this.deferred.Insert(0, () => {
                        node.Size[dim] = node.Children.Max(c => c.OuterBox[dim]);
                        TabbedDebug.WriteLine($"{node}[{dim}] = {node.Size[dim]}");
                    });
                }
            }
            else {
                if (styleVal.Unit.Equals("%")) {
                    TabbedDebug.WriteLine($"{styleVal.Factor} % of parent");
                    node.Size[dim] = node.Parent!.Size[dim] * styleVal.Factor / 100;
                }
                else /* auto */{
                    if (node.IsStretch(dim)) {
                        if (node.Parent!.IsFit(dim)) {
                            this.deferred.Insert(0, () => {
                                node.Size[dim] = node.Children.Max(c => c.OuterBox[dim]);
                                TabbedDebug.WriteLine($"{node}[{dim}] = {node.Size[dim]}");
                            });
                        }
                        else {
                            TabbedDebug.WriteLine($"parent stretch deferred");
                            this.deferred.Add(() => {
                                node.Size[dim] = node.Parent.ContentBox[dim] - node.OuterBox[dim] + node.ContentBox[dim];
                                TabbedDebug.WriteLine($"{node}[{dim}] = {node.Size[dim]}");
                            });
                        }
                    }
                    else {
                        if (node.IsLeaf) node.Size[dim] = 0;
                        else {
                            TabbedDebug.WriteLine($"parent other deferred");
                            this.deferred.Insert(0, () => {
                                node.Size[dim] = node.Children.Max(c => c.OuterBox[dim]);
                                TabbedDebug.WriteLine($"{node}[{dim}] = {node.Size[dim]}");
                            });
                        }
                    }
                }
            }
        }

        internal void DoPos(RenderNode root) {
            new TreeWalker<RenderNode>(root).Walk(current => {
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
                    if (current.Style.Position == Enums.Position.Flex) {
                        this.DoPosFlex(current, page);
                    }
                }

                if (current.Style.Position == Enums.Position.Absolute) current.DoPosAbsolute(current.Parent!);
                if (current.Style.Position == Enums.Position.Fixed) current.DoPosAbsolute(current.Root);

                // if absolute or flex, then position relative to parent
                if (current.Style.Position != Enums.Position.Fixed && !current.IsRoot) {
                    current.Translation = current.Translation.Translate(current.Parent!.ContentBox.TopLeft);
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
                    float heightRemaining = node.ContentBox.Height;
                    foreach (RenderNode child in node.Children) {
                        child.Page = page;
                        heightRemaining -= child.OuterBox.Height;
                        if (heightRemaining <= 0) {
                            page++;
                            heightRemaining = node.ContentBox.Height;
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
            float widthRemaining = node.ContentBox.Width;

            foreach (RenderNode child in children) {
                widthRemaining -= child.OuterBox.Width;
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
            float heightRemaining = node.ContentBox.Height;

            foreach (RenderNode child in children) {
                heightRemaining -= child.OuterBox.Height;
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
                var diff = new PointF(child.OuterBox.Width, child.OuterBox.Height).Scale(vector);
                current = current.Translate(diff);
            }
        }

        public static void SpaceEvenly(this RenderNode root, List<RenderNode> children) {
            if (root.Style.Flex_Axis == Enums.Flex_Axis.Column) {
                float spaceBetween = root.HeightRemaining(children) / (children.Count + 1);
                float dy = spaceBetween;

                foreach (var child in children) {
                    child.Translate(0, dy);
                    dy = dy + spaceBetween + child.OuterBox.Height;
                }
            }
            else {
                float spaceBetween = root.WidthRemaining(children) / (children.Count + 1);
                float dx = spaceBetween;

                foreach (var child in children) {
                    child.Translate(dx, 0);
                    dx = dx + spaceBetween + child.OuterBox.Width;
                }
            }
        }

        public static void SpaceBetween(this RenderNode root, List<RenderNode> children) {
            if (root.Style.Flex_Axis == Enums.Flex_Axis.Row) {
                float spaceBetween = root.WidthRemaining(children) / (children.Count - 1);
                float dx = 0;

                foreach (var child in children) {
                    child.Translate(dx, 0);
                    dx = dx + spaceBetween + child.OuterBox.Width;
                }
            }
            else {
                float spaceBetween = root.HeightRemaining(children) / (children.Count - 1);
                float dy = 0;

                foreach (var child in children) {
                    child.Translate(0, dy);
                    dy = dy + spaceBetween + child.OuterBox.Height;
                }
            }
        }

        public static void SpaceAround(this RenderNode root, List<RenderNode> children) {

            if (root.Style.Flex_Axis == Enums.Flex_Axis.Row) {
                float spaceAround = root.WidthRemaining(children) / (children.Count * 2);
                float dx = spaceAround;

                foreach (var child in children) {
                    child.Translate(dx, 0);
                    dx = dx + (2 * spaceAround) + child.OuterBox.Width;
                }
            }
            else {
                float spaceAround = root.HeightRemaining(children) / (children.Count * 2);
                float dy = spaceAround;

                foreach (var child in children) {
                    child.Translate(0, dy);
                    dy = dy + (2 * spaceAround) + child.OuterBox.Height;
                }
            }
        }

        public static void DoPosAbsolute(this RenderNode node, RenderNode relative) {
            if (node.IsRoot) return;
            float x = 0f, y = 0f;

            if (node.Style.Left != null) {
                if (node.Style.Left.Unit.Equals("px")) x = node.Style.Left.Factor;
                else x = node.Style.Left.Factor * relative.ContentBox.Width / 100;
            }
            if (node.Style.Right != null) {
                if (node.Style.Right.Unit.Equals("px")) {
                    x = relative.ContentBox.Width - node.Size.Width - node.Style.Right.Factor;
                }
                else {
                    x = relative.ContentBox.Width - node.Size.Width - (node.Style.Right.Factor * node.Size.Width / 100);
                }
            }
            if (node.Style.Top != null) {
                if (node.Style.Top.Unit.Equals("px")) y = node.Style.Top.Factor;
                else y = node.Style.Top.Factor * node.Size.Height / 100;
            }
            if (node.Style.Bottom != null) {
                if (node.Style.Bottom.Unit.Equals("px")) {
                    y = relative.ContentBox.Height - node.Size.Height - node.Style.Bottom.Factor;
                }
                else {
                    y = relative.ContentBox.Height - node.Size.Height - (node.Style.Bottom.Factor * node.Size.Height / 100);
                }
            }

            node.Translate(x, y);
        }

        public static void AlignItems(this RenderNode node, List<RenderNode> children) {
            switch (node.Style.Flex_Axis) {
                case Enums.Flex_Axis.Row:
                    switch (node.Style.Align_Items) {
                        case Enums.Align_Items.Flex_start:
                        case Enums.Align_Items.Stretch:
                            break;
                        case Enums.Align_Items.Flex_end:
                            children.ForEach(c => c.Translate(0, node.ContentBox.Height - c.OuterBox.Height));
                            break;
                        case Enums.Align_Items.Center:
                            children.ForEach(c => c.Translate(0, (node.ContentBox.Height / 2) - (c.OuterBox.Height / 2)));
                            break;
                    }
                    break;
                case Enums.Flex_Axis.Column:
                    switch (node.Style.Align_Items) {
                        case Enums.Align_Items.Flex_start:
                        case Enums.Align_Items.Stretch:
                            break;
                        case Enums.Align_Items.Flex_end:
                            children.ForEach(c => c.Translate(node.ContentBox.Width - c.OuterBox.Width, 0));
                            break;
                        case Enums.Align_Items.Center:
                            children.ForEach(c => c.Translate((node.ContentBox.Width / 2) - (c.OuterBox.Width / 2), 0));
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

            node.LineupElements(children, from);
        }

        public static void JustifyStart(this RenderNode node, List<RenderNode> children) {
            node.LineupElements(children, new());
        }
    }
}
