using System.Diagnostics;
using System.Reflection;
using Leagueinator.CSSParser;

namespace Leagueinator.Printer {
    public struct Cardinal<T> {
        public T? Left, Right, Top, Bottom;

        public Cardinal() {
            Left = default;
            Right = default;
            Top = default;
            Bottom = default;
        }

        public Cardinal(T value) {
            Left = value;
            Right = value;
            Top = value;
            Bottom = value;
        }

        public Cardinal(T top, T right, T bottom, T left) {
            this.Left = left;
            this.Right = right;
            this.Top = top;
            this.Bottom = bottom;
        }

        public static bool TryParse(string source, out Cardinal<T> target) {
            target = new();

            string[] split = source.Split();
            List<object> values = new();

            for (int i = 0; i < split.Length; i++) {
                bool parsed = MultiParse.TryParse(split[i], typeof(T), out object? obj);
                if (parsed && obj != null) values.Add(obj);
            }

            if (split.Length == 0) throw new Exception("No cardinal values found");
            if (values.Count == 0) throw new Exception("No cardinal values found");

            target.Top = (T?)values[0];
            target.Right = values.Count > 1 ? (T?)values[1] : (T?)values[0];
            target.Bottom = values.Count > 2 ? (T?)values[2] : (T?)values[0];
            target.Left = values.Count > 3 ? (T?)values[3] : (T?)values[0];

            return true;
        }

        public override string ToString() {
            return $"[{this.Top} {this.Right} {this.Bottom} {this.Left}]";
        }
    }
}
