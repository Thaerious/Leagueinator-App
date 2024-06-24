using Leagueinator.Model.Tables;
using System.Data;

namespace Model.Source.Tables.Round {
    public class MatchBoundTeams(MatchRow matchRow): DataView, IEnumerable<TeamRow> {

        private TeamTable ChildTable { get; } = matchRow.League.TeamTable;

        public TeamRow Add(int index) => this.ChildTable.AddRow(matchRow.UID, index);

        public TeamRow Get(int index) => this.ChildTable.GetRow(matchRow.UID, index);

        public bool Has(int index) => this.ChildTable.HasRow(matchRow.UID, index);

        public new TeamRow? this[int index] {
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
    }
}
