using Leagueinator.CSSParser;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using Leagueinator.Utility.Seek;

namespace Leagueinator.Printer {
    public enum Flex_Direction { Default, Row, Row_reverse, Column, Column_reverse };
    public enum Justify_Content { Default, Flex_start, Flex_end, Center, Space_between, Space_around, Space_evenly }
    public enum Align_Items { Default, Flex_start, Flex_end, Center }
    public enum Display { Flex, Absolute }
    public enum Direction { Forward, Reverse }
    public enum Position { Static, Relative, Fixed }

    public class BorderSize : Cardinal<UnitFloat> {
        public static readonly BorderSize Default = new(new(1f, "px"));

        public BorderSize(UnitFloat value) : base(value) { }
        public BorderSize(UnitFloat top, UnitFloat right, UnitFloat bottom, UnitFloat left) : base(top, right, bottom, left) { }

        public static bool TryParse(string source, out BorderSize target) {
            if (TryParse(source, out Cardinal<UnitFloat> tc)) {
                target = new(tc.Top, tc.Right, tc.Bottom, tc.Left);
                return true;
            }

            target = Default;
            return false;
        }
    }

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

    public class BorderStyle : Cardinal<DashStyle> {
        public static readonly BorderStyle Default = new(DashStyle.Solid);

        public BorderStyle(DashStyle value) : base(value) { }
        public BorderStyle(DashStyle top, DashStyle right, DashStyle bottom, DashStyle left) : base(top, right, bottom, left) { }

