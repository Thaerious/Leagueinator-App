using Leagueinator.Model.Views;

namespace Leagueinator.Forms.Results.Plus {
    
    /// <summary>
    /// A summary of all match results for a specific team.
    /// </summary>
    public record SummaryResultsPlus : IComparable<SummaryResultsPlus>{
        public Team Team { get;}
        public int GamesPlayed { get; }
        public int Ends { get; }
        public int BowlsFor { get; }
        public int BowlsAgainst { get; private set; }
        public int Wins { get; }
        public int PointsFor { get; }
        public int PointsAgainst { get; }
        public int PlusFor { get; }
        public int PlusAgainst { get; }

        public SummaryResultsPlus(Team team, IReadOnlyList<MatchResultsPlus> matchResults) {
            this.Team = team;
            
            foreach (MatchResultsPlus matchResult in matchResults) {
                this.GamesPlayed++;
                this.Ends += matchResult.Ends;
                this.BowlsFor += matchResult.BowlsFor;
                this.BowlsAgainst += matchResult.BowlsAgainst;
                if (matchResult.Result() == Result.Win || matchResult.Result() == Result.Bye) this.Wins++;
                this.PointsFor += matchResult.PointsFor;
                this.PointsAgainst += matchResult.PointsAgainst;
                this.PlusFor += matchResult.PlusFor;
                this.PlusAgainst += matchResult.PlusAgainst;
            }
        }

        public override string ToString() {
            return $"[{Wins}, {Ends}, {BowlsFor}, {BowlsAgainst}, {PointsFor}, {PlusFor}, {PointsAgainst}, {PlusAgainst}]";
        }

        public int CompareTo(SummaryResultsPlus? that) {
            if (that is null) return -1;

            if (that.Wins != this.Wins) return that.Wins - this.Wins;
            if (that.PointsFor != this.PointsFor) return that.PointsFor - this.PointsFor;
            if (that.PlusFor != this.PlusFor) return that.PlusFor - this.PlusFor;
            if (that.PointsAgainst != this.PointsAgainst) return that.PointsAgainst - this.PointsAgainst;
            if (that.PlusAgainst != this.PlusAgainst) return that.PlusAgainst - this.PlusAgainst;
            return 0;
        }
    }
}
