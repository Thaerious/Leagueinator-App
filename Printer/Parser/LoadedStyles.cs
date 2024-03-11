using Leagueinator.Printer;
using System.Reflection;

namespace Leagueinator.CSSParser {
    public class LoadedStyles : Dictionary<string, Style> {

        /// <summary>
        /// Using the loaded styles sheet, apply styles to root and all child nodes
        /// recursivly.  Replaces the parent styles on the xChild.
        /// </summary>
        public void ApplyTo(Element root) {
            Queue<Element> queue = new();
            queue.Enqueue(root);

            while (queue.Count > 0) {
                Element current = queue.Dequeue();

                this.ApplyNameStyles(current, current.Style);
                this.ApplyClassStyles(current, current.Style);
                this.ApplyIDStyles(current, current.Style);
                this.ApplyWildcardStyles(current, current.Style);
                this.ApplyParentStyles(current, current.Style);
                this.ApplyDefaultValues(current.Style);

                foreach (Element child in current.Children) {
                    queue.Enqueue(child);
                }
            }
        }

        internal void ApplyParentStyles(Element element, Style style) {
            // TODO optimze by going root up.
            Element? parent = element.Parent;

            if (parent != null) {
                MemberInfo[] methods = [.. typeof(Style).GetFields(), .. typeof(Style).GetProperties()];

                foreach (MemberInfo member in methods) {
                    InheritedAttribute? attr = member.GetCustomAttribute<InheritedAttribute>();
                    if (attr == null) continue;

                    if (member is FieldInfo field) {
                        var sourceValue = field.GetValue(parent.Style);
                        var targetValue = field.GetValue(element.Style);

                        if (sourceValue is not null && targetValue is null) {
                            field.SetValue(element.Style, sourceValue);
                        }
                    }

                    if (member is PropertyInfo prop) {
                        var sourceValue = prop.GetValue(parent.Style);
                        var targetValue = prop.GetValue(element.Style);

                        if (sourceValue is not null && targetValue is null) {
                            prop.SetValue(parent.Style, sourceValue);
                        }
                    }
                }
            }
        }

        internal void ApplyDefaultValues(Style style) {
            Style.MergeStyles(style, Style.Default);
        }

        internal void ApplyClassStyles(Element current, Style nStyle) {
            foreach (var className in current.ClassList) {
                if (this.ContainsKey("." + className)) {
                    Style.MergeStyles(nStyle, this["." + className]);
                }
            }
        }

        internal void ApplyIDStyles(Element current, Style nStyle) {
            if (current.Attributes.ContainsKey("id")) {
                var selector = "#" + current.Attributes["id"];
                if (this.ContainsKey(selector)) {
                    Style.MergeStyles(nStyle, this[selector]);
                }
            }
        }

        internal void ApplyNameStyles(Element current, Style nStyle) {
            if (this.ContainsKey(current.TagName)) {
                Style.MergeStyles(nStyle, this[current.TagName]);
            }
        }

        internal void ApplyWildcardStyles(Element current, Style nStyle) {
            if (this.ContainsKey("*")) {
                Style.MergeStyles(nStyle, this["*"]);
            }
        }
    }
}

