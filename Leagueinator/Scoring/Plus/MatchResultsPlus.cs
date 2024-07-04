using Leagueinator.Model.Tables;
using Leagueinator.Utility;

namespace Leagueinator.Scoring.Plus {

    public enum Result { Win, Loss, Tie, Bye };

    /// <summary>
    /// The results of a single match for a single team.
    /// Consolidates multiple table rows into one view.
    /// Calculates extra information for plus scoring.
    /// </summary>
    /// <param name="teamRow"></param>    
    public record MatchResultsPlus : IMatchResult<MatchResultsPlus> {

        /// <summary>
        /// The results of a single match for a single team.
        /// Consolidates multiple table rows into one view.
        /// </summary>
        /// <param name="teamRow"></param>
        public MatchResultsPlus(TeamRow teamRow) {
            this._teamRow = teamRow;

            // For a bye, average bowls for and against from all other matches.
            // These values will be the same.
            if (this.Result() == Plus.Result.Bye) {
                RoundRow roundRow = this.TeamRow.Match.Round;
                this.BowlsFor = (int)roundRow.Matches
                                .SelectMany(match => match.Teams)
                                .Where(team => team.Members.Count > 0)
                                .Where(team => !team.Equals(teamRow))
                                .Select(team => team.Bowls)
                                .DefaultIfEmpty(0)
                                .Average();

                this.BowlsAgainst = this.BowlsFor;
            }
            else {
                this.BowlsFor = teamRow.Bowls;
                foreach (TeamRow t in teamRow.Match.Teams) {
                    if (!t.Equals(teamRow)) this.BowlsAgainst += t.Bowls;
                }
            }
        }

        public TeamRow TeamRow {
            get => _teamRow;
        }

        public int Round { get => this.TeamRow.Match.Round.Index; }
        public int Lane { get => this.TeamRow.Match.Lane; }
        public int Ends { get => this.TeamRow.Match.Ends; }
        public int BowlsFor { get; } = 0;
        public int TieBreaker { get => this.TeamRow.Tie; }
        public int BowlsAgainst { get; private set; } = 0;

        public int PointsFor {
            get => Math.Min(this.BowlsFor, (int)(this.Ends * 1.5));
        }

        public int PointsAgainst {
            get => Math.Min(this.BowlsAgainst, (int)(this.Ends * 1.5));
        }

        public int PlusFor {
            get => this.BowlsFor - this.PointsFor;
        }

        public int PlusAgainst {
            get => this.BowlsAgainst - this.PointsAgainst;
        }

        public override string ToString() {
            return $"[{this.Round}, {this.Lane}, {this.Ends}, {this.BowlsFor}, {this.BowlsAgainst}, {this.TieBreaker}, {this.PointsFor}, {this.PlusFor}, {this.PointsAgainst}, {this.PlusAgainst}]";
        }

        /// <summary>
        /// Calculate W/L/T based on bowls for vs bowls against.
        /// 
        /// </summary>
        /// <returns></returns>
        public Result Result() {
            int nonEmptyTeams = this.TeamRow.Match.Teams.Where(t => t.Members.Count > 0).Count();

            if (nonEmptyTeams == 1) return Plus.Result.Bye;
            if (this.BowlsFor > this.BowlsAgainst) return Plus.Result.Win;
            if (this.BowlsFor < this.BowlsAgainst) return Plus.Result.Loss;
            if (this.TieBreaker > 0) return Plus.Result.Win;
            if (this.TieBreaker < 0) return Plus.Result.Loss;

            foreach (TeamRow t in this.TeamRow.Match.Teams) {
                if (!t.Equals(this.TeamRow)) continue;
                if (t.Tie > 0) return Plus.Result.Loss;
            }

            return Plus.Result.Tie;
        }

        public int CompareTo(MatchResultsPlus? that) {
            if (that is null) return 1;
            if (this.Round != that.Round) return this.Round - that.Round;
            if (this.Lane != that.Lane) return this.Lane - that.Lane;
            return 0;
        }

        private TeamRow _teamRow;
    }
}
