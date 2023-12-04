namespace Leagueinator.Utility {
    public static class StringExtensions {
        public static string DelString<T>(this IEnumerable<T> list, string del = ", ") {
            return string.Join(del, list.Select(t => t?.ToString()).ToArray());
        }

        public static string DelString<T>(this IList<T> list, string del = ", ") {
            return string.Join(del, list.Select(t => t?.ToString()).ToArray());
        }

        public static string DelString<T>(this IEnumerable<T> list, int colsize, string del = ", ") {
            string?[] array = list.Select(t => t?.ToString())
                .NotNull()
                .Select(t => t?.PadRight(colsize, ' '))
                .ToArray();
            return string.Join(del, array);
        }

        public static bool IsEmpty(this string str) {
            return str == null || str.Length == 0;
        }

        public static string GetHashCode(this object obj, string format) {
            ArgumentNullException.ThrowIfNull(obj, nameof(obj));
            return obj.GetHashCode().ToString(format);
        }

        /// <summary>
        /// Creates a copy of the string in all lower case with spliters (-, _).
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToFlatCase(this string source) {
            return source.ToLower().Split('_', '-').DelString("");
        }
    }
}
