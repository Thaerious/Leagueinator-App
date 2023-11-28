using Leagueinator.CSSParser;
using Leagueinator.Utility;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Text;

namespace Leagueinator.Printer {
    public enum Flex_Direction { Row, Row_reverse, Column, Column_reverse };
    public enum Justify_Content { Flex_start, Flex_end, Center, Space_between, Space_around, Space_evenly }
    public enum Align_Items { Flex_start, Flex_end, Center }
    public enum Display { Flex, Absolute }
    public enum Direction { Forward, Reverse }
    public enum Position { Static, Relative, Fixed }

    public class Style {
        [CSS] public Display Display = Display.Flex;
        [CSS] public Position Position = Position.Static;

        [CSS] public PointF Location = new();
        [CSS] public Func<float, float?> Width = f => null;
        [CSS] public Func<float, float?> Height = f => null;
        [CSS] public Color? BackgroundColor = null;

        [CSS] public Cardinal<float> Margin = new(0f);
        [CSS] public Cardinal<float> Padding = new(0f);
        [CSS] public Cardinal<Color> BorderColor = new();
        [CSS] public Cardinal<float> BorderSize = new();

        [CSS] public Flex_Direction Flex_Direction = Flex_Direction.Row;
        [CSS] public Justify_Content Justify_Content = Justify_Content.Flex_start;
        [CSS] public Align_Items Align_Items = Align_Items.Flex_start;

        [CSS] public Font Font = new("Arial", 12, FontStyle.Bold, GraphicsUnit.Point);
        [CSS] public Brush Brush = new SolidBrush(Color.Black);
        [CSS] public Pen Pen = new(Color.Black);
        [CSS] public StringFormat StringFormat = new StringFormat {
            Alignment = StringAlignment.Near,
            LineAlignment = StringAlignment.Near
        };

        public string Selector { get; init; }

        public Style() : this("") { }

        public Style(string Selector) {
            this.Selector = Selector;
        }

        public Style(Style that) {
            this.Selector = that.Selector;

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
        /// Copy all non-null public properties from that to this.
        /// </summary>
        /// <param name="that"></param>
        public Style MergeWith(Style that) {
            PropertyInfo[] properties = this.GetType().GetProperties();
            FieldInfo[] fields = this.GetType().GetFields();

            foreach (var property in properties) {
                if (property.CanWrite && property.CanRead && property.Name != "FallBack") {
                    var value = property.GetValue(that);
                    if (value == null) continue;
                    property.SetValue(this, value);
                }
                foreach (var field in fields) {
                    var value = field.GetValue(that);
                    if (value == null) continue;
                    field.SetValue(this, value);
                }
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

        public static Dictionary<string, FieldInfo> Fields { get; }  = new();

        static Style() {
            foreach (FieldInfo field in typeof(Style).GetFields()) {
                CSS? css = field.GetCustomAttribute<CSS>();
                if (css == null) continue;
                var key = css.Key?.ToLower() ?? field.Name.ToPlainCase();
                Fields[key] = field;
                Debug.WriteLine($"{key} = {field}");
            }
        }
        private Font? _font = null;
        private Brush? _brush = null;
        private Pen? _pen = null;
        private StringFormat? _stringFormat = null;
    }
}
