using Leagueinator.Model;

namespace Leagueinator {
    internal class MockModel : League {
        public MockModel() {
            var eventRow = this.EventTable.AddRow("My Event");
            eventRow.Settings["match_count"] = "4";

            eventRow.Rounds.Add().PopulateMatches();
            var roundRow = eventRow.Rounds.Add().PopulateMatches();

            this.PlayerTable.AddRow("Zed");
            this.PlayerTable.AddRow("Yyvonne");
            this.PlayerTable.AddRow("Adam");
            this.PlayerTable.AddRow("Eve");
            this.PlayerTable.AddRow("Cain");
            this.PlayerTable.AddRow("Able");

            roundRow.IdlePlayers.Add("Zed");
            roundRow.IdlePlayers.Add("Yyvonne");

            var matchRow = roundRow.Matches[0];
            var teamRow1 = matchRow.Teams.Add(1);
            var teamRow2 = matchRow.Teams.Add(2);

            roundRow.Matches[1].Teams.Add(1);
            roundRow.Matches[1].Teams.Add(2);

            teamRow1.Members.Add("Adam");
            teamRow2.Members.Add("Eve");
            teamRow1.Members.Add("Cain");
            teamRow2.Members.Add("Able");

            teamRow1.Bowls = 6;
            teamRow2.Bowls = 4;
        }
    }
}
