using Leagueinator.Printer;
using Leagueinator.Utility;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Text.RegularExpressions;
using static Leagueinator.Printer.Style;

namespace Leagueinator.CSSParser {
    public static class MultiParse {

        /// <summary>
        /// Assign a value of 'type' to 'target' based on the 'source' string.
        /// </summary>
        /// <param name="source">The string used to generate the new object.</param>
        /// <param name="type">The type of object to generate.</param>
        /// <param name="target">The reference to write the generated value to.</param>
        /// <returns>True if a value was written, otherwise false.</returns>
        public static bool TryParse(string source, Type type, out object? target) {
            target = default;

            // Check if the type is nullable get the underlying type
            Type? underlyingType = Nullable.GetUnderlyingType(type);
            Type targetType = underlyingType ?? type;

            // If the target it is a string
            if (targetType == typeof(string)) {
                Debug.WriteLine($"MultiParse.TryParse({source}, {type}) as string");
                target = source;
                return true;
            }

            // if the target has static TryParse(string, out type)
            Debug.WriteLine($"Seeking method '{typeof(string)}', '{targetType.MakeByRefType()}'");
            var method = targetType.GetMethod(
                "TryParse",
                BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy,
                new Type[] { typeof(string), targetType.MakeByRefType() }
            );

            // If the target has a "TryParse" method, applies to primitives.
            if (method != null) {
                Debug.WriteLine($"MultiParse.TryParse({source}, {type}) as method");
                object?[] args = new object?[] { source, target };
                bool result = (bool)method.Invoke(null, args)!;
                if (result) target = args[1];
                return result;
            }

            // If the target is an Enum
            if (targetType.IsEnum) {
                Debug.WriteLine($"MultiParse.TryParse({source}, {type}) as enum");
                if (!Enum.IsDefined(targetType, source)) {
                    return false;
                }
                else {
                    target = Enum.Parse(targetType, source);
                    return true;
                }
            }

            // Search the style's public static properties
            var prop = targetType.GetProperty(source);
            if (prop != null && prop.CanRead && prop.PropertyType == targetType) {
                Debug.WriteLine($"MultiParse.TryParse({source}, {type}) as static property");
                target = prop.GetValue(null);
                return true;
            }

            var specialCase = typeof(SpecialCases).GetMethod(
                "Parse",
                BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy,
                new Type[] { typeof(string), targetType.MakeByRefType() }
            );

            if (specialCase != null) {
                Debug.WriteLine($"MultiParse.TryParse({source}, {type}) as special case");
                object?[] args = new object?[] { source, target };
                bool result = (bool)specialCase.Invoke(null, args)!;
                if (result) target = args[1];
                return result;
            }

            Debug.WriteLine($"MultiParse.TryParse({source}, {type}) did not parse");
            return false;
        }
    }

    public static class SpecialCases {
        private static string rgbPattern = @"rgb\([ ]*(\d\d?\d?)[ ]*,[ ]*(\d\d?\d?)[ ]*,[ ]*(\d\d?\d?)[ ]*\)";
        private static Regex rgbRegex = new Regex(rgbPattern);

        public static bool Parse(string source, out Color color) {
            var match = rgbRegex.Match(source);
            if (match.Success) {
                int r = int.Parse(match.Groups[1].Value);
                int g = int.Parse(match.Groups[2].Value);
                int b = int.Parse(match.Groups[3].Value);
                color = Color.FromArgb(r, g, b);
                return true;
            }
            color = default;
            return false;
        }

        public static bool Parse(string source, out Func<float, float?> func) {
            if (source.EndsWith("px")) {
                var substring = source.Substring(0, source.Length - 2);
                var parsed = float.Parse(substring);
                func = f => parsed;
                return true;
            }
            else if (source.EndsWith("%")) {
                var substring = source.Substring(0, source.Length - 1);
                var parsed = float.Parse(substring);
                func = f => f * (parsed / 100);
                return true;
            }
            else {
                func = f => null;
                return false;
            }
        }
    }
}
