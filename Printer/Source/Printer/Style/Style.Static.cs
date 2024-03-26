using Leagueinator.CSSParser;
using System.Reflection;
using Leagueinator.Utility;
using System.Diagnostics;

namespace Leagueinator.Printer.Styles {
    public partial class Style {
        static Style() {
            try {
                Console.WriteLine("Style Static Enter");
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
                Console.WriteLine("Style Static Exit");
            }
            catch (Exception ex) {
                Console.WriteLine(ex.ToString());
            }
        }

        public static IReadOnlyDictionary<string, FieldInfo> CSSFields { get; private set; }
        public static IReadOnlyDictionary<string, PropertyInfo> CSSProperties { get; private set; }
        public static IReadOnlyDictionary<string, FieldInfo> InheritedFields { get; private set; }
        public static IReadOnlyDictionary<string, PropertyInfo> InheritedProperties { get; private set; }
    }
}
