using Leagueinator.CSSParser;

namespace Leagueinator.Printer {
    public class CoordinateParseException : Exception {
        public string SourceString { get; }
        public Type Type { get; }

        public CoordinateParseException(string sourceString, Type type, string message) : base(message) {
            this.SourceString = sourceString;
            this.Type = type;
        }
    }

    public class Coordinate<T> where T : new() {
        public readonly T X, Y;

        public Coordinate() {
            this.X = new();
            this.Y = new();

        }

        public Coordinate(T value) {
            this.X = value;
            this.Y = value;
        }

        public Coordinate(T x, T y) {
            this.X = x;
            this.Y = y;
        }

        public static bool TryParse(string source, out Coordinate<T> target) {
            string[] split = source.Split(' ', ',');
            List<object> values = new();

            for (int i = 0; i < split.Length; i++) {
                bool parsed = MultiParse.TryParse(split[i], typeof(T), out object? obj);
                if (parsed && obj != null) values.Add(obj);
            }

            if (split.Length == 0) {
                throw new CoordinateParseException(source, typeof(T), "No parsable strings");
            }

            if (values.Count == 0) {
                throw new CoordinateParseException(source, typeof(T), "No converted objects");
            }

            target = new(
                (T)values[0],
                values.Count > 1 ? (T)values[1] : (T)values[0]
            );

            return true;
        }

        public override string ToString() {
            return $"[{this.X} {this.Y}]";
        }
    }
}
