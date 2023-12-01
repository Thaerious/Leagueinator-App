using Leagueinator.Utility;
using Printer.Printer;
using System.Drawing;

namespace Leagueinator.Printer {
    public class RootElement : PrinterElement {
        public delegate SizeF Source();
        public Source RootSource { get; set; }

        public RootElement(Source source) {
            this.RootSource = source;
            this.Name = "@document";
            this.Style = new NoStyle();
        }

        public override SizeF ContentSize {
            get => this.RootSource();
            set => throw new NotImplementedException();
        }

        public override SizeF BorderSize {
            get => this.RootSource();
            set => throw new NotImplementedException();
        }

        public override SizeF OuterSize {
            get => this.RootSource();
        }

        public override PointF Location => new();

        public override XMLStringBuilder ToXML() {
            XMLStringBuilder xml = new();

            xml.OpenTag(this.Name);
            xml.Attribute("width", this.RootSource().Width);
            xml.Attribute("height", this.RootSource().Height);
            foreach (var child in this.Children) {
                xml.AppendXML(child.ToXML());
            }
            xml.CloseTag();

            return xml;
        }

    }
}
