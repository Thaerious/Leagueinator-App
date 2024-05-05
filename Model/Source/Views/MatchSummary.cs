using Leagueinator.Model.Tables;

namespace Leagueinator.Model.Views {
    
    /// <summary>
    /// A view of the Match results for a single Team.
    /// </summary>
    /// <param name="teamRow"></param>    
    public record MatchSummary : IComparable<MatchSummary>{
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

        public MatchSummary(Team teamRow, IReadOnlyList<MatchResults> matchResults) {
            this.Team = teamRow;
            
            foreach (MatchResults matchResult in matchResults) {
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

        public int CompareTo(MatchSummary? that) {
            if (that is null) return 1;

            if (this.Wins != that.Wins) return this.Wins - that.Wins;
            if (this.PointsFor != that.PointsFor) return this.PointsFor - that.PointsFor;
            if (this.PlusFor != that.PlusFor) return this.PlusFor - that.PlusFor;
            if (this.PointsAgainst != that.PointsAgainst) return this.PointsAgainst - that.PointsAgainst;
            if (this.PlusAgainst != that.PlusAgainst) return this.PlusAgainst - that.PlusAgainst;
            return 0;
        }
    }
}
