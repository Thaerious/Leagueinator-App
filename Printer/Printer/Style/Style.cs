using Leagueinator.CSSParser;
using System.Reflection;
using System.Text;
using System.Drawing.Drawing2D;
using Leagueinator.Printer.Enums;

namespace Leagueinator.Printer {
    public partial class Style {
        [CSS("flex")] public Display? Display = null;

        [CSS("0, 0", "SetLocation")] public PointF? Location = null;
        [CSS] public UnitFloat? Width = null;
        [CSS] public UnitFloat? Height = null;
        [CSS] public Color? BackgroundColor = null;

        [CSS] public Box Margin = Box.Default;
        [CSS] public Box Padding = Box.Default;

        [CSS] public Cardinal<Color>? BorderColor = null;
        [CSS] public Cardinal<UnitFloat>? BorderSize = new(new(0, "px"));
        [CSS] public Cardinal<DashStyle>? BorderStyle = new(DashStyle.Solid);
        [CSS] public string Border { set => this.SetBorder(value); }

        [CSS("Default")] public Flex_Axis? Flex_Axis = null;
        [CSS("Default")] public Justify_Content? Justify_Content = null;
        [CSS("Default")] public Align_Items? Align_Items = null;
        [CSS("Forward")] public Direction? Flex_Direction = null;

        [CSS][Inherited] public string? FontFamily = null;
        [CSS][Inherited] public UnitFloat? FontSize = null;
        [CSS][Inherited] public FontStyle? FontStyle = null;

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

        public string Selector { get; internal set; }

        public Style(string selector = "") {
            this.Selector = selector;
        }

        public virtual void DoLayout(Element element) {
            this.DoSize(element);
            this.DoPos(element);
            this.AssignInvokes(element);
        }

        public virtual void DoSize(Element element) { }
        public virtual void DoPos(Element element) { }
        public virtual void AssignInvokes(Element element) { }
        public virtual void Draw(Graphics g, Element element) { }
        public virtual void DrawPage(Graphics g, Element element, int page) { }

        public Enums.Direction Flex_Major_Direction {
            get {
                switch (Flex_Axis) {
                    case Enums.Flex_Axis.Default:
                    case Enums.Flex_Axis.Row:
                    case Enums.Flex_Axis.Column:
                        return Enums.Direction.Forward;
                    default:
                        return Enums.Direction.Reverse;
                }
            }
        }

        /// <summary>
        /// Copy all CSS properties and fields from source to target.
        /// Will only overwrite null fields on target.
        /// Used to create inhereited style properties.
        /// </summary>
        /// <param name="source"></param>
        internal static void MergeStyles(Style target, Style source) {
            PropertyInfo[] properties = source.GetType().GetProperties();
            FieldInfo[] fields = source.GetType().GetFields();

            target.Selector += " " + source.Selector;

            foreach (var property in properties) {
                if (property.GetCustomAttribute<CSS>() == null) continue;
                if (property.CanWrite && property.CanRead) {
                    var sourceValue = property.GetValue(source);
                    var targetValue = property.GetValue(target);
                    if (sourceValue == null || targetValue != null) continue;
                    property.SetValue(target, sourceValue);
                }
            }

            foreach (var field in fields) {
                if (field.GetCustomAttribute<CSS>() == null) continue;
                var sourceValue = field.GetValue(source);
                var targetValue = field.GetValue(target);
                if (sourceValue == null || targetValue != null) continue;
                field.SetValue(target, sourceValue);
            }
        }

        /// <summary>
        /// Create a new style from this style.
        /// Copies all properties to the new style.
        /// Used to create a specific style after the source has been read.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T ToStyle<T>() where T : Style, new(){
            T target = new();

            foreach (var propName in Style.Properties.Keys) {
                var sourceProp = Style.Properties[propName];                

                if (sourceProp.GetCustomAttribute<CSS>() == null) continue;
                var targetProp = Style.Properties[propName];

                if (sourceProp.CanRead && targetProp.CanWrite) {
                    var sourceValue = sourceProp.GetValue(this);
                    if (sourceValue == null) continue;
                    targetProp.SetValue(target, sourceValue);
                }
            }

            foreach (var fieldName in Style.Fields.Keys) {
                var sourceField = Style.Fields[fieldName];                

                if (sourceField.GetCustomAttribute<CSS>() == null) continue;
                var targetField = Style.Fields[fieldName];

                var sourceValue = sourceField.GetValue(this);
                if (sourceValue == null) continue;
                targetField.SetValue(target, sourceValue);
            }                     

            return target;
        }

        public bool SetLocation(string source) {
            var args = source.Split(',')
                             .Select(s => int.Parse(s))
                             .ToArray();

            this.Location = new Point(args[0], args[1]);
            return true;
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

        public static IReadOnlyDictionary<string, FieldInfo> Fields { get; private set; }
        public static IReadOnlyDictionary<string, PropertyInfo> Properties { get; private set; }

        static Style() {
            Fields = typeof(Style).GetFields().ToDictionary();
            Properties = typeof(Style).GetProperties().ToDictionary();
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            PropertyInfo[] properties = this.GetType().GetProperties();
            FieldInfo[] fields = this.GetType().GetFields();

            sb.AppendLine($"Class : {this.GetType()}");
            sb.AppendLine($"Selector : {this.Selector}");
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
    }
}
