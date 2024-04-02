using Leagueinator.Printer.Elements;
using System.Diagnostics;

namespace Leagueinator.Printer.Styles {
    public class Flex {
        private int pageCount = 1;
        private Queue<RenderNode> deferred = [];

        /// <summary>
        /// Begin the layout process, typcially only called on the root node.
        /// </summary>
        /// <returns></returns>
        public (int, RenderNode) DoLayout(Element element) {
            TabbedDebug.StartBlock($"Flex.DoLayout()");
            deferred = [];
            RenderNode renderNode = BuildRenderTree(element, null);
            FirstPass(renderNode);
            SecondPass();

            Debug.WriteLine(renderNode.ToXML((node, xml) => {
                xml.AppendLine(node.Size.ToString());
            }));

            //this.pageCount = this.AssignPages();
            //this.DoPos(node);

            TabbedDebug.EndBlock();
            return (pageCount, renderNode);
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

        //internal int DoPos(RenderNode node) {
        //    if (this.Translate != null && !node.IsRoot) {
        //        if (this.Translate.X.Factor.Equals("%")){
        //            node.Translation.X += 
        //                node.Parent!.Size.Width * node.Parent!.Style.Translate!.X.Factor / 100;
        //        } else {
        //            node.Translation.X += this.Translate!.X.Factor;
        //        }
        //        if (this.Translate.Y.Factor.Equals("%")) {
        //            node.Translation.X +=
        //                node.Parent!.Size.Height * node.Parent!.Style.Translate!.Y.Factor / 100;
        //        }
        //        else {
        //            node.Translation.Y += this.Translate!.Y.Factor;
        //        }
        //    }

        //    // position if absolute or flex position relative to parent
        //    if (this.Position != Enums.Position.Fixed) {
        //        this.Transform(this.Element.ContainerRect.TopLeft());
        //    }

        //    for (int page = 0; page < this.pageCount; page++) {
        //        this.DoPosFlex(node, page);
        //    }

        //    //this.DoPosAbsolute(node.ContentBox(), Enums.Position.Absolute);
        //    //this.DoPosAbsolute(node.ContentBox(), Enums.Position.Fixed);

        //    return pageCount;
        //}

        //private void DoPosFlex(RenderNode node, int page) {
        //    var children = this.CollectChildren(page);
        //    if (children.Count == 0) return;

        //    this.JustifyContent(children);
        //    this.AlignItems(children);

        //    foreach (Element childElement in children) childElement.Style.DoPos(node);
        //}

        //private void DoPosAbsolute(RectangleF reference, Enums.Position position) {
        //    this.Element.Children
        //           .Where(childElement => childElement.Style.Position == position)
        //           .ToList()
        //           .ForEach(childElement => {
        //               float x = 0f, y = 0f;
        //               var cStyle = childElement.Style;

        //               if (cStyle.Left != null) {
        //                   x = cStyle.Left;
        //               }
        //               if (cStyle.Right != null) {
        //                   x = reference.Width - childElement.OuterRect.Width - cStyle.Right;
        //               }
        //               if (cStyle.Top != null) {
        //                   y = cStyle.Top;
        //               }
        //               if (cStyle.Bottom != null) {
        //                   y = reference.Height - childElement.OuterRect.Height - cStyle.Bottom;
        //               }

        //               childElement.Style.Transform(x, y);
        //               childElement.Style.DoPos();
        //           });
        //}

        /// <summary>
        /// Layout all this.Element nodes without taking into account alignment.
        /// </summary>
        /// <param name="this.Element"></param>
        /// <param name="children"></param>
        //private void JustifyContent(List<Element> children) {
        //    switch (this.Justify_Content) {
        //        default:
        //        case Enums.Justify_Content.Flex_start: {
        //                this.LineupElements(children, new PointF(0, 0));
        //                break;
        //            }

        //        case Enums.Justify_Content.Flex_end: {
        //                PointF from;

        //                if (this.Flex_Axis == Enums.Flex_Axis.Row) {
        //                    from = new PointF(this.Element.WidthRemaining(children), 0);
        //                }
        //                else {
        //                    from = new PointF(0, this.Element.HeightRemaining(children));
        //                }

        //                this.LineupElements(children, from);
        //                break;
        //            }

        //        case Enums.Justify_Content.Center: {
        //                PointF from;

        //                if (this.Flex_Axis == Enums.Flex_Axis.Row) {
        //                    from = new PointF(this.Element.WidthRemaining(children) / 2, 0);
        //                }
        //                else {
        //                    from = new PointF(0, this.Element.HeightRemaining(children) / 2);
        //                }

        //                this.LineupElements(children, from);
        //                break;
        //            }

        //        case Enums.Justify_Content.Space_evenly:
        //            this.SpaceEvenly(children);
        //            break;
        //        case Enums.Justify_Content.Space_between:
        //            this.SpaceBetween(children);
        //            break;

        //        case Enums.Justify_Content.Space_around:
        //            this.SpaceAround(children);
        //            break;
        //    }
        //}

        //private void SpaceEvenly(List<Element> children) {
        //    if (this.Flex_Axis == Enums.Flex_Axis.Column) {
        //        float spaceBetween = this.Element.HeightRemaining(children) / (children.Count + 1);
        //        float dy = spaceBetween;

        //        foreach (var childElement in children) {
        //            childElement.Style.Transform(0, dy);
        //            dy = dy + spaceBetween + childElement.OuterRect.Height;
        //        }
        //    }
        //    else {
        //        float spaceBetween = this.Element.WidthRemaining(children) / (children.Count + 1);
        //        float dx = spaceBetween;

        //        foreach (var childElement in children) {
        //            childElement.Style.Transform(dx, 0);
        //            dx = dx + spaceBetween + childElement.OuterRect.Width;
        //        }
        //    }
        //}

        //private void SpaceBetween(List<Element> children) {
        //    if (this.Flex_Axis == Enums.Flex_Axis.Row) {
        //        float spaceBetween = this.Element.WidthRemaining(children) / (children.Count - 1);
        //        float dx = 0;

        //        foreach (var childElement in children) {
        //            childElement.Style.Transform(dx, 0);
        //            dx = dx + spaceBetween + childElement.OuterRect.Width;
        //        }
        //    }
        //    else {
        //        float spaceBetween = this.Element.HeightRemaining(children) / (children.Count - 1);
        //        float dy = 0;

        //        foreach (var childElement in children) {
        //            childElement.Style.Transform(0, dy);
        //            dy = dy + spaceBetween + childElement.OuterRect.Height;
        //        }
        //    }
        //}

        //private void SpaceAround(List<Element> children) {
        //    if (this.Flex_Axis == Enums.Flex_Axis.Row) {
        //        float spaceAround = this.Element.WidthRemaining(children) / (children.Count * 2);
        //        float dx = spaceAround;

        //        foreach (var childElement in children) {
        //            childElement.Style.Transform(dx, 0);
        //            dx = dx + (2 * spaceAround) + childElement.OuterRect.Width;
        //        }
        //    }
        //    else {
        //        float spaceAround = this.Element.HeightRemaining(children) / (children.Count * 2);
        //        float dy = spaceAround;

        //        foreach (var childElement in children) {
        //            childElement.Style.Transform(0, dy);
        //            dy = dy + (2 * spaceAround) + childElement.OuterRect.Height;
        //        }
        //    }
        //}

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

        //private void LineupElements(List<Element> children, PointF from) {
        //    PointF vector;
        //    if (this.Flex_Axis == Enums.Flex_Axis.Column) {
        //        vector = new(0, 1);
        //    }
        //    else {
        //        vector = new(1, 0);
        //    }

        //    PointF current = from;
        //    foreach (Element childElement in children) {
        //        childElement.Style.Transform(current);
        //        var diff = new PointF(childElement.OuterRect.Width, childElement.OuterRect.Height).Scale(vector);
        //        current = current.Translate(diff);
        //    }
        //}

        //private void AlignItems(List<Element> children) {
        //    switch (this.Flex_Axis) {
        //        case Enums.Flex_Axis.Row:
        //            switch (this.Align_Items) {
        //                case Enums.Align_Items.Flex_start:
        //                    break;
        //                case Enums.Align_Items.Flex_end:
        //                    children.ForEach(c => c.Style.Transform(0, this.ContentBox().Height - c.OuterRect.Height));
        //                    break;
        //                case Enums.Align_Items.Center:
        //                    children.ForEach(c => c.Style.Transform(0, (this.ContentBox().Height / 2) - (c.OuterRect.Height / 2)));
        //                    break;
        //            }
        //            break;
        //        case Enums.Flex_Axis.Column:
        //            switch (this.Align_Items) {
        //                case Enums.Align_Items.Flex_start:
        //                    break;
        //                case Enums.Align_Items.Flex_end:
        //                    children.ForEach(c => c.Style.Transform(this.ContentBox().Width - c.OuterRect.Width, 0));
        //                    break;
        //                case Enums.Align_Items.Center:
        //                    children.ForEach(c => c.Style.Transform((this.ContentBox().Width / 2) - (c.OuterRect.Width / 2), 0));
        //                    break;
        //            }
        //            break;
        //    }
        //}

        /// <summary>
        /// Calculates and assigns page numbers to this.s of a given _parent this.Element based on the 
        /// _parent's content Size and the heights of the children. Returns the total page count for 
        /// the _parent this.Element.
        /// </summary>
        /// <param name="this.Element">The _parent this.Element.</param>
        /// <returns>Total page count for all this.s.</returns>
        //private int AssignPages() {
        //    if (this.Element.Style.Overflow != Enums.Overflow.Paged) return 1;
        //    Queue<Element> children = new(this.Element.Children);

        //    int page = -1;

        //    while (children.Count > 0) {
        //        page++;
        //        float heightRemaining = this.ContentBox().Height;
        //        var childElement = children.Dequeue();
        //        heightRemaining -= childElement.OuterRect.Height;
        //        childElement.Style.Page = page;

        //        while (children.Count > 0 && heightRemaining - children.Peek().OuterRect.Height > 0) {
        //            childElement = children.Dequeue();
        //            heightRemaining -= childElement.OuterRect.Height;
        //            childElement.Style.Page = page;
        //        }
        //    }

        //    return page + 1;
        //}
    }

    public static class FlexExt {

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
    }
}
