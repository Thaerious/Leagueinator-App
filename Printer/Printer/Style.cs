using Leagueinator.CSSParser;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Drawing.Drawing2D;
using Printer;
using System.Diagnostics;

namespace Leagueinator.Printer {
    public enum Flex_Direction { Row, Row_reverse, Column, Column_reverse };
    public enum Justify_Content { Flex_start, Flex_end, Center, Space_between, Space_around, Space_evenly }
    public enum Align_Items { Flex_start, Flex_end, Center }
    public enum Display { Flex, Absolute }
    public enum Direction { Forward, Reverse }
    public enum Position { Static, Relative, Fixed }

    public class Style {
        public static Style DefaultStyle = new (){
            FontFamily = "Ariel",
            FontSize = new (12, "px"),
            FontStyle = FontStyle.Regular
        };

        [CSS] public Display Display = Display.Flex;
        [CSS] public Position Position = Position.Static;

        [CSS] public PointF Location = new();
        [CSS] public UnitFloat Width = new();
        [CSS] public UnitFloat Height = new();
        [CSS] public Color? BackgroundColor = null;

        [CSS] public Cardinal<UnitFloat> Margin = new(new UnitFloat());
        [CSS] public Cardinal<UnitFloat> Padding = new(new UnitFloat());

        [CSS] public Cardinal<Color> BorderColor = new();
        [CSS] public Cardinal<UnitFloat> BorderSize = new(new UnitFloat());
        [CSS] public Cardinal<DashStyle> BorderStyle = new();
        [CSS] public string Border { set => this.SetBorder(value); }

        [CSS] public Flex_Direction Flex_Direction = Flex_Direction.Row;
        [CSS] public Justify_Content Justify_Content = Justify_Content.Flex_start;
        [CSS] public Align_Items Align_Items = Align_Items.Flex_start;

        [CSS(true)] public string FontFamily;
        [CSS(true)] public UnitFloat FontSize;
        [CSS(true)] public FontStyle FontStyle;

        public Font Font {
            get {
                return new(this.FontFamily, this.FontSize, this.FontStyle, GraphicsUnit.Point);
            }
        }

        public StringFormat StringFormat = new() {
            Alignment = StringAlignment.Near,
            LineAlignment = StringAlignment.Near
        };

        public string Selector { get; init; }

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
                    Debug.WriteLine($"{property.Name} {value.ToString()}");
                }
            }

            foreach (var field in fields) {
                CSS? css = field.GetCustomAttribute<CSS>();
                if (css == null) continue;
                if (css.Inherited == false) continue;

                var value = field.GetValue(that);
                if (value == null) continue;
                field.SetValue(this, value);
                Debug.WriteLine($"{field.Name} {value.ToString()}");
            }

            return this;
        }

        /// <summary>
        /// Copy from that to this.
        /// Copy all non-null public properties from that to this.
        /// </summary>
        /// <param name="that"></param>
        public Style MergeWith(Style that) {            
            PropertyInfo[] properties = this.GetType().GetProperties();
            FieldInfo[] fields = this.GetType().GetFields();

            foreach (var property in properties) {
                if (property.GetCustomAttribute<CSS>() == null) continue;
                if (property.CanWrite && property.CanRead) {
                    var value = property.GetValue(that);
                    if (value == null) continue;
                    property.SetValue(this, value);
                }
            }

            foreach (var field in fields) {
                if (field.GetCustomAttribute<CSS>() == null) continue;
                var value = field.GetValue(that);
                if (value == null) continue;
                field.SetValue(this, value);
            }

            return this;
        }

        public Flex_Direction Flex_Major {
            get {
                switch (Flex_Direction) {
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
                        this.BorderColor = new Cardinal<Color>((Color)color);
                    }
                }
                else if (MultiParse.TryParse(s, out DashStyle? style)) {
                    if (style != null) {
                        this.BorderStyle = new Cardinal<DashStyle>((DashStyle)style);
                    }
                }
                else if (MultiParse.TryParse(s, out UnitFloat? width)) {
                    if (width != null) {
                        this.BorderSize = new Cardinal<UnitFloat>((UnitFloat)width);
                    }
                }
            }
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            PropertyInfo[] properties = this.GetType().GetProperties();
            FieldInfo[] fields = this.GetType().GetFields();

            foreach (var property in properties) {
                if (property.CanWrite && property.CanRead && property.Name != "FallBack") {
                    var value = property.GetValue(this);
                    if (value == null) continue;
                    sb.AppendLine($"{property.Name} : {value}");
                }
            }
            foreach (var field in fields) {
                var value = field.GetValue(this);
                if (value == null) continue;
                sb.AppendLine($"{field.Name} : {value}");
            }
            return sb.ToString();
        }

        public static LCDictionary<FieldInfo> Fields { get; } = new();
        public static LCDictionary<PropertyInfo> Properties { get; } = new();

        static Style() {
            Fields = typeof(Style).GetFields().LCDictionary();
            Properties = typeof(Style).GetProperties().LCDictionary();
        }
    }
}
