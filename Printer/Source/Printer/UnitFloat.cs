using Leagueinator.Utility;
using System.Text.RegularExpressions;

namespace Leagueinator.Printer {
    public class UnitFloat {
        public delegate void UnitFloatDelegate(float value);
        public event UnitFloatDelegate ValueChange = delegate { };
        public static readonly UnitFloat Default = new();

        public float? Value {
            get => _value;
            set {
                if (value is null) throw new InvalidOperationException();
                this._value = value;
                this.ValueChange.Invoke((float)value);
            }
        }

        public bool HasValue => this.Value != null;

        public string Unit { get; init; } = "auto";

        public float Factor { get; init; } = 0f;

        public void ApplySource(float value) {
            if (this.Unit == "%") {
                this.Value = value * this.Factor / 100;
            }
            else if(this.Unit == "px") {
                this.Value = this.Factor;
            }
        }

        public static implicit operator float(UnitFloat? m) {
            if (m is null) return 0f;
            if (m.HasValue) return (float)m.Value!;
            return 0f;
        }

        public static bool TryParse(string input, out UnitFloat target) {
            if (input.IsEmpty()) {
                target = new();
                return false;
            }

            if (input.Equals("auto") || input.IsEmpty()) {
                target = new() {
                    Factor = 0f,
                    Unit = "auto"
                };
                return true;
            }

            string pattern = @"(\d+)([a-zA-Z%]+)";
            Match match = Regex.Match(input, pattern);

            target = new() {
                Factor = float.Parse(match.Groups[1].Value),
                Unit = match.Groups[2].Value
            };

            return true;
        }

        public override String ToString() {
            return $"({this.Factor}, {this.Unit}) = {(float)this}f";
        }

        private float? _value = default;

        public enum Axis { MAJOR, MINOR }
        public enum Dim { WIDTH, HEIGHT }
    }
}
