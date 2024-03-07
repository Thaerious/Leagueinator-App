namespace Leagueinator.Printer {
    public class Box : Cardinal<UnitFloat> {
        public static readonly Box Default = new(new());

        public Box(UnitFloat value) : base(value) { }
        public Box(UnitFloat top, UnitFloat right, UnitFloat bottom, UnitFloat left) : base(top, right, bottom, left) { }

        public static bool TryParse(string source, out Box target) {
            if (TryParse(source, out Cardinal<UnitFloat> tc)) {
                target = new(tc.Top, tc.Right, tc.Bottom, tc.Left);
                return true;
            }

            target = Default;
            return false;
        }
    }
}
