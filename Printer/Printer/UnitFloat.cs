using Leagueinator.Printer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leagueinator.Printer {
    public class UnitFloat {
        public float Value { get; set; } = 0f;
        public string Unit { get; set; } = "";
        public bool HasValue { get; private set; } = false;

        public float Factor { get; set; } = 0f; // used when getting the value in a % case

        public UnitFloat() { }

        public UnitFloat(float value, string unit) {
            this.Value = value;
            this.Unit = unit;
            this.HasValue = true;
        }

        public static implicit operator float(UnitFloat m) {
            if (m.Unit == "%") return m.Value * m.Factor / 100;
            return m.Value;
        }

        public static bool TryParse(string source, out UnitFloat target) {
            if (source.EndsWith("px")) {
                var value = float.Parse(source[..^2]);
                target = new(value, "px");
                return true;
            }
            else if (source.EndsWith("%")) {
                var value = float.Parse(source[..^1]);
                target = new(value, "%");
                return true;
            }
            else {
                target = new UnitFloat();
                return false;
            }
        }
        public override String ToString() {
            return $"{this.Value}{this.Unit}";
        }
    }
}
