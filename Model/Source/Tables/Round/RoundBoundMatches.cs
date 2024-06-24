using Leagueinator.Model.Tables;
using Leagueinator.Model.Views;
using System.Data;

namespace Model.Source.Tables.Round {
    public class RoundBoundMatches(RoundRow roundRow) : DataView(roundRow.League.MatchTable), IEnumerable<MatchRow> {

        private MatchTable MatchTable { get; } = roundRow.League.MatchTable;

        public MatchRow Add(int lane, int ends) => this.MatchTable.AddRow(roundRow.UID, lane, ends);

        public MatchRow Get(int lane) => this.MatchTable.GetRow(roundRow.UID, lane);

        public bool Has(int lane) => this.MatchTable.HasRow(roundRow.UID, lane);

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
