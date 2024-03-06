using Printer.Printer;
using System.Drawing;

namespace Leagueinator.Printer {
    internal class NoStyle : Style {
        public override void DoSize(Element element) {
            foreach (Element child in element.Children) {
                child.Style.DoSize(child);
            }
        }

        public override void DoPos(Element element) {
            foreach (Element child in element.Children) {
                child.Style.DoPos(child);
            }
        }

        public override void Draw(Element element, Graphics g, int page) {
            foreach (Element child in element.Children) {
                child.Style.Draw(child, g, page);
            }
        }
    }
}
