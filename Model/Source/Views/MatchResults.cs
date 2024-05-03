using Leagueinator.Model.Tables;
using System.Diagnostics;

namespace Leagueinator.Model.Views {
    
    /// <summary>
    /// A view of the Match results for a single TeamRow.
    /// </summary>
    /// <param name="teamRow"></param>    
    public record MatchResults {
        private readonly TeamRow TeamRow;
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
            this.TeamRow = teamRow;
            this.Round = teamRow.Match.Round;
            this.Lane = teamRow.Match.Lane;
            this.Ends = teamRow.Match.Ends;
            this.BowlsFor = teamRow.Bowls;
            this.BowlsAgainst = 0;
            this.TieBreaker = teamRow.Tie;

            foreach (TeamRow t in teamRow.Match.Teams) {
                if (!t.Equals(teamRow)) this.BowlsAgainst += t.Bowls;
            }
        }

        public override string ToString() {
            return $"[{Round}, {Lane}, {Ends}, {BowlsFor}, {BowlsAgainst}, {TieBreaker}, {PointsFor}, {PlusFor}, {PointsAgainst}, {PlusAgainst}]";
        }

        public readonly static IComparer<MatchResults> CompareByRound = new RoundCompare();

        private class RoundCompare : IComparer<MatchResults> {
            public int Compare(MatchResults x, MatchResults y) {
                if (x.Round != y.Round) return x.Round - y.Round;
                if (x.Lane != y.Lane) return x.Lane - y.Lane;
                return 0;
            }
        }

        public bool IsWin() {
            if (this.BowlsFor > BowlsAgainst) return true;
            return this.TieBreaker > 0;
        }
    }
}
