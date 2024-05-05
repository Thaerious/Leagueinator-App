using Leagueinator.Model.Tables;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Leagueinator.Model.Views {

    public enum Result { Win, Loss, Tie, Bye };

    /// <summary>
    /// A view of the Match results for a single Team.
    /// </summary>
    /// <param name="teamRow"></param>    
    public record MatchResults : IComparable<MatchResults> {
        public TeamRow TeamRow { get; }
        public int Round { get; }
        public int Lane { get; }
        public int Ends { get; }
        public int BowlsFor { get; }
        public int TieBreaker { get; }
        public int BowlsAgainst { get; private set; }

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

        public MatchResults(TeamRow teamRow) {
            Debug.WriteLine($"new MatchResults({teamRow.Match.Round.UID}, {teamRow.Match.Lane}, {teamRow.Index})");

            this.TeamRow = teamRow;
            this.Round = teamRow.Match.Round;
            this.Lane = teamRow.Match.Lane;
            this.Ends = teamRow.Match.Ends;

            if (this.Result() == Views.Result.Bye) {
                RoundRow roundRow = TeamRow.Match.Round;
                this.BowlsFor = (int)roundRow.Matches
                                .SelectMany(match => match.Teams)
                                .Where(team => team.Members.Count > 0)
                                .Where(team => !team.Equals(teamRow))
                                .Select(team => team.Bowls)
                                .Average();

                this.BowlsAgainst = BowlsFor;
            }
            else {
                this.BowlsFor = teamRow.Bowls;
                this.BowlsAgainst = 0;
                this.TieBreaker = teamRow.Tie;
            }

            foreach (TeamRow t in teamRow.Match.Teams) {
                if (!t.Equals(teamRow)) this.BowlsAgainst += t.Bowls;
            }
        }

        public MatchResults(TeamRow teamRow, int bowlsFor, int bowlsAgainst) {
            this.TeamRow = teamRow;
            this.Round = teamRow.Match.Round;
            this.Lane = teamRow.Match.Lane;
            this.Ends = teamRow.Match.Ends;
            this.BowlsFor = bowlsFor;
            this.BowlsAgainst = bowlsAgainst;
            this.TieBreaker = teamRow.Tie;
        }

        public override string ToString() {
            return $"[{Round}, {Lane}, {Ends}, {BowlsFor}, {BowlsAgainst}, {TieBreaker}, {PointsFor}, {PlusFor}, {PointsAgainst}, {PlusAgainst}]";
        }

        public Result Result() {
            int nonEmptyTeams = TeamRow.Match.Teams.Where(t => t.Members.Count > 0).Count();

            if (nonEmptyTeams == 1) return Views.Result.Bye;
            if (this.BowlsFor > BowlsAgainst) return Views.Result.Win;
            if (this.BowlsFor < BowlsAgainst) return Views.Result.Loss;
            if (this.TieBreaker > 0) return Views.Result.Win;

            foreach (TeamRow t in TeamRow.Match.Teams) {
                if (!t.Equals(TeamRow)) continue;
                if (t.Tie > 0) return Views.Result.Loss;
            }

            return Views.Result.Tie;
        }

        public int CompareTo(MatchResults? that) {
            if (that is null) return 1;
            if (this.Round != that.Round) return this.Round - that.Round;
            if (this.Lane != that.Lane) return this.Lane - that.Lane;
            return 0;
        }
    }
}
