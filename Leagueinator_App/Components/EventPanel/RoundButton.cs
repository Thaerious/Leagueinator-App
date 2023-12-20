using Model;

namespace Leagueinator.App.Components {
    internal class RoundButton : Button {
        public Round Round { get; }

        public RoundButton(Round round) {
            this.Round = round;
        }
    }
}
