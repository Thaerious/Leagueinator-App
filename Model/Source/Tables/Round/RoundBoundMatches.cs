using Leagueinator.Model.Tables;
using Leagueinator.Model.Views;
using System.Data;

namespace Model.Source.Tables.Round {
    public class RoundBoundMatches : DataView, IEnumerable<MatchRow> {

        internal RoundBoundMatches(RoundRow roundRow) : base(roundRow.League.MatchTable) {
            this.RoundRow = roundRow;
        }

        private MatchTable MatchTable => this.RoundRow.League.MatchTable;

        public MatchRow Add(int lane, int ends) => this.MatchTable.AddRow(this.RoundRow.UID, lane, ends);

        public MatchRow Add(int lane) => this.MatchTable.AddRow(this.RoundRow.UID, lane, this.RoundRow.Event.EndsDefault);

        public MatchRow Get(int lane) => this.MatchTable.GetRow(this.RoundRow.UID, lane);

        public bool Has(int lane) => this.MatchTable.HasRow(this.RoundRow.UID, lane);

        public new MatchRow this[int index] {
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

        private readonly RoundRow RoundRow;
    }
}