        public static bool TryParse(string source, out BorderStyle target) {
            if (TryParse(source, out Cardinal<DashStyle> tc)) {
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

    public class FontSize : IHasDefault<UnitFloat> {
        public static readonly FontSize Default = new();
        public UnitFloat Value { get; set; } = new UnitFloat(12, "px");

        public static bool TryParse(string source, out FontSize target) {
            if (UnitFloat.TryParse(source, out UnitFloat unitFloat)){
                target = new() { Value = unitFloat };
                return true;    
            }

            target = Default;
            return false;
        }
    }

    public class FontFamilyDefault : IHasDefault<FontFamily> {
        public static readonly FontFamilyDefault Default = new();
        public FontFamily Value { get; set; } = new FontFamily("Arial");

        public static bool TryParse(string source, out FontFamilyDefault target) {
            target = new FontFamilyDefault() {
                Value = new FontFamily(source)
            };
            return true;
        }
    }

    public class FontStyleDefault : IHasDefault<FontStyle> {
        public static readonly FontStyleDefault Default = new();
        public FontStyle Value { get; set; } = FontStyle.Regular;

        public static bool TryParse(string source, out FontStyleDefault target) {
            object? fontStyle = FontStyle.Regular;

            if (MultiParse.ParseEnum(source, typeof(FontStyle), ref fontStyle)) {                
                if (fontStyle == null) target = new();
                else target = new() { Value = (FontStyle)fontStyle };
                return true;
            }

            target = new();
            return false;
        }
    }

    public class NullableStyle {
        [CSS] public Display? Display = null;
        [CSS] public Position? Position = null;

        [CSS] public PointF? Location = null;
        [CSS] public UnitFloat? Width = null;
        [CSS] public UnitFloat? Height = null;
        [CSS] public Color? BackgroundColor = null;

        [CSS] public Box? Margin = Box.Default;
        [CSS] public Box? Padding = Box.Default;

        [CSS] public BorderColor? BorderColor = BorderColor.Default;
        [CSS] public BorderSize? BorderSize = BorderSize.Default;
        [CSS] public BorderStyle? BorderStyle = BorderStyle.Default;

        [CSS] public Flex_Direction? Flex_Direction = null;
        [CSS] public Justify_Content? Justify_Content = null;
        [CSS] public Align_Items? Align_Items = null;

        [CSS(true)] public FontFamilyDefault FontFamily = FontFamilyDefault.Default;
        [CSS(true)] public FontSize FontSize = FontSize.Default;
        [CSS(true)] public FontStyleDefault FontStyle = FontStyleDefault.Default;


        public class DefaultStyle : Style {
        [CSS] new public PointF Location = new();
        [CSS] new public UnitFloat Width = UnitFloat.Default;
        [CSS] new public UnitFloat Height = UnitFloat.Default;
    }

    public class Style {
        [CSS] public Display Display = Display.Flex;
        [CSS] public Position Position = Position.Static;

        [CSS] public PointF? Location = null;
        [CSS] public UnitFloat? Width = null;
        [CSS] public UnitFloat? Height = null;
        [CSS] public Color? BackgroundColor = null;

        [CSS] public Box Margin = Box.Default;
        [CSS] public Box Padding = Box.Default;

        [CSS] public BorderColor BorderColor = BorderColor.Default;
        [CSS] public BorderSize BorderSize = BorderSize.Default;
        [CSS] public BorderStyle BorderStyle = BorderStyle.Default;
        [CSS] public string Border { set => this.SetBorder(value); }

        [CSS] public Flex_Direction Flex_Direction = Flex_Direction.Default;
        [CSS] public Justify_Content Justify_Content = Justify_Content.Default;
        [CSS] public Align_Items Align_Items = Align_Items.Default;

        [CSS(true)] public FontFamilyDefault FontFamily = FontFamilyDefault.Default;
        [CSS(true)] public FontSize FontSize = FontSize.Default;
        [CSS(true)] public FontStyleDefault FontStyle = FontStyleDefault.Default;

        public Font Font {
            get {
                return new(this.FontFamily.Value, this.FontSize.Value, this.FontStyle.Value, GraphicsUnit.Point);
            }
        }

        public StringFormat StringFormat = new() {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };

        public string Selector { get; private set; }

        public Style() : this("") { }

        public Style(string Selector) {
            this.Selector = Selector;
        }

        public Style(Style that) : this(that.Selector) {
            foreach (FieldInfo field in typeof(Style).GetFields()) {
                CSS? css = field.GetCustomAttribute<CSS>();
                if (css == null) continue;
                var value = field.GetValue(that);
                field.SetValue(this, value);
            }
        }

        public virtual void DoSize(PrinterElement element) { }
        public virtual void DoLayout(PrinterElement element) { }
        public virtual void DoDraw(PrinterElement element, Graphics g) { }


        /// <summary>
        /// Copy from that to this.
        /// Copy all non-null fields and properties marked with CSS and
        /// inherited is flagged as true.
        /// </summary>
        /// <param name="that"></param>
        public Style MergeInheritedWith(Style that) {
            PropertyInfo[] properties = this.GetType().GetProperties();
            FieldInfo[] fields = this.GetType().GetFields();

            foreach (var property in properties) {
                CSS? css = property.GetCustomAttribute<CSS>();
                if (css == null) continue;
                if (css.Inherited == false) continue;

                if (property.CanWrite && property.CanRead) {
                    var value = property.GetValue(that);
                    if (value == null) continue;
                    property.SetValue(this, value);
                }
            }

            foreach (var field in fields) {
                CSS? css = field.GetCustomAttribute<CSS>();
                if (css == null) continue;
                if (css.Inherited == false) continue;

                var value = field.GetValue(that);
                if (value == null) continue;
                field.SetValue(this, value);
            }

            return this;
        }

        /// <summary>
        /// Copy all CSS properties and fields from that to this.
        /// Will overwrite fields on this that are null.
        /// Will not overwrite fields on this that are non-null.
        /// </summary>
        /// <param name="that"></param>
        public Style MergeWith(Style that) {
            PropertyInfo[] properties = this.GetType().GetProperties();
            FieldInfo[] fields = this.GetType().GetFields();

            this.Selector = $"{this.Selector.Trim()} {that.Selector.Trim()}";

            foreach (var property in properties) {
                if (property.GetCustomAttribute<CSS>() == null) continue;
                if (property.CanWrite && property.CanRead) {
                    var thatValue = property.GetValue(that);
                    var thisValue = property.GetValue(this);
                    if (thatValue == null || thisValue != null) continue;
                    if (IsEnumDefault(thatValue)) continue;
                    if (IsObjectDefault(thatValue)) continue;
                    property.SetValue(this, thatValue);
                }
            }

            foreach (var field in fields) {
                if (field.GetCustomAttribute<CSS>() == null) continue;
                var thatValue = field.GetValue(that);
                var thisValue = field.GetValue(this);
                if (thatValue == null || thisValue != null) continue;
                if (IsEnumDefault(thatValue)) continue;
                if (IsObjectDefault(thatValue)) continue;
                field.SetValue(this, thatValue);
            }

            return this;
        }

        private static bool IsEnumDefault(object value) {
            if (!value.GetType().IsEnum) return false;
            if (value.ToString()?.ToLower() == "default") return true;
            return false;
        }

        private static bool IsObjectDefault(object value) {
            var type = value.GetType();
            var field = type.GetField("Default", BindingFlags.Public | BindingFlags.Static);
            if (field == null) return false;
            if (field.GetValue(null) == value) return true;
            return false;
        }

        private bool CheckHasValue(object value) {
            if (value is IHasValue hasValue) {
                return hasValue.HasValue;
            }
            return true;
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

            sb.AppendLine($"Selector : {this.Selector}");
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
