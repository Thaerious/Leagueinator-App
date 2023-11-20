using System.Drawing;

namespace Leagueinator.Printer {
    public class TextElement : PrinterElement {
        public string text = "";

        public override SizeF InnerSize {
            get {
                using var bitmap = new Bitmap(1, 1);
                using var graphics = Graphics.FromImage(bitmap);
                SizeF size = graphics.MeasureString(text, this.Style.Font);
                return size;
            }
            set { }
        }

        public override SizeF OuterSize {
            get {
                using var bitmap = new Bitmap(1, 1);
                using var graphics = Graphics.FromImage(bitmap);
                SizeF size = graphics.MeasureString(text, this.Style.Font);
                return size;
            }
            set { }
        }

        public TextElement(string text) : base() {
            this.text = text.Trim();
            this.Name = "@text";
        }

        public override void Draw(Graphics g) {
            g.DrawString(this.text, this.Style.Font, this.Style.Brush, this.InnerRect, this.Style.StringFormat);
        }
    }
}
