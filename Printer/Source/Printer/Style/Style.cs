﻿using Leagueinator.CSSParser;
using Leagueinator.Printer.Query;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Text;
using Leagueinator.Printer.Elements;
using Leagueinator.Utility;

namespace Leagueinator.Printer.Styles {

    public partial class Style : IComparable<Style> {
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

        public Element? Owner { get; set; }

        public bool Invalid {
            get => this.Owner?.Invalid ?? false;
            internal set {
                if (this.Owner is not null) this.Owner.Invalid = value;
            }
        }

        internal int DoLayout(Element element) {
            Console.WriteLine($"Do Layout For {element.Identifier}");

            this.DoSize(element);
            int pageCount = this.DoPos(element);
            this.AssignInvokes(element);
            element.Invalid = false;

            return pageCount;
        }
        internal virtual void DoSize(Element element) { }
        internal virtual int DoPos(Element element) { return 0; }
        internal virtual void AssignInvokes(Element element) { }
        
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
            sb.AppendLine($"Owner : {(this.Owner != null ? this.Owner.Identifier : "")}");
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
