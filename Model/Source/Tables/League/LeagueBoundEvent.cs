using Leagueinator.Model.Tables;
using System.Data;

namespace Leagueinator.Model {
    public class LeagueBoundEvent : DataView, IEnumerable<EventRow> {

        internal LeagueBoundEvent(League league) : base(league.EventTable) {
            this.League = league;
        }

        private EventTable EventTable => this.League.EventTable;

        public EventRow Add(string eventName, string? date = null) => this.EventTable.AddRow(eventName, date);

        public EventRow Get(string eventName) => this.EventTable.GetRow(eventName);

        public bool Has(string eventName) => this.EventTable.HasRow(eventName);

        public new EventRow this[int index] {
            get => new(base[index].Row);
        }

        /// <summary>
        /// Enumerator for child rows.
        /// </summary>
        /// <returns></returns>
        public new IEnumerator<EventRow> GetEnumerator() {
            for (int i = 0; i < this.Count; i++) {
                if (this[i] is not null) yield return this[i]!;
            }
        }

        private readonly League League;
    }
}
