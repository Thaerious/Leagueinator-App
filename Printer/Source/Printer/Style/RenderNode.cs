using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leagueinator.Printer.Styles {
    internal class RenderNode {
        public readonly Style style;

        public SizeF size = new();
        public RectangleF content = new();
        public RectangleF border = new();
        public RectangleF outer = new();

        public RenderNode(Style style) {
            this.style = style;
        }
    }
}
