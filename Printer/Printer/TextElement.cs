using System.Drawing;

namespace Leagueinator.Printer {
    public class TextElement : PrinterElement {
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
                return graphics.MeasureString(text, this.Style.Font);
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
        }

        public TextElement(string text) : base() {
            this.text = text.Trim();
            this.Name = "@text";
        }

        public override void Draw(Graphics g) {
            g.DrawString(this.text, this.Style.Font, this.Style.Brush, this.ContentRect, this.Style.StringFormat);
        }
    }
}
