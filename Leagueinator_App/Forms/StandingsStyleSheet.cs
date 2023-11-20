using Leagueinator.Printer;

namespace Leagueinator_App.Forms {
    internal class StandingsStyleSheet : StyleSheet {
        public StandingsStyleSheet() {
            this["root"] = new Flex() {
                BorderColor = new(Color.Black),
                BorderSize = new(2f),
                Flex_Direction = Flex_Direction.Column
            };

            this["div"] = new Flex() {
                Display = Display.Flex,
                Justify_Content = Justify_Content.Center,
                Align_Items = Align_Items.Center,
                Font = new Font("Arial Black", 20, FontStyle.Bold, GraphicsUnit.Point),
                BorderColor = new(Color.Black),
                BorderSize = new(1f),
            };
        }
    }
}
