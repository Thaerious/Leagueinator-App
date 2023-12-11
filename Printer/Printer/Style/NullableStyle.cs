using Leagueinator.CSSParser;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using Leagueinator.Utility;

namespace Leagueinator.Printer {
    internal class NullableStyle {
        [CSS] public Display? Display = null;
        [CSS] public Position? Position = null;

        [CSS] public PointF? Location = null;
        [CSS] public UnitFloat? Width = null;
        [CSS] public UnitFloat? Height = null;
        [CSS] public Color? BackgroundColor = null;

        [CSS] public Box? Margin = null;
        [CSS] public Box? Padding = null;

        [CSS] public Cardinal<Color>? BorderColor = null;
        [CSS] public Cardinal<UnitFloat>? BorderSize = null;
        [CSS] public Cardinal<DashStyle>? BorderStyle = null;
        [CSS] public string Border { set => this.SetBorder(value); }

        [CSS] public Flex_Direction? Flex_Direction = null;
        [CSS] public Justify_Content? Justify_Content = null;
        [CSS] public Align_Items? Align_Items = null;

        [CSS(true)] public string? FontFamily = null;
        [CSS(true)] public UnitFloat? FontSize = null;
        [CSS(true)] public FontStyle? FontStyle = null;

        public string Selector { get; private set; }

        public NullableStyle(string selector = "") {
            this.Selector = selector;
        }

        /// <summary>
        /// Copy all CSS sourceProperties and sourceFields from source to target.
        /// Will only overwrite sourceFields on target source are null.
        /// </summary>
        /// <param name="source"></param>
        internal static void MergeCSS(object target, object source) {
            PropertyInfo[] properties = source.GetType().GetProperties();
            FieldInfo[] fields = source.GetType().GetFields();

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
        /// Copy from source to this.
        /// Copy all non-null sourceFields and sourceProperties marked with CSS and
        /// inherited is flagged as true.
        /// </summary>
        /// <param name="source"></param>
        public void MergeInheritedCSS(Style source) {
            PropertyInfo[] properties = source.GetType().GetProperties();
            FieldInfo[] fields = source.GetType().GetFields();

            foreach (var sourceProperty in properties) {
                CSS? css = sourceProperty.GetCustomAttribute<CSS>();
                if (css == null) continue;
                if (css.Inherited == false) continue;
                PropertyInfo? targetProperty = this.GetType().GetProperty(sourceProperty.Name);
                if (targetProperty == null) continue;

                if (sourceProperty.CanWrite && sourceProperty.CanRead) {
                    var sourceValue = sourceProperty.GetValue(source);
                    if (sourceValue is null) continue;
                    if (targetProperty.GetValue(this) is not null) continue;
                    targetProperty.SetValue(this, sourceValue);
                }
            }

            foreach (var sourceField in fields) {
                CSS? css = sourceField.GetCustomAttribute<CSS>();
                if (css == null) continue;
                if (css.Inherited == false) continue;
                FieldInfo? targetField = this.GetType().GetField(sourceField.Name);
                if (targetField == null) continue;

                var sourceValue = sourceField.GetValue(source);
                if (sourceValue == null) continue;
                if (targetField.GetValue(this) is not null) continue;
                targetField.SetValue(this, sourceValue);
            }
        }

        public T ToStyle<T>() where T : Style, new(){
            T target = new();

            foreach (var propName in NullableStyle.Properties.Keys) {
                var sourceProp = NullableStyle.Properties[propName];                

                if (sourceProp.GetCustomAttribute<CSS>() == null) continue;
                var targetProp = Style.Properties[propName];

                if (sourceProp.CanRead && targetProp.CanWrite) {
                    var sourceValue = sourceProp.GetValue(this);
                    if (sourceValue == null) continue;
                    targetProp.SetValue(target, sourceValue);
                }
            }

            foreach (var fieldName in NullableStyle.Fields.Keys) {
                var sourceField = NullableStyle.Fields[fieldName];                

                if (sourceField.GetCustomAttribute<CSS>() == null) continue;
                var targetField = Style.Fields[fieldName];

                var sourceValue = sourceField.GetValue(this);
                if (sourceValue == null) continue;
                targetField.SetValue(target, sourceValue);
            }                     

            return target;
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

        static NullableStyle() {
            Fields = typeof(NullableStyle).GetFields().ToDictionary();
            Properties = typeof(NullableStyle).GetProperties().ToDictionary();
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
    }
}
