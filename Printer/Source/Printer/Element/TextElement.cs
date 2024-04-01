using Leagueinator.Printer.Aspects;
using Leagueinator.Utility;

namespace Leagueinator.Printer.Elements {
    internal class TextElement : Element {
        public static readonly string TAG_NAME = "@text";

        public TextElement(string text) : base(TAG_NAME, []) {
            this.Text = text.Trim();
            this.OnDraw += this.TextElement_OnDraw;
        }

        private void TextElement_OnDraw(Graphics g, Element element, int page) {            
            //using Brush brush = new SolidBrush(Color.Black);
            //Debug.WriteLine($"Draw String {this.Style.ContentBox()}");
            //g.DrawRectangle(new Pen(Color.Black, 1), this.Style.ContentBox());
            //g.DrawString(this.Text, this.Style.Font, brush, this.Style.ContentBox(), this.Style.StringFormat);
        }

        public string Text { get; [Validated] set; } = "";

        internal SizeF Size() {
            if (this.Style == null) return new();
            if (this.Style.Font == null) return new();

            using var bitmap = new Bitmap(1, 1);
            using var graphics = Graphics.FromImage(bitmap);
            return graphics.MeasureString(this.Text, this.Style.Font);
        }


        public override XMLStringBuilder ToXML(Action<Element, XMLStringBuilder>? action = null) {
            action ??= (element, xml) => { };

            return base.ToXML((e, xml) => {
                action(this, xml);
                xml.AppendLine(this.Text);
            });
        }
    }
}
