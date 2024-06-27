using System.Data;

namespace Leagueinator.Model.Tables {
    public class EventBoundRounds : DataView, IEnumerable<RoundRow> {

        internal EventBoundRounds(EventRow eventRow) : base(eventRow.League.RoundTable) {
            this.EventRow = eventRow;
            this.RowFilter = $"{RoundTable.COL.EVENT} = '{eventRow.UID}'";
        }

        private RoundTable RoundTable => this.EventRow.League.RoundTable;

        public RoundRow Add() => this.RoundTable.AddRow(this.EventRow.UID);

        public RoundRow Get(int index) => this.RoundTable.GetRow(this.EventRow.UID, index);

        public bool Has(int index) => this.RoundTable.HasRow(this.EventRow.UID, index);

        public new RoundRow this[int index] {
            get => new(base[index].Row);
        }

        /// <summary>
        /// Enumerator for child rows.
        /// </summary>
        /// <returns></returns>
        public new IEnumerator<RoundRow> GetEnumerator() {
            for (int i = 0; i < this.Count; i++) {
                if (this[i] is not null) yield return this[i]!;
            }
        }

        private readonly EventRow EventRow;
    }
}
