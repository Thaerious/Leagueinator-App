using Model;
using Model.Tables;

namespace Model_Test {
    [TestClass]
    public class ReflectedRowList_Test {
        [TestMethod]
        public void AsView() {
            League league = new();
            EventRow eventRow = league.EventTable.AddRow("my_event");
            RoundRow roundRow = eventRow.Rounds.Add();
            MatchRow matchRow = roundRow.Matches.Add(0, 10);
            TeamRow teamRow = matchRow.Teams.Add(0);

            league.PlayerTable.AddRow("Adam");
            league.PlayerTable.AddRow("Eve");
            league.PlayerTable.AddRow("Cain");

            teamRow.Members.Add("Adam");
            teamRow.Members.Add("Eve");
            teamRow.Members.Add("Cain");

            // it's just it's own view now
            Console.WriteLine(teamRow.Members.PrettyPrint());
        }
    }
}
