using Leagueinator.Utility;
using Printer.Printer;
using System.Drawing;

namespace Leagueinator.Printer
{
    public class TextElement : PrinterElement
    {
        public string text = "";

        public override PrinterElement Clone() {
            TextElement clone = new(this.text) {
                Style = this.Style,
                Name = this.Name
            };

            clone.ClassList.AddRange(this.ClassList);

            foreach (PrinterElement child in this.Children) {
                clone.AddChild(child.Clone());
            }
            return clone;
        }

        public override SizeF ContentSize {
            get {
                using var bitmap = new Bitmap(1, 1);
                using var graphics = Graphics.FromImage(bitmap);
                this.Style.FontSize.Factor = 12f;
                return graphics.MeasureString(text, this.Style.Font);
            }
            set { }
        }

        public override SizeF OuterSize {
            get {
                using var bitmap = new Bitmap(1, 1);
                using var graphics = Graphics.FromImage(bitmap);
                this.Style.FontSize.Factor = 12f;
                SizeF size = graphics.MeasureString(text, this.Style.Font);
                return size;
            }
        }

        public TextElement(string text) : base() {
            this.text = text.Trim();
            this.Name = "@text";
        }

        public override XMLStringBuilder ToXML() {
            XMLStringBuilder xml = new();

            xml.OpenTag(this.Name);
            xml.Attribute("width", this.ContentRect.Width);
            xml.Attribute("height", this.ContentRect.Height);
            xml.AppendLine(this.text);
            xml.CloseTag();

            return xml;
        }

        public override void Draw(Graphics g) {
            using Brush brush = new SolidBrush(Color.Black);
            g.DrawString(this.text, this.Style.Font, brush, this.ContentRect, this.Style.StringFormat);
        }
    }
}
