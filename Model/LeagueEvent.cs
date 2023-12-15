using System.Data;
using System.Diagnostics;

namespace Model.Tables {
    public class LeagueEvent : DataView{

        public string EventName { get; init; }

        private DataView View { get; }

        internal League League { get; }

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

        public DataRow AddRow(int round, int lane, int teamID = -1, int bowls = 0, int ends = 0, int tiebreaker = 0) {            
            var eRow = this.Table?.NewRow() ?? throw new ArgumentNullException(nameof(this.Table));

            eRow["event_name"] = this.EventName;
            eRow["round"] = round;
            eRow["lane"] = lane;
            eRow["team"] = teamID;
            eRow["bowls"] = bowls;
            eRow["tie"] = tiebreaker;
            eRow["ends"] = ends;

            this.Table.Rows.Add(eRow);
            return eRow;
        }

        public List<Round> GetRounds() {
            SortedSet<int> ids = [];
            List<Round> rounds = [];

            foreach (DataRow row in this.Table.AsEnumerable()) {
                int id = (row.Field<int>("round"));
                if (!ids.Contains(id)) continue;
                ids.Add(id);
                rounds.Add(this.GetRound(id));
            }

            return rounds;
        }

        public Round GetRound(int roundIndex) {
            return new Round(this, roundIndex) {
                RowFilter = $"event_name = '{this.EventName}' AND round = {roundIndex}"
            };
        }

        public string PrettyPrint() {
            return this.Table.PrettyPrint(this.View, this.EventName);
        }
    }
}
