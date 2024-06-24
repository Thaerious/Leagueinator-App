using System.Text.RegularExpressions;

namespace Leagueinator.CSSParser {
    public static partial class MultiParse {
        public static class SpecialCases {
            private static string rgbPattern = @"rgb\([ ]*(\d\d?\d?)[ ]*,[ ]*(\d\d?\d?)[ ]*,[ ]*(\d\d?\d?)[ ]*\)";
            private static string rgbaPattern = @"rgba\([ ]*(\d\d?\d?)[ ]*,[ ]*(\d\d?\d?)[ ]*,[ ]*(\d\d?\d?)[ ]*,[ ]*(\d\d?\d?)[ ]*\)";
            private static Regex rgbRegex = new Regex(rgbPattern);
            private static Regex rgbaRegex = new Regex(rgbaPattern);

            public static bool Parse(string source, out Color color) {
                var match = rgbRegex.Match(source);
                if (match.Success) {
                    int r = int.Parse(match.Groups[1].Value);
                    int g = int.Parse(match.Groups[2].Value);
                    int b = int.Parse(match.Groups[3].Value);
                    color = Color.FromArgb(r, g, b);
                    return true;
                }

                match = rgbaRegex.Match(source);
                if (match.Success) {
                    int r = int.Parse(match.Groups[1].Value);
                    int g = int.Parse(match.Groups[2].Value);
                    int b = int.Parse(match.Groups[3].Value);
                    int a = int.Parse(match.Groups[4].Value);
                    color = Color.FromArgb(a, r, g, b);
                    return true;
                }

                color = default;
                return false;
            }
        }
    }
}
