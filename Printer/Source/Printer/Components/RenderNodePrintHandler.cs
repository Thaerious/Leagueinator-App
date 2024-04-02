using Leagueinator.Printer.Styles;
using System.Drawing.Printing;

namespace Leagueinator.Printer.Components {
    public class RenderNodePrintHandler : PrintDocument {
        private readonly int PageCount;
        private readonly RenderNode Root;
        private int CurrentPage = 0;

        public RenderNodePrintHandler(RenderNode root) {
            this.PrintPage += this.HndPrintPage;
            this.Root = root;
        }

        private void HndPrintPage(object sender, PrintPageEventArgs e) {
            if (this.CurrentPage < this.PageCount) {
                this.Root.Draw(e.Graphics!, this.CurrentPage);
                this.CurrentPage++;
            }
            e.HasMorePages = this.CurrentPage < this.PageCount;
        }
    }
}
