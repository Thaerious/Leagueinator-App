using System.Data;

namespace Leagueinator.Model.Tables {
    public class EventBoundRounds(EventRow eventRow) : DataView(eventRow.League.RoundTable), IEnumerable<RoundRow> {

        private RoundTable RoundTable { get; } = eventRow.League.RoundTable;

        public RoundRow Add() => this.RoundTable.AddRow(eventRow.UID);

        public new RoundRow? this[int index] {
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
    }
}
