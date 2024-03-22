﻿using Leagueinator.Printer;
using System.Diagnostics;
using System.Drawing.Printing;

namespace Leagueinator.PrinterComponents {
    public class ElementPrintHandler : PrintDocument {
        private readonly int PageCount;
        private readonly Element Root;
        private int CurrentPage = 0;

        public ElementPrintHandler(Element root) {
            this.PageCount = root.Style.DoLayout(root);
            this.PrintPage += this.HndPrintPage;
            this.Root = root;
        }

        private void HndPrintPage(object sender, PrintPageEventArgs e) {
            if (CurrentPage < this.PageCount) {
                this.Root.Style.Draw(e.Graphics!, this.Root, this.CurrentPage);
                CurrentPage++;
            }
            e.HasMorePages = CurrentPage < this.PageCount;
        }
    }
}
