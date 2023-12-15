using System.Data;

namespace Model.Tables {
    public class Match : DataView{

        internal Round Round { get; }

        public int MatchIndex { get; }

        public new Team this[int index] {
            get => this.GetTeam(index);
        }

        public Match(Round round, int matchIndex) : base(round.Table){
            this.Round = round;
            this.MatchIndex = matchIndex;
            LeagueEvent lEvent = this.Round.LeagueEvent;
        }

        public Team GetTeam(int teamID) {
            return new Team(this, teamID);
        }

        public List<Team> Teams {
            get {
                List<Team> list = [];
                LeagueEvent lEvent = this.Round.LeagueEvent;

                var view = new DataView(lEvent.Table) {
                    RowFilter = $"event_name = '{lEvent}' AND round = {this.Round.RoundIndex} AND lane = {this.MatchIndex}"
                };

                foreach (DataRow row in view) {
                    list.Add(new Team(this, (int)row["lane"]));
                }

                return list;
            }
        }

        public string PrettyPrint() {
            return this.Table.PrettyPrint(this, $"Lane {MatchIndex} of Round {Round.RoundIndex}");
        }
    }
}
