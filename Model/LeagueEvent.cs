using Model.Tables;
using System.Data;

namespace Model {
    /// <summary>
    /// A view of EventTable restricted to event.
    /// The public methods do not directly change the data set.
    /// </summary>
    public class LeagueEvent : DataView{

        public string EventName { get; init; }

        private DataView View { get; }

        public League League { get; }

        public List<Round> Rounds { 
            get => this.GetRounds();
        }

        internal LeagueEvent(League league, string eventName) : base(league.EventTable){
            this.EventName = eventName;
            this.League = league;

            this.View = new DataView(this.Table) {
                RowFilter = $"event_name = '{eventName}'"
            };
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
            DataTable table = this.Table ?? throw new NullReferenceException();
            List<Round> rounds = [];

            for (int i = 0; i <= this.LastRoundIndex; i++) {
                rounds.Add(this.GetRound(i));
            }

            return rounds;
        }

        private Round GetRound(int roundIndex) {
            return new Round(this, roundIndex) {
                RowFilter = $"event_name = '{this.EventName}' AND round = {roundIndex}"
            };
        }

        /// <summary>
        /// Create a new round and add it to the round directory.
        /// </summary>
        /// <returns></returns>
        public Round NewRound() {
            int count = LastRoundIndex;
            SetRoundIndex(count + 1);
            return GetRound(count + 1);
        }

        public int LastRoundIndex {
            get {
                var table = League.RoundDirectoryTable;
                string filter = $"{RoundDirectoryTable.COL.EVENT_NAME} = '{this.EventName}'";
                var rows = table.Select(filter);
                if (rows.Length == 0) throw new KeyNotFoundException(filter);

                return (int)rows[0][RoundDirectoryTable.COL.ROUND_COUNT];
            }
        }

        private void SetRoundIndex(int value) {
            var table = League.RoundDirectoryTable;
            string filter = $"{RoundDirectoryTable.COL.EVENT_NAME} = '{this.EventName}'";
            var rows = table.Select(filter);
            int count = LastRoundIndex;

            rows[0][RoundDirectoryTable.COL.ROUND_COUNT] = value;
        }

        public string PrettyPrint() {
            return this.Table.PrettyPrint(this.View, this.EventName);
        }
    }
}
