using Model;
using Model.Tables;

namespace Model.Scoring.Plus {
    public class PlusScore {
        private readonly LeagueEvent LeagueEvent;
        public readonly PlusSummary PlusSummary;
        public readonly PlusRounds PlusRounds;
        public readonly PlusTeams PlusTeams;

        public PlusScore(LeagueEvent leagueEvent) {
            this.LeagueEvent = leagueEvent;
            this.PlusRounds = new(leagueEvent);            
            this.PlusTeams = new();
            this.FillTeams();
            this.PlusSummary = new(this.PlusRounds);
        }              

        private void FillTeams() {
            foreach (Round round in this.LeagueEvent.Rounds) {
                foreach (Match match in round.Matches) {
                    foreach (Team team in match.Teams) {
                        int index = this.PlusTeams.AddTeamIf(team.Players);
                        var row = this.PlusRounds.GetRow(team.EventTableUID);
                        row[RoundTable.COL.TEAM_IDX] = index;
                    }
                }
            }
        }

        public string PrettyPrint() {
            return this.PlusSummary.PrettyPrint() + 
                   this.PlusRounds.PrettyPrint() + 
                   this.PlusTeams.PrettyPrint();
        }
    }
}
