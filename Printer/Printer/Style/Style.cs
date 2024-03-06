using Leagueinator.CSSParser;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Drawing.Drawing2D;

namespace Leagueinator.Printer {
    public enum Flex_Direction { Default, Row, Row_reverse, Column, Column_reverse };
    public enum Justify_Content { Default, Flex_start, Flex_end, Center, Space_between, Space_around, Space_evenly }
    public enum Align_Items { Default, Flex_start, Flex_end, Center }
    public enum Display { Flex, Absolute }
    public enum Direction { Forward, Reverse }
    public enum Position { Static, Relative, Fixed }

    public class BorderColor : Cardinal<Color> {
        public static readonly BorderColor Default = new(Color.Black);

        public BorderColor(Color value) : base(value) { }
        public BorderColor(Color top, Color right, Color bottom, Color left) : base(top, right, bottom, left) { }

        public static bool TryParse(string source, out BorderColor target) {
            if (TryParse(source, out Cardinal<Color> tc)) {
                target = new(tc.Top, tc.Right, tc.Bottom, tc.Left);
                return true;
            }

            target = Default;
            return false;
        }
    }

    public class Box : Cardinal<UnitFloat> {
        public static readonly Box Default = new(new());

        public Box(UnitFloat value) : base(value) { }
        public Box(UnitFloat top, UnitFloat right, UnitFloat bottom, UnitFloat left) : base(top, right, bottom, left) { }

        public static bool TryParse(string source, out Box target) {
            if (TryParse(source, out Cardinal<UnitFloat> tc)) {
                target = new(tc.Top, tc.Right, tc.Bottom, tc.Left);
                return true;
            }

            target = Default;
            return false;
        }
    }

    public class Style {
        [CSS] public Display Display = Display.Flex;
        [CSS] public Position Position = Position.Static;

        [CSS] public PointF Location = new();
        [CSS] public UnitFloat Width = UnitFloat.Default;
        [CSS] public UnitFloat Height = UnitFloat.Default;
        [CSS] public Color? BackgroundColor = null;
        [CSS] public int? Page = default;

        [CSS] public Box Margin = Box.Default;
        [CSS] public Box Padding = Box.Default;

        [CSS] public Cardinal<Color>? BorderColor = null;
        [CSS] public Cardinal<UnitFloat> BorderSize = new(new(0, "px"));
        [CSS] public Cardinal<DashStyle> BorderStyle = new(DashStyle.Solid);
        [CSS] public string Border { set => this.SetBorder(value); }

        [CSS] public Flex_Direction Flex_Direction = Flex_Direction.Default;
        [CSS] public Justify_Content Justify_Content = Justify_Content.Default;
        [CSS] public Align_Items Align_Items = Align_Items.Default;

        [CSS(true)] public string FontFamily = "Arial";
        [CSS(true)] public UnitFloat FontSize = new(12, "px");
        [CSS(true)] public FontStyle FontStyle = FontStyle.Regular;

        public Font Font {
            get {
                return new(this.FontFamily, this.FontSize, this.FontStyle, GraphicsUnit.Point);
            }
        }

        public StringFormat StringFormat = new() {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };

        public virtual void DoSize(Element element) { }
        public virtual void DoPos(Element element) { }
        public virtual void Draw(Element element, Graphics g, int page) { }


        /// <summary>
        /// Copy from source to this.
        /// Copy all non-null sourceFields and sourceProperties marked with CSS and
        /// inherited is flagged as true.
        /// </summary>
        /// <param name="source"></param>
        public Style MergeInheritedWith(Style source) {
            PropertyInfo[] properties = this.GetType().GetProperties();
            FieldInfo[] fields = this.GetType().GetFields();

            foreach (var property in properties) {
                CSS? css = property.GetCustomAttribute<CSS>();
                if (css == null) continue;
                if (css.Inherited == false) continue;

                if (property.CanWrite && property.CanRead) {
                    var sourceValue = property.GetValue(source);
                    if (sourceValue is null) continue;
                    if (property.GetValue(this) is not null) continue;
                    property.SetValue(this, sourceValue);
                }
            }

            foreach (var field in fields) {
                CSS? css = field.GetCustomAttribute<CSS>();
                if (css == null) continue;
                if (css.Inherited == false) continue;

                var sourceValue = field.GetValue(source);
                if (sourceValue == null) continue;
                if (field.GetValue(this) is not null) continue;
                field.SetValue(this, sourceValue);
            }

            return this;
        }

        public Flex_Direction Flex_Major {
            get {
                switch (Flex_Direction) {
                    case Flex_Direction.Default:
                    case Flex_Direction.Row:
                    case Flex_Direction.Row_reverse:
                        return Flex_Direction.Row;
                    default:
                        return Flex_Direction.Column;
                }
            }
        }

        public Direction Flex_Major_Direction {
            get {
                switch (Flex_Direction) {
                    case Flex_Direction.Default:
                    case Flex_Direction.Row:
                    case Flex_Direction.Column:
                        return Direction.Forward;
                    default:
                        return Direction.Reverse;
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
            this.BorderSize ??= new(new(1, "px"));
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            PropertyInfo[] properties = this.GetType().GetProperties();
            FieldInfo[] fields = this.GetType().GetFields();

            sb.AppendLine($"Class : {this.GetType()}");

            foreach (var property in properties) {
                CSS? css = property.GetCustomAttribute<CSS>();
                if (css == null) continue;
                if (property.CanRead == false) continue;
                sb.AppendLine($"{property.Name} : {property.GetValue(this)}");
            }

            foreach (var field in fields) {
                CSS? css = field.GetCustomAttribute<CSS>();
                if (css == null) continue;
                sb.AppendLine($"{field.Name} : {field.GetValue(this)}");
            }
            return sb.ToString();
        }

        public static IReadOnlyDictionary<string, FieldInfo> Fields { get; private set; }
        public static IReadOnlyDictionary<string, PropertyInfo> Properties { get; private set; }

        static Style() {
            Fields = typeof(Style).GetFields().ToDictionary();
            Properties = typeof(Style).GetProperties().ToDictionary();
        }
    }
}
