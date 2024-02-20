using Model;
using Model.Tables;
using System.Data;

namespace Model_Test {
    [TestClass]
    public class PlayerTest {

        [TestMethod]
        public void PlayerRow() {
            League league = new();
            EventRow eventRow = league.EventTable.AddRow("my_event");
            RoundRow roundRow = eventRow.Rounds.Add();
            MatchRow matchRow = roundRow.Matches.Add(0, 10);
            TeamRow teamRow = matchRow.Teams.Add();
            PlayerRow playerRow = teamRow.Players.Add("Adam");

            Assert.IsNotNull(playerRow);
        }

        [TestMethod]
        public void Cast_PlayerRow_String() {
            League league = new();
            EventRow eventRow = league.EventTable.AddRow("my_event");
            RoundRow roundRow = eventRow.Rounds.Add();
            MatchRow matchRow = roundRow.Matches.Add(0, 10);
            TeamRow teamRow = matchRow.Teams.Add();
            PlayerRow playerRow = teamRow.Players.Add("Adam");
            string asString = (string)playerRow;

            Assert.IsTrue(asString is string);
        }
    }
}
