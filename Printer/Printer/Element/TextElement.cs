using Leagueinator.Utility;

namespace Leagueinator.Printer
{
    public class TextElement : Element
    {
        public string Text = "";

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
            action ??= (element, xml) => { };

            return base.ToXML((e, xml)=> {
                action(this, xml);
                xml.AppendLine(this.Text);
            });
        }

        public override void Draw(Graphics g) {
            base.Draw(g);    
            using Brush brush = new SolidBrush(Color.Black);
            g.DrawString(this.Text, this.Style.Font, brush, this.ContentRect, this.Style.StringFormat);
        }
    }
}
