using System.Data;

namespace Model.Tables {
    public class Round : DataView {
        public LeagueEvent LeagueEvent { get; }

        public int RoundIndex { get; }

        public Round(LeagueEvent lEvent, int roundIndex) : base(lEvent.Table) {
            this.LeagueEvent = lEvent;
            this.RoundIndex = roundIndex;
        }

        public Match GetMatch(int matchIndex) {
            return new Match(this, matchIndex) {
                RowFilter = $"event_name = '{this.LeagueEvent.EventName}' AND round = {this.RoundIndex} AND lane = {matchIndex}"
            };
        }

        public string PrettyPrint() {
            return this.Table.PrettyPrint(this, $"Match {RoundIndex} of {LeagueEvent.EventName}");
        }
    }
}
