using Leagueinator.CSSParser;
using Newtonsoft.Json.Linq;

namespace Leagueinator.Printer {
    public class CardinalParseException : Exception {
        public string SourceString { get; }
        public Type Type { get; }

        public CardinalParseException(string sourceString, Type type, string message) : base(message) {
            this.SourceString = sourceString;
            this.Type = type;
        }
    }

    public class Cardinal<T> where T: new() {
        public readonly T Left, Right, Top, Bottom;

        public Cardinal() {
            this.Top = new();
            this.Right = new();
            this.Bottom = new();
            this.Left = new();

        }

        public Cardinal(T value) {
            this.Top = value;
            this.Right = value;
            this.Bottom = value;
            this.Left = value;
        }

        public Cardinal(T top, T right, T bottom, T left) {
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;
            this.Left = left;
        }

        public static bool TryParse(string source, out Cardinal<T> target) {
            string[] split = source.Split(' ', ',');
            List<object> values = new();

            for (int i = 0; i < split.Length; i++) {
                bool parsed = MultiParse.TryParse(split[i], typeof(T), out object? obj);
                if (parsed && obj != null) values.Add(obj);
            }

            if (split.Length == 0) {
                throw new CardinalParseException(source, typeof(T), "No parsable strings");
            }

            if (values.Count == 0) {
                throw new CardinalParseException(source, typeof(T), "No converted objects");
            }

            target = new(
                (T)values[0],
                values.Count > 1 ? (T)values[1] : (T)values[0],
                values.Count > 2 ? (T)values[2] : (T)values[0],
                values.Count > 3 ? (T)values[3] : (T)values[0]
            );

            return true;
        }

        public override string ToString() {
            return $"[{this.Top} {this.Right} {this.Bottom} {this.Left}]";
        }
    }
}
