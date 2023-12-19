using Model.Tables;
using System.Data;

namespace Model {
    /// <summary>
    /// A view of EventTable restricted to event.
    /// The public methods do not directly change the data set.
    /// </summary>
    public class LeagueEvent : DataView, IDeleted {

        public string EventName { get; init; }

        public string EventDate { get => (string)this.DirectoryRow[EventDirectoryTable.COL.DATE]; }

        public League League { get; }

        private DataRow DirectoryRow { get; }

        public bool Deleted { get; private set; } = false;

        public List<Round> Rounds {
            get => this.GetRounds();
        }

        internal LeagueEvent(League league, string eventName) : base(league.EventTable) {
            this.EventName = eventName;
            this.League = league;

            league.EventDirectoryTable.DefaultView.Sort = EventDirectoryTable.COL.EVENT_NAME;
            int index = league.EventDirectoryTable.DefaultView.Find(eventName);
            if (index == -1) throw new KeyNotFoundException($"Event Name: {eventName}");
            this.DirectoryRow = league.EventDirectoryTable.Rows[index];

            this.RowFilter = $"event_name = '{eventName}'";
        }

        internal DataRow AddRow(int round, int lane, int teamIDX) {
            return this.League.EventTable.AddRow(
                eventName: this.EventName,
                round: round,
                lane: lane,
                teamIdx: teamIDX
            );
        }

        private List<Round> GetRounds() {
            DeletedException.ThrowIf(this);

            DataTable table = this.Table ?? throw new NullReferenceException();
            List<Round> rounds = [];

            for (int i = 0; i < this.RoundCount; i++) {
                rounds.Add(this.GetRound(i));
            }

            return rounds;
        }

        private Round GetRound(int roundIndex) {
            DeletedException.ThrowIf(this);

            return new Round(this, roundIndex);
        }

        /// <summary>
        /// Create a new Round and add it to the Round directory.
        /// </summary>
        /// <returns></returns>
        public Round NewRound() {
            int count = RoundCount;
            this.RoundCount = count + 1;
            return GetRound(count);
        }

        public int RoundCount {
            get {
                DeletedException.ThrowIf(this);
                return (int)this.DirectoryRow[EventDirectoryTable.COL.ROUND_COUNT];
            }
            internal set {
                this.DirectoryRow[EventDirectoryTable.COL.ROUND_COUNT] = value;
            }
        }

        public string PrettyPrint() {
            return this.Table.PrettyPrint(this, this.EventName) + "\n" +
                   this.DirectoryRow.PrettyPrint();
        }

        public void Delete() {
            DeletedException.ThrowIf(this);
            foreach (Round round in this.Rounds) round.Delete();
            League.EventDirectoryTable.Rows.Remove(this.DirectoryRow);
            this.Deleted = true;
        }
    }
}
