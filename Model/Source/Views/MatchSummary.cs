using Leagueinator.Model.Tables;

namespace Leagueinator.Model.Views {
    
    /// <summary>
    /// A view of the Match results for a single TeamRow.
    /// </summary>
    /// <param name="teamRow"></param>    
    public record MatchSummary {
        public int GamesPlayed { get; }
        public int Ends { get; }
        public int BowlsFor { get; }
        public int BowlsAgainst { get; private set; }
        public int Wins { get; }
        public int PointsFor { get; }
        public int PointsAgainst { get; }
        public int PlusFor { get; }
        public int PlusAgainst { get; }

        public MatchSummary(IReadOnlyList<MatchResults> matchResults) {
            foreach (MatchResults matchResult in matchResults) {
                this.GamesPlayed++;
                this.Ends += matchResult.Ends;
                this.BowlsFor += matchResult.BowlsFor;
                this.BowlsAgainst += matchResult.BowlsAgainst;
                if (matchResult.IsWin()) this.Wins++;
                this.PointsFor += matchResult.PointsFor;
                this.PointsAgainst += matchResult.PointsAgainst;
                this.PlusFor += matchResult.PlusFor;
                this.PlusAgainst += matchResult.PlusAgainst;
            }
        }

        public override string ToString() {
            return $"[{Wins}, {Ends}, {BowlsFor}, {BowlsAgainst}, {PointsFor}, {PlusFor}, {PointsAgainst}, {PlusAgainst}]";
        }

        public readonly static IComparer<MatchSummary> CompareByRound = new RoundCompare();

        private class RoundCompare : IComparer<MatchSummary> {
            public int Compare(MatchSummary x, MatchSummary y) {
                return 0;
            }
        }
    }
}
