using Leagueinator.Model;
using Leagueinator.Model.Tables;
using Leagueinator.Model.Views;
using System.Data;

namespace Model_Test {
    [TestClass]
    public class Team_Test {

        [TestMethod]
        public void Same_Hash_Code() {
            League league = new();
            EventRow eventRow = league.EventTable.AddRow("my_event");
            RoundRow roundRow = eventRow.Rounds.Add();
            MatchRow matchRow = roundRow.Matches.Add(0, 10);
            TeamRow teamRow1 = matchRow.Teams.Add(0);

            league.PlayerTable.AddRow("Adam");
            league.PlayerTable.AddRow("Eve");
            teamRow1.Members.Add("Adam");
            teamRow1.Members.Add("Eve");

            Team team1 = new(teamRow1);
            Team team2 = new(teamRow1);
            Console.WriteLine(team1.GetHashCode());
            Console.WriteLine(team2.GetHashCode());

            Assert.AreEqual(team1.GetHashCode(), team2.GetHashCode());
        }

        [TestMethod]
        public void Equals() {
            League league = new();
            EventRow eventRow = league.EventTable.AddRow("my_event");
            RoundRow roundRow = eventRow.Rounds.Add();
            MatchRow matchRow = roundRow.Matches.Add(0, 10);
            TeamRow teamRow1 = matchRow.Teams.Add(0);

            league.PlayerTable.AddRow("Adam");
            league.PlayerTable.AddRow("Eve");
            teamRow1.Members.Add("Adam");
            teamRow1.Members.Add("Eve");

            Team team1 = new(teamRow1);
            Team team2 = new(teamRow1);
            Console.WriteLine(team1.GetHashCode());
            Console.WriteLine(team2.GetHashCode());

            Assert.AreEqual(team1, team2);
        }
    }
}
