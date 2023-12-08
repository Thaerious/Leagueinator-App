using Leagueinator.Utility;
using Printer.Printer;
using System.Drawing;

namespace Leagueinator.Printer
{
    public class TextElement : PrinterElement
    {
        public string Text = "";

        public override PrinterElement Clone() {
            TextElement clone = new(this.Text) {
                Style = this.Style,
                Name = this.Name
            };

            clone.ClassList.AddRange(this.ClassList);

            foreach (PrinterElement child in this.Children) {
                clone.AddChild(child.Clone());
            }
            return clone;
        }

        public override SizeF ContentSize => Size();

        public override SizeF BorderSize => Size();

        public override SizeF OuterSize => Size();

        private SizeF Size() {
            if (this.Style == null) return new();
            if (this.Style.Font == null) return new();

            using var bitmap = new Bitmap(1, 1);
            using var graphics = Graphics.FromImage(bitmap);
            return graphics.MeasureString(Text, this.Style.Font);
        }

        public TextElement(string text) : base() {
            this.Text = text.Trim();
            this.Name = "@text";
        }

        public override XMLStringBuilder ToXML() {
            XMLStringBuilder xml = new();

            xml.AppendLine(this.Text);

            return xml;
        }

        public override void Draw(Graphics g) {
            this.Style.DoDraw(this, g);            
            using Brush brush = new SolidBrush(Color.Black);
            g.DrawString(this.Text, this.Style.Font, brush, this.ContentRect, this.Style.StringFormat);
        }
    }
}
