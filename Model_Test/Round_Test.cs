using Model;
using Model.Tables;
using System.Data;
using System.Diagnostics;

namespace Model_Test {
    [TestClass]
    public class Round_Test {

        [TestMethod]
        public void League_Event() {
            League league = new League();
            LeagueEvent lEvent = league.AddLeagueEvent("my_event");
            Round round = lEvent.NewRound();

            Assert.IsNotNull(round.LeagueEvent);
            Assert.AreEqual(lEvent, round.LeagueEvent);
        }

        [TestMethod]
        public void Empty_Round() {
            League league = new League();
            LeagueEvent lEvent = league.AddLeagueEvent("my_event");
            Round round = lEvent.NewRound();

            Assert.IsNotNull(round);
        }

        [TestMethod]
        public void Empty_Round_Get_Matches() {
            League league = new League();
            LeagueEvent lEvent = league.AddLeagueEvent("my_event");
            Round round = lEvent.NewRound();

            Assert.AreEqual(round.Matches.Count, 0);
        }

        [TestMethod]
        public void First_Round_Index() {
            League league = new League();
            LeagueEvent lEvent = league.AddLeagueEvent("my_event");
            Round round = lEvent.NewRound();

            Assert.AreEqual(0, round.RoundIndex);
        }

        [TestMethod]
        public void Second_Round_Index() {
            League league = new League();
            LeagueEvent lEvent = league.AddLeagueEvent("my_event");
            lEvent.NewRound();
            Round round = lEvent.NewRound();

            Assert.AreEqual(1, round.RoundIndex);
        }

        /// <summary>
        /// Getting a match doesn't update the table.
        /// </summary>
        [TestMethod]
        public void Empty_Round_Add_Match() {
            League league = new League();
            LeagueEvent lEvent = league.AddLeagueEvent("my_event");
            Round round = lEvent.NewRound();
            round.GetMatch(0);

            Assert.AreEqual(round.Matches.Count, 0);
        }

        /// <summary>
        /// Matches with players will be returned by GetMatches.
        /// </summary>
        [TestMethod]
        public void Update_Match_Get_Matches() {
            League league = new League();
            LeagueEvent lEvent = league.AddLeagueEvent("my_event");
            Round round = lEvent.NewRound();
            Match match = round.GetMatch(0);
            match.NewTeam();

            Assert.AreEqual(round.Matches.Count, 1);
        }
    }
}
