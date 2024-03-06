using Leagueinator.Utility;
using Printer.Printer;
using System.Drawing;

namespace Leagueinator.Printer
{
    public class TextElement : Element
    {
        public string Text = "";

        public override Element Clone() {
            TextElement clone = new(this.Text) {
                Style = this.Style,
                TagName = this.TagName
            };

            clone.ClassList.AddRange(this.ClassList);

            foreach (Element child in this.Children) {
                clone.AddChild(child.Clone());
            }
            return clone;
        }

        internal override SizeF ContentSize => Size();

        internal override SizeF BorderSize => Size();

        internal override SizeF OuterSize => Size();

        private SizeF Size() {
            if (this.Style == null) return new();
            if (this.Style.Font == null) return new();

            using var bitmap = new Bitmap(1, 1);
            using var graphics = Graphics.FromImage(bitmap);
            return graphics.MeasureString(Text, this.Style.Font);
        }

        public TextElement(string text) : base("@text") {
            this.Text = text.Trim();
        }

        public override XMLStringBuilder ToXML(Action<Element, XMLStringBuilder>? action = null) {
            XMLStringBuilder xml = new();
            xml.AppendLine(this.Text);
            return xml;
        }

        public override void Draw(Graphics g, int page) {
            this.Style.Draw(this, g, page);            
            using Brush brush = new SolidBrush(Color.Black);
            g.DrawString(this.Text, this.Style.Font, brush, this.ContentRect, this.Style.StringFormat);
        }
    }
}
