using Leagueinator.Model.Tables;
using Leagueinator.Model.Views;
using Leagueinator.Utility;
using System.Collections.ObjectModel;

namespace Leagueinator.Scoring.Plus {

    /// <summary>
    /// A summary of all match results for a specific team.
    /// </summary>
    [TimeTrace]
    public record SummaryPlus : ISummary<MatchResultsPlus, SummaryPlus> {
        public TeamView TeamView { get; }
        public int GamesPlayed { get; }
        public int Ends { get; }
        public int BowlsFor { get; }
        public int BowlsAgainst { get; private set; }
        public int Wins { get; }
        public int PointsFor { get; }
        public int PointsAgainst { get; }
        public int PlusFor { get; }
        public int PlusAgainst { get; }

        public SummaryPlus(TeamView team) {
            this.TeamView = team;

            foreach (MatchResultsPlus matchResult in this.MatchResults()) {
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

        public List<MatchResultsPlus> MatchResults (){
            List<MatchResultsPlus> allResults = [];

            foreach (MatchRow matchRow in this.TeamView.Matches) {
                foreach (TeamRow teamRow in matchRow.Teams) {
                    if (this.TeamView.Equals(teamRow)){
                        allResults.Add(new MatchResultsPlus(teamRow));
                    }
                }
            }

            return allResults;
        }

        public override string ToString() {
            return $"[{this.Wins}, {this.Ends}, {this.BowlsFor}, {this.BowlsAgainst}, {this.PointsFor}, {this.PlusFor}, {this.PointsAgainst}, {this.PlusAgainst}]";
        }

        public int CompareTo(SummaryPlus? that) {
            if (that is null) return -1;

            if (that.Wins != this.Wins) return that.Wins - this.Wins;
            if (that.PointsFor != this.PointsFor) return that.PointsFor - this.PointsFor;
            if (that.PlusFor != this.PlusFor) return that.PlusFor - this.PlusFor;
            if (that.PointsAgainst != this.PointsAgainst) return this.PointsAgainst - that.PointsAgainst;
            if (that.PlusAgainst != this.PlusAgainst) return this.PlusAgainst - that.PlusAgainst;
            return 0;
        }
    }
}
