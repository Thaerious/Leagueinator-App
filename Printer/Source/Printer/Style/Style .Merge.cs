using Leagueinator.CSSParser;
using Leagueinator.Printer.Query;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Text;
using Leagueinator.Printer.Elements;
using Leagueinator.Utility;

namespace Leagueinator.Printer.Styles {

    public partial class Style : IComparable<Style> {

        private readonly List<Style> sourceStyles = [];
        public IReadOnlyCollection<Style> SourceStyles => this.sourceStyles.AsReadOnly();

        internal static void MergeInheritedStyles(Style target, Style source) {
            foreach (var property in Style.InheritedProperties.Values) {
                var sourceValue = property.GetValue(source);
                var targetValue = property.GetValue(target);
                if (sourceValue == null || targetValue != null) continue;
                property.SetValue(target, sourceValue);
            }
        }

        internal void MergeWith(Style source) {
            this.sourceStyles.Add(source);  

            foreach (var property in Style.CSSProperties.Values) {
                var sourceValue = property.GetValue(source);
                var targetValue = property.GetValue(this);
                if (sourceValue == null || targetValue != null) continue;
                property.SetValue(this, sourceValue);
            }
        }
    }
}
