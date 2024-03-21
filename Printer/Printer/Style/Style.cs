using Leagueinator.CSSParser;
using Leagueinator.Printer.Enums;
using Leagueinator.Printer.Query;
using Leagueinator.Utility;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Text;

namespace Leagueinator.Printer {

    public partial class Style : IComparable<Style> {
        [CSS("Flex")] public Position? Position = null;
        [CSS("Visible")] public Overflow? Overflow = null;

        [CSS("0px, 0px")] public Coordinate<UnitFloat>? Translate = null;
        [CSS] public UnitFloat? Left = null;
        [CSS] public UnitFloat? Right = null;
        [CSS] public UnitFloat? Top = null;
        [CSS] public UnitFloat? Bottom = null;
        [CSS] public UnitFloat? Width = null;
        [CSS] public UnitFloat? Height = null;
        [CSS] public Color? BackgroundColor = null;

        [CSS("0px")] public Cardinal<UnitFloat>? Margin = null;
        [CSS("0px")] public Cardinal<UnitFloat>? Padding = null;

        [CSS] public Cardinal<Color>? BorderColor = null;
        [CSS("0px")] public Cardinal<UnitFloat>? BorderSize;
        [CSS("Solid")] public Cardinal<DashStyle>? BorderStyle;

        [CSS] public string Border {
            get => $"{BorderSize} {BorderStyle} {BorderColor}";
            set => this.SetBorder(value); 
        }

        [CSS("Row")] public Flex_Axis? Flex_Axis = null;
        [CSS("Flex_Start")] public Justify_Content? Justify_Content = null;
        [CSS("Flex_Start")] public Align_Items? Align_Items = null;
        [CSS("Forward")] public Direction? Flex_Direction = null;

        [CSS][Inherited] public string? FontFamily = null;
        [CSS][Inherited] public UnitFloat? FontSize = null;
        [CSS][Inherited] public FontStyle? FontStyle = null;

        public int Page { get; internal set; } = 0;

        public Font Font {
            get {
                string fontFamily = this.FontFamily ?? "Arial";
                float fontSize = this.FontSize ?? new(12, "px");
                FontStyle fontStyle = this.FontStyle ?? System.Drawing.FontStyle.Regular;
                return new(fontFamily, fontSize, fontStyle);
            }
        }

        public StringFormat StringFormat = new() {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };

        public string Selector { get; init; } = "";

        public int[] Specificity { get; init; } = new int[QueryEngine.SPECIFICITY_SIZE];
        public virtual int DoLayout(Element element) {
            this.DoSize(element);
            int pageCount = this.DoPos(element);
            this.AssignInvokes(element);

            return pageCount;
        }
        public virtual void DoSize(Element element) { }
        public virtual int DoPos(Element element) { return 0; }
        public virtual void AssignInvokes(Element element) { }
        public virtual void Draw(Graphics g, Element element, int page) { }

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

        internal static void MergeInheritedStyles(Style target, Style source) {
            foreach (var property in Style.InheritedProperties.Values) {                
                var sourceValue = property.GetValue(source);
                var targetValue = property.GetValue(target);
                if (sourceValue == null || targetValue != null) continue;
                property.SetValue(target, sourceValue);
            }

            foreach (var field in Style.InheritedFields.Values) {
                var sourceValue = field.GetValue(source);
                var targetValue = field.GetValue(target);
                if (sourceValue == null || targetValue != null) continue;
                field.SetValue(target, sourceValue);
            }
        }

        /// <summary>
        /// Copy all CSS properties and fields from source to target.
        /// Will only overwrite null fields on target.
        /// Used to create inhereited style properties.
        /// </summary>
        /// <param name="source"></param>
        internal static void MergeStyles(Style target, Style source) {
            foreach (var property in Style.CSSProperties.Values) {
                var sourceValue = property.GetValue(source);
                var targetValue = property.GetValue(target);
                if (sourceValue == null || targetValue != null) continue;
                property.SetValue(target, sourceValue);
            }

            foreach (var field in Style.CSSFields.Values) {
                var sourceValue = field.GetValue(source);
                var targetValue = field.GetValue(target);
                if (sourceValue == null || targetValue != null) continue;
                field.SetValue(target, sourceValue);
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
            this.BorderSize ??= new(new(1, "px"));
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            PropertyInfo[] properties = this.GetType().GetProperties();
            FieldInfo[] fields = this.GetType().GetFields();

            sb.AppendLine($"Class : {this.GetType()}");
            sb.AppendLine($"Selector : {this.Selector}");
            sb.AppendLine($"Specificity : [{this.Specificity.DelString()}]");
            sb.AppendLine($"Properties : [");

            foreach (var property in properties) {
                CSS? css = property.GetCustomAttribute<CSS>();
                if (css == null) continue;
                if (property.CanRead == false) continue;
                sb.AppendLine($"\t{property.Name} : {property.GetValue(this)}");
            }

            foreach (var field in fields) {
                CSS? css = field.GetCustomAttribute<CSS>();
                if (css == null) continue;
                sb.AppendLine($"\t{field.Name} : {field.GetValue(this)}");
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
