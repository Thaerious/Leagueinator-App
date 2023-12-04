using Leagueinator.Utility;
using Newtonsoft.Json.Linq;
using System;
using System.Drawing;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Leagueinator.CSSParser {
    public static class MultiParse {

        /// <summary>
        /// Assign a value of type 'T' to target based on a source string.
        /// </summary>
        /// <typeparam name="T">The type of object to generate.</typeparam>
        /// <param name="source">The string used to generate the new object.</param>
        /// <param name="target">The reference to write the generated value to.</param>
        /// <returns>True if a value was written, otherwise false.</returns>
        /// <returns></returns>
        public static bool TryParse<T>(string source, out T? target) {
            var r = TryParse(source, typeof(T), out object? _target);
            target = (T?)_target;
            return r;
        }

        /// <summary>
        /// Assign a value of 'type' to 'target' based on the 'source' string.
        /// </summary>
        /// <param name="source">The string used to generate the new object.</param>
        /// <param name="type">The type of object to generate.</param>
        /// <param name="target">The reference to write the generated value to.</param>
        /// <returns>True if a value was written, otherwise false.</returns>
        public static bool TryParse(string source, Type type, out object? target) {
            target = default;

            // If the type is nullable get the underlying type
            Type? underlyingType = Nullable.GetUnderlyingType(type);
            Type targetType = underlyingType ?? type;

            if (ParseString(source, targetType, ref target)) return true;
            if (ParseStaticMethod(source, targetType, ref target)) return true;
            if (ParseEnum(source, targetType, ref target)) return true;
            if (ParsePublicProperty(source, targetType, ref target)) return true;
            if (ParseSpecialCase(source, targetType, ref target)) return true;
           
            return false;
        }

        private static bool ParseString(string source, Type type, ref object? target) {
            if (type != typeof(string)) return false;
            target = source;
            return true;
        }

        private static bool ParseStaticMethod(string source, Type type, ref object? target) {
            var method = type.GetMethod(
                "TryParse",
                BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy,
                new Type[] { typeof(string), type.MakeByRefType() }
            );

            if (method == null) return false;

            object?[] args = new object?[] { source, target };
            bool result = (bool)method.Invoke(null, args)!;
            if (result) target = args[1];
            return result;
        }

        private static bool ParseSpecialCase(string source, Type type, ref object? target) {
            var specialCase = typeof(SpecialCases).GetMethod(
                "Parse",
                BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy,
                new Type[] { typeof(string), type.MakeByRefType() }
            );

            if (specialCase != null) {
                object?[] args = new object?[] { source, target };
                bool result = (bool)specialCase.Invoke(null, args)!;
                if (result) target = args[1];
                return result;
            }

            return false;
        }

        private static bool ParseEnum(string source, Type type, ref object? target) {
            if (!type.IsEnum) return false;

            string[] names = Enum.GetNames(type);

            foreach (string name in names) {
                if (name.ToFlatCase() == source.ToFlatCase()) {
                    target = Enum.Parse(type, name);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Create a new object based on the value of a public property.
        /// The propery must have a public getter.
        /// The properties are searched without case and decoration.
        /// If a public property was not found target is set to default and false is returned.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="type"></param>
        /// <param name="target"></param>
        /// <returns>True if value was assigned</returns>
        private static bool ParsePublicProperty(string source, Type type, ref object? target) {
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Static).ToDictionary();
            var key = source.ToFlatCase();

            if (!properties.ContainsKey(key)) return false;
            var property = properties[key];
            if (!property.CanRead) return false;

            target = properties[key].GetValue(null);
            return true;
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
}
