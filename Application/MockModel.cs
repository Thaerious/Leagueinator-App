using Model;

namespace Leagueinator {
    internal class MockModel : League{

        public MockModel() {
            var eventRow = this.EventTable.AddRow("My Event");
            eventRow.Settings["match_count"] = "4";

            eventRow.Rounds.Add().PopulateMatches();
            var roundRow = eventRow.Rounds.Add().PopulateMatches();

            roundRow.IdlePlayers.Add("Zed");
            roundRow.IdlePlayers.Add("Yyvonne");

            var matchRow = roundRow.Matches[0];
            var teamRow1 = matchRow.Teams.Add();
            var teamRow2 = matchRow.Teams.Add();
            
            roundRow.Matches[1].Teams.Add();
            roundRow.Matches[1].Teams.Add();

            teamRow1.Members.Add("Adam");
            teamRow2.Members.Add("Eve");
        }
    }
}
