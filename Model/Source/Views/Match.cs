using Leagueinator.Model.Tables;

namespace Leagueinator.Model.Views {
    public readonly struct Match(TeamRow team) {
        public int Round { get; } = team.Match.Round;
        public int Lane { get; } = team.Match.Lane;
        public int Ends { get; } = team.Match.Ends;
        public int Bowls { get; } = team.Bowls;
        public int Tie { get; } = team.Tie;

        public override string ToString() {
            return $"[{Round}, {Lane}, {Ends}, {Bowls}, {Tie}]";
        }

        public readonly static IComparer<Match> CompareByRound = new RoundCompare();

        private class RoundCompare : IComparer<Match> {
            public int Compare(Match x, Match y) {
                if (x.Round != y.Round) return x.Round - y.Round;
                if (x.Lane != y.Lane) return x.Lane - y.Lane;
                return 0;
            }
        }
    }
}
