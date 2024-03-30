using Leagueinator.Printer.Elements;
using Leagueinator.Utility;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Leagueinator.Printer {
    public class UnitFloat {
        public enum Orientation { HORZ, VERT }

        public static readonly UnitFloat Default = new();
        public string Unit { get; set; } = "auto";
        internal Element? Element { get; set; } = default;
        public Orientation Orient = Orientation.HORZ;

        public float Value = 0f;

        public UnitFloat() { }

        public UnitFloat(float value, string unit) {
            this.Value = value;
            this.Unit = unit;
        }

        public UnitFloat(float value) : this(value, "px") {}

        public void SetSource(Element element, Orientation orientation) {
            this.Element = element;
            this.Orient = orientation;
        }

        public static implicit operator float(UnitFloat? m) {
            return m?.ToFloat() ?? 0f;
        }

        public float ToFloat() {
            if (this is null) return 0;

            switch (this.Unit) {
                case "%":
                    if (this.Orient == Orientation.HORZ) {
                        return (this.Element?.ContainerRect.Width ?? 0f) * this.Value / 100f;
                    }
                    else {
                        return (this.Element?.ContainerRect.Height ?? 0f) * this.Value / 100f;
                    }

                default: return this.Value;
            }
        }

        //case "%h": return (this.Element?.ContainerRect.Height ?? 0f) * this.Value / 100f;
        //case "%w": return (this.Element?.ContainerRect.Width ?? 0f) * this.Value / 100f;
        //case "vh": return (this.Element?.Root.ContainerRect.Height ?? 0) * this.Value / 100f;
        //case "vw": return (this.Element?.Root.ContainerRect.Width ?? 0) * this.Value / 100f;

        public static bool TryParse(string input, out UnitFloat target) {
            if (input.IsEmpty()) {
                target = new();
                return false;
            }

            string pattern = @"(\d+)([a-zA-Z%]+)";
            Match match = Regex.Match(input, pattern);

            var value = float.Parse(match.Groups[1].Value);
            target = new(value, match.Groups[2].Value);

            return true;
        }

        public override String ToString() {
            return $"{this.Value}{this.Unit}";
        }
    }
}
