using Leagueinator.Utility;
using System.Text.RegularExpressions;

namespace Leagueinator.Printer {
    public class UnitFloat {
        public static readonly UnitFloat Default = new();

        public string Unit { get; init; } = "auto";

        public float Factor { get; init; } = 0f;

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

            string pattern = @"(-?\d+)([a-zA-Z%]+)";
            Match match = Regex.Match(input, pattern);

            target = new() {
                Factor = float.Parse(match.Groups[1].Value),
                Unit = match.Groups[2].Value
            };

            return true;
        }

        public override String ToString() {
            return $"{this.Factor}{this.Unit}";
        }
    }
}
