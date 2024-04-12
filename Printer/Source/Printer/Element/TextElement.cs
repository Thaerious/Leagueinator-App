using Leagueinator.Printer.Aspects;
using Leagueinator.Utility;

namespace Leagueinator.Printer.Elements {
    internal class TextElement : Element {
        public static readonly string TAG_NAME = "@text";

        public TextElement(string text) : base(TAG_NAME, []) {
            this.Text = text.Trim();
        }

        public string Text { get; [Validated] set; } = "";

        public SizeF Size() {
            if (this.Style == null) return new();
            if (this.Style.Font == null) return new();

            using var bitmap = new Bitmap(1, 1);
            using var graphics = Graphics.FromImage(bitmap);
            return graphics.MeasureString(this.Text, this.Style.Font);
        }


        public override XMLStringBuilder ToXML(Action<Element, XMLStringBuilder>? action = null) {
            action ??= (element, xml) => { };
            return new XMLStringBuilder().AppendLine(this.Text);
        }
    }
}
