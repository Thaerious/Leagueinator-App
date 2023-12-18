using Model;
using System.Diagnostics;

namespace Model_Test {
    [TestClass]
    public class Integration_Test {
        public static League Mock() {
            League league = new();
            LeagueEvent myEvent = league.AddLeagueEvent("my_event");
            LeagueEvent myOtherEvent = league.AddLeagueEvent("my_other_event");
            
            myEvent.NewRound();
            myEvent.NewRound();
            myEvent.NewRound();

            myEvent.Rounds[0].GetMatch(0).NewTeam();
            myEvent.Rounds[0].GetMatch(0).NewTeam();

            myEvent.Rounds[0].GetMatch(0).Teams[0].AddPlayer("Adam");
            myEvent.Rounds[0].GetMatch(0).Teams[0].AddPlayer("Betty");
            myEvent.Rounds[0].GetMatch(0).Teams[1].AddPlayer("Chuck");
            myEvent.Rounds[0].GetMatch(0).Teams[1].AddPlayer("Dianne");

            return league;
        }

        [TestMethod]
        public void Constructor_Sanity() {
            League league = Mock();
            Debug.WriteLine(league.PrettyPrint());
            Assert.IsTrue(league != null);
        }

        [TestMethod]
        public void Count_Rounds_From_League_Event() {
            League league = Mock();
            LeagueEvent lEvent = league.LeagueEvents["my_event"];
            Debug.WriteLine(lEvent.PrettyPrint());
            Assert.AreEqual(3, lEvent.Rounds.Count);
        }
    }
}
