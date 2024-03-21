using Leagueinator.CSSParser;
using Leagueinator.Utility;
using System.Diagnostics;
using System.Reflection;

namespace Leagueinator.Printer {
    public partial class Style {
        static Style() {
            PropertyInfo[] properties = typeof(Style).GetProperties();
            FieldInfo[] fields = typeof(Style).GetFields();

            Dictionary<string, FieldInfo> fDict = [];
            Dictionary<string, PropertyInfo> pDict = [];
            Dictionary<string, FieldInfo> ifDict = [];
            Dictionary<string, PropertyInfo> ipDict = [];

            foreach (var property in properties) {
                if (property.GetCustomAttribute<CSS>() == null) continue;
                if (property.CanWrite && property.CanRead) pDict[property.Name.ToLower()] = property;

                if (property.GetCustomAttribute<InheritedAttribute>() == null) continue;
                ipDict[property.Name.ToLower()] = property;
            }

            foreach (var field in fields) {
                if (field.GetCustomAttribute<CSS>() == null) continue;
                fDict[field.Name.ToFlatCase()] = field;

                if (field.GetCustomAttribute<InheritedAttribute>() == null) continue;
                ifDict[field.Name.ToFlatCase()] = field;
            }

            Style.CSSFields = fDict.AsReadOnly();
            Style.CSSProperties = pDict.AsReadOnly();
            Style.InheritedFields = ifDict.AsReadOnly();
            Style.InheritedProperties = ipDict.AsReadOnly();
        }

        public static IReadOnlyDictionary<string, FieldInfo> CSSFields { get; private set; }
        public static IReadOnlyDictionary<string, PropertyInfo> CSSProperties { get; private set; }
        public static IReadOnlyDictionary<string, FieldInfo> InheritedFields { get; private set; }
        public static IReadOnlyDictionary<string, PropertyInfo> InheritedProperties { get; private set; }
    }
}
