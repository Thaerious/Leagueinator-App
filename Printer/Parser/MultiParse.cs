﻿using Leagueinator.Utility;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Text.RegularExpressions;

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

            // if the target has static TryParse(string, out type)
            var method = targetType.GetMethod(
                "TryParse",
                BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy,
                new Type[] { typeof(string), targetType.MakeByRefType() }
            );

            // If the target it is a string
            if (targetType == typeof(string)) {
                target = source;
                return true;
            }

            // If the target has a "TryParse" method, applies to primitives.
            if (method != null) {
                object?[] args = new object?[] { source, target };
                bool result = (bool)method.Invoke(null, args)!;
                if (result) target = args[1];
                return result;
            }

            // If the target is an Enum
            if (targetType.IsEnum) {
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
                target = prop.GetValue(null);
                return true;
            }

            var specialCase = typeof(SpecialCases).GetMethod(
                "Parse",
                BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy,
                new Type[] { typeof(string), targetType.MakeByRefType() }
            );

            if (specialCase != null) {
                object?[] args = new object?[] { source, target };
                bool result = (bool)specialCase.Invoke(null, args)!;
                if (result) target = args[1];
                return result;
            }

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
    }
}
