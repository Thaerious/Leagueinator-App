﻿using Leagueinator.Printer.Elements;
using System.Drawing.Printing;

namespace Leagueinator.Printer.Components {
    public class ElementPrintHandler : PrintDocument {
        private readonly int PageCount;
        private readonly Element Root;
        private int CurrentPage = 0;

        public ElementPrintHandler(Element root) {
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
