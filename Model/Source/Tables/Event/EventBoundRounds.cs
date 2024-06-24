using Leagueinator.Model.Views;
using System.Data;

namespace Leagueinator.Model.Tables {
    public class EventBoundRounds(EventRow eventRow) : DataView, IEnumerable<RoundRow> {

        public RoundTable ChildTable { get; } = eventRow.League.RoundTable;

        public EventRow SourceRow { get; } = eventRow;

        public RoundRow Add() => this.ChildTable.AddRow(eventRow.UID);

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
