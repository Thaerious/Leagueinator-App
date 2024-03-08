using Leagueinator.CSSParser;
using System.Reflection;

namespace Leagueinator.Printer {
    public partial class Style {

        public static Style Default {
            get {
                if (_default is null) BuildDefault();
                return Style._default!;
            }
        }

        private static void BuildDefault() {
            _default = new Style();

            MemberInfo[] methods = [.. typeof(Style).GetFields(), .. typeof(Style).GetProperties()];

            foreach (MemberInfo member in methods) {
                CSS? css = member.GetCustomAttribute<CSS>();
                if (css == null) continue;

                if (css.DefVal is not null) {
                    var r = css.TryParse(_default, css.DefVal, member);
                    if (!r) throw new Exception($"Failed to parse default value '{css.DefVal.Trim()}' for field '{member.Name}'.");
                }
            }
        }

        private static Style? _default = null;
    }
}
