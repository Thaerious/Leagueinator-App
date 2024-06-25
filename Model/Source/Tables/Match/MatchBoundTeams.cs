using System.Data;

namespace Leagueinator.Model.Tables {
    public class MatchBoundTeams: DataView, IEnumerable<TeamRow> {

        internal MatchBoundTeams(MatchRow matchRow) : base(matchRow.League.TeamTable) {
            this.MatchRow = matchRow;
        }

        private TeamTable TeamTable => this.MatchRow.League.TeamTable;

        public TeamRow Add(int index) => this.TeamTable.AddRow(this.MatchRow.UID, index);

        public TeamRow Add() => this.TeamTable.AddRow(this.MatchRow.UID, this.MatchRow.Teams.Count);

        public TeamRow Get(int index) => this.TeamTable.GetRow(this.MatchRow.UID, index);

        public bool Has(int index) => this.TeamTable.HasRow(this.MatchRow.UID, index);

        public new TeamRow this[int index] {
            get => new(base[index].Row);
        }

        /// <summary>
        /// Enumerator for child rows.
        /// </summary>
        /// <returns></returns>
        public new IEnumerator<TeamRow> GetEnumerator() {
            for (int i = 0; i < this.Count; i++) {
                if (this[i] is not null) yield return this[i]!;
            }
        }

        private readonly MatchRow MatchRow;
    }
}
