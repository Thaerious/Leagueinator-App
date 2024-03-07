namespace Leagueinator.Printer {
    public class BorderColor : Cardinal<Color> {
        public static readonly BorderColor Default = new(Color.Black);

        public BorderColor(Color value) : base(value) { }
        public BorderColor(Color top, Color right, Color bottom, Color left) : base(top, right, bottom, left) { }

        public static bool TryParse(string source, out BorderColor target) {
            if (TryParse(source, out Cardinal<Color> tc)) {
                target = new(tc.Top, tc.Right, tc.Bottom, tc.Left);
                return true;
            }

            target = Default;
            return false;
        }
    }
}
