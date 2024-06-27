using Leagueinator.Model.Tables;
using System.Data;

namespace Model.Source.Tables.Round {
    public class RoundBoundIdles: DataView, IEnumerable<IdleRow> {

        internal RoundBoundIdles(RoundRow roundRow) : base(roundRow.League.IdleTable) {
            this.RoundRow = roundRow;
            this.RowFilter = $"{IdleTable.COL.ROUND} = '{roundRow.UID}'";
        }

        private IdleTable IdleTable => this.RoundRow.League.IdleTable;

        public IdleRow Add(string name) => this.IdleTable.AddRow(this.RoundRow.UID, name);

        public IdleRow Get(string name) => this.IdleTable.GetRow(this.RoundRow.UID, name);

        public bool Has(string name) => this.IdleTable.HasRow(this.RoundRow.UID, name);

        public new IdleRow this[int index] {
            get => new(base[index].Row);
        }

        /// <summary>
        /// Enumerator for child rows.
        /// </summary>
        /// <returns></returns>
        public new IEnumerator<IdleRow> GetEnumerator() {
            for (int i = 0; i < this.Count; i++) {
                if (this[i] is not null) yield return this[i]!;
            }
        }

        private readonly RoundRow RoundRow;
    }
}
