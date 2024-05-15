using Leagueinator.Model.Tables;
using System.Diagnostics;

namespace Leagueinator.Forms.Results.Plus {

    public enum Result { Win, Loss, Tie, Bye };

    /// <summary>
    /// The results of a single match for a single team.
    /// Consolidates multiple table rows into one view.
    /// Calculates extra information for plus scoring.
    /// </summary>
    /// <param name="teamRow"></param>    
    public record MatchResultsPlus : IComparable<MatchResultsPlus> {
        public readonly TeamRow TeamRow;
        public int Round { get => this.TeamRow.Match.Round; }
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

        /// <summary>
        /// The results of a single match for a single team.
        /// Consolidates multiple table rows into one view.
        /// </summary>
        /// <param name="teamRow"></param>
        public MatchResultsPlus(TeamRow teamRow) {
            this.TeamRow = teamRow;

            // For a bye, average th bowls for and against from all other matches.
            // These values will be the same.
            if (this.Result() == Plus.Result.Bye) {
                RoundRow roundRow = TeamRow.Match.Round;
                this.BowlsFor = (int)roundRow.Matches
                                .SelectMany(match => match.Teams)
                                .Where(team => team.Members.Count > 0)
                                .Where(team => !team.Equals(teamRow))
                                .Select(team => team.Bowls)
                                .DefaultIfEmpty(0)
                                .Average();

                this.BowlsAgainst = BowlsFor;
            }
            else {
                this.BowlsFor = teamRow.Bowls;
                foreach (TeamRow t in teamRow.Match.Teams) {
                    if (!t.Equals(teamRow)) this.BowlsAgainst += t.Bowls;
                }
            }
        }

        public override string ToString() {
            return $"[{Round}, {Lane}, {Ends}, {BowlsFor}, {BowlsAgainst}, {TieBreaker}, {PointsFor}, {PlusFor}, {PointsAgainst}, {PlusAgainst}]";
        }

        /// <summary>
        /// Calculate W/L/T based on bowls for vs bowls against.
        /// 
        /// </summary>
        /// <returns></returns>
        public Result Result() {
            int nonEmptyTeams = TeamRow.Match.Teams.Where(t => t.Members.Count > 0).Count();

            if (nonEmptyTeams == 1) return Plus.Result.Bye;
            if (this.BowlsFor > BowlsAgainst) return Plus.Result.Win;
            if (this.BowlsFor < BowlsAgainst) return Plus.Result.Loss;
            if (this.TieBreaker > 0) return Plus.Result.Win;
            if (this.TieBreaker < 0) return Plus.Result.Loss;

            foreach (TeamRow t in TeamRow.Match.Teams) {
                if (!t.Equals(TeamRow)) continue;
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
    }
}
