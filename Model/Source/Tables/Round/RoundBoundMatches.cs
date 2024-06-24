using Leagueinator.Model.Tables;
using Leagueinator.Model.Views;
using System.Data;

namespace Model.Source.Tables.Round {
    public class RoundBoundMatches(RoundRow roundRow) : DataView, IEnumerable<MatchRow> {

        private MatchTable ChildTable { get; } = roundRow.League.MatchTable;

        public MatchRow Add(int lane, int ends) => this.ChildTable.AddRow(roundRow.UID, lane, ends);

        public MatchRow Get(int lane) => this.ChildTable.GetRow(roundRow.UID, lane);

        public bool Has(int lane) => this.ChildTable.HasRow(roundRow.UID, lane);

        public new MatchRow? this[int index] {
            get => new(base[index].Row);
        }


        /// <summary>
        /// Enumerator for child rows.
        /// </summary>
        /// <returns></returns>
        public new IEnumerator<MatchRow> GetEnumerator() {
            for (int i = 0; i < this.Count; i++) {
                if (this[i] is not null) yield return this[i]!;
            }
        }
    }
}
