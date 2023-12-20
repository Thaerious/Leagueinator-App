using Model;
using System.Diagnostics;

namespace Model_Test {
    [TestClass]
    public class Integration_Test {
        public static League Mock() {
            League league = new();
            LeagueEvent myEvent = league.NewLeagueEvent("my_event");
            LeagueEvent myOtherEvent = league.NewLeagueEvent("my_other_event");
            
            myEvent.NewRound();
            myEvent.NewRound();
            myEvent.NewRound();

            myEvent.Rounds[0].GetMatch(0).NewTeam();
            myEvent.Rounds[0].GetMatch(0).NewTeam();

            myEvent.Rounds[0].GetMatch(0).Teams[0].AddPlayer("Adam");
            myEvent.Rounds[0].GetMatch(0).Teams[0].AddPlayer("Betty");
            myEvent.Rounds[0].GetMatch(0).Teams[1].AddPlayer("Chuck");
            myEvent.Rounds[0].GetMatch(0).Teams[1].AddPlayer("Dianne");

            myOtherEvent.NewRound();
            myOtherEvent.Rounds[0].GetMatch(0).NewTeam();
            myOtherEvent.Rounds[0].GetMatch(0).NewTeam();

            myEvent.Rounds[0].GetMatch(0).Teams[0].AddPlayer("Adam");
            myEvent.Rounds[0].GetMatch(0).Teams[0].AddPlayer("Dianne");
            myEvent.Rounds[0].GetMatch(0).Teams[1].AddPlayer("Chuck");
            myEvent.Rounds[0].GetMatch(0).Teams[1].AddPlayer("Betty");

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
            LeagueEvent lEvent = league.GetLeagueEvent(0);
            Debug.WriteLine(lEvent.PrettyPrint());
            Assert.AreEqual(3, lEvent.Rounds.Count);
        }
    }
}
