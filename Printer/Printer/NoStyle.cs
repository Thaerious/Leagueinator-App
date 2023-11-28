using Printer.Printer;
using System.Drawing;

namespace Leagueinator.Printer {
    internal class NoStyle : Style {
        public override void DoSize(PrinterElement element) {
            foreach (PrinterElement child in element.Children) {
                child.Style.DoSize(child);
            }
        }

        public override void DoLayout(PrinterElement element) {
            foreach (PrinterElement child in element.Children) {
                child.Style.DoLayout(child);
            }
        }

        public override void DoDraw(PrinterElement element, Graphics g) {
            foreach (PrinterElement child in element.Children) {
                child.Style.DoDraw(child, g);
            }
        }
    }
}
