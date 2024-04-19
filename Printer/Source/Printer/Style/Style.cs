using Leagueinator.CSSParser;
using Leagueinator.Printer.Query;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Text;
using Leagueinator.Printer.Elements;
using Leagueinator.Utility;

namespace Leagueinator.Printer.Styles {

    public partial class Style(Element? owner) : IComparable<Style> {
        public Element? Element { get; } = owner;

        public Font Font {
            get {
                string fontFamily = this.FontFamily ?? "Arial";
                this.FontSize ??= new() { Factor = 12, Unit = "px" };
                //this.FontSize.ApplySource(1);
                FontStyle fontStyle = this.FontStyle ?? System.Drawing.FontStyle.Regular;
                return new(fontFamily, 12, fontStyle);
            }
        }

        public StringFormat StringFormat = new() {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };

        public string Selector { get; init; } = "";

        public int[] Specificity { get; init; } = new int[QueryEngine.SPECIFICITY_SIZE];

        public Element? Owner { get; set; }

        public bool Invalid {
            get => this.Owner?.Invalid ?? false;
            internal set {
                if (this.Owner is not null) this.Owner.Invalid = value;
            }
        }
        
        public Enums.Direction Flex_Major_Direction {
            get {
                switch (this.Flex_Axis) {
                    case Enums.Flex_Axis.Row:
                    case Enums.Flex_Axis.Column:
                        return Enums.Direction.Forward;
                    default:
                        return Enums.Direction.Reverse;
                }
            }
        }

        private void SetBorder(string source) {
            foreach (string s in source.Split(' ')) {
                if (MultiParse.TryParse(s, out Color? color)) {
                    if (color != null) {
                        this.BorderColor = new((Color)color);
                    }
                }
                else if (MultiParse.TryParse(s, out DashStyle? style)) {
                    if (style != null) {
                        this.BorderStyle = new((DashStyle)style);
                    }
                }
                else if (MultiParse.TryParse(s, out UnitFloat? width)) {
                    if (width != null) {
                        this.BorderSize = new(width);
                    }
                }
            }

            this.BorderColor ??= new(Color.Black);
            this.BorderStyle ??= new(DashStyle.Solid);
            this.BorderSize ??= new(new UnitFloat() { Factor = 1, Unit = "px"});
        }

        public override string ToString() {
            return this.ToString(true);
        }

        public string ToString(bool printNulls) {
            StringBuilder sb = new StringBuilder();
            PropertyInfo[] properties = this.GetType().GetProperties();
            FieldInfo[] fields = this.GetType().GetFields();

            sb.AppendLine($"Class : {this.GetType()}");
            sb.AppendLine($"Selector : {this.Selector}");
            sb.AppendLine($"Owner : {(this.Owner != null ? this.Owner.Identifier : "")}");
            sb.AppendLine($"Specificity : [{this.Specificity.DelString()}]");

            sb.AppendLine($"Sources : [");
            foreach (Style style in this.SourceStyles) {
                sb.AppendLine($"\t'{style.Selector}'");
            }
            sb.AppendLine($"]");

            sb.AppendLine($"Properties : [");

            foreach (var property in properties) {
                CSS? css = property.GetCustomAttribute<CSS>();
                if (css == null) continue;
                if (property.CanRead == false) continue;
                var value = property.GetValue(this);
                if (value != null || printNulls == true) {
                    sb.AppendLine($"\t{property.Name} : {property.GetValue(this)}");
                }
            }

            sb.AppendLine($"]");
            return sb.ToString();
        }

        public int CompareTo(Style? other) {
            if (other == null) return 1;
            for (int i = 0; i < this.Specificity.Length; i++) {
                if (this.Specificity[i] == other.Specificity[i]) continue;
                return other.Specificity[i].CompareTo(this.Specificity[i]);
            }

            return 0;
        }
    }
}
