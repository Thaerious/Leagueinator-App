using Leagueinator.Utility;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace Printer {

    public class LCDictionary<V> : Dictionary<string, V> {
        new public V this[string key] {
            get {
                return base[key.ToPlainCase()];
            }
            set {
                base[key.ToPlainCase()] = value;
            }
        }
    }

    public static class ReflectionExt {

        public static LCDictionary<FieldInfo> LCDictionary(this FieldInfo[] fields) {
            LCDictionary<FieldInfo> dict = new();

            foreach (var field in fields) {
                dict[field.Name] = field;
            }
            return dict;
        }

        public static LCDictionary<PropertyInfo> LCDictionary(this PropertyInfo[] properties) {
            LCDictionary<PropertyInfo> dict = new();

            foreach (var property in properties) {
                dict[property.Name] = property;
            }
            return dict;
        }
    }
}
