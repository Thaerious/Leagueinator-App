using Leagueinator.Printer.Styles;
using System.Reflection;

namespace Leagueinator.CSSParser {

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class CSS : Attribute {
        public string? DefVal { get; init; } = null;

        public string SetMethod { get; init; } = "";

        public CSS() { }

        public CSS(string defVal) {
            this.DefVal = defVal;
        }

        public CSS(string? defVal, string setMethod) {
            this.DefVal = defVal;
            this.SetMethod = setMethod;
        }

        /// <summary>
        /// Attempt to apply a Value to the style object
        /// </summary>
        /// <param name="style">The style object that will recieve the Value.</param>
        /// <param name="value">The string source for the Value.</param>
        /// <param name="member">The prop that will receive the Value.</param>
        /// <returns>True if a Value was assigned</returns>
        public bool TryParse(Style style, string value, MemberInfo member) {
            if (!string.IsNullOrEmpty(this.SetMethod)) {
                MethodInfo method
                    = typeof(Style).GetMethod(this.SetMethod, [typeof(String)])
                    ?? throw new MissingMethodException($"On member '{member.Name}' CSS set method '{this.SetMethod}' not found.");

                return (bool)method.Invoke(style, [value])!;
            }
            if (member is FieldInfo field) {
                var r = MultiParse.TryParse(value.Trim(), field.FieldType, out object? newObject);
                field.SetValue(style, newObject);
                return r;
            }

            if (member is PropertyInfo prop) {
                var r = MultiParse.TryParse(value.Trim(), prop.PropertyType, out object? newObject);
                prop.SetValue(style, newObject);
                return r;
            }

            return false;
        }
    }
}
