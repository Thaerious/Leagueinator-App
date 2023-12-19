using Model;
using Model.Tables;
using System.Data;
using System.Diagnostics;

namespace Model_Test {
    [TestClass]
    public class LeagueEvent_Test {

        [TestMethod]
        public void League() {
            League league = new();
            LeagueEvent lEvent = league.NewLeagueEvent("my_event");

            Assert.AreEqual(league, lEvent.League);
        }

        [TestMethod]
        public void Sanity_Check() {
            League league = new();
            LeagueEvent lEvent = league.NewLeagueEvent("my_event");
            Assert.IsNotNull(lEvent);
        }

        [TestMethod]
        public void Event_Name_Matched() {
            League league = new();
            LeagueEvent lEvent = league.NewLeagueEvent("my_event");
            Assert.AreEqual("my_event", lEvent.EventName);
        }

        [TestMethod]
        public void New_Round() {
            League league = new();
            LeagueEvent lEvent = league.NewLeagueEvent("my_event");
            Round round = lEvent.NewRound();
            Assert.IsNotNull(lEvent);
            Assert.AreEqual(0, round.RoundIndex);
        }

        [TestMethod]
        public void Round_Count_Empty() {
            League league = new();
            LeagueEvent lEvent = league.NewLeagueEvent("my_event");
            Assert.IsNotNull(lEvent);
            Assert.AreEqual(0, lEvent.RoundCount);
        }

        [TestMethod]
        public void Round_Count_One() {
            League league = new();
            LeagueEvent lEvent = league.NewLeagueEvent("my_event");
            lEvent.NewRound();
            Assert.AreEqual(1, lEvent.RoundCount);
        }

        [TestMethod]
        public void Round_Count_Many() {
            League league = new();
            LeagueEvent lEvent = league.NewLeagueEvent("my_event");
            lEvent.NewRound();
            lEvent.NewRound();
            lEvent.NewRound();
            lEvent.NewRound();
            lEvent.NewRound();
            Assert.AreEqual(5, lEvent.RoundCount);
        }

        [TestMethod]
        public void All_Rounds() {
            League league = new();
            LeagueEvent lEvent = league.NewLeagueEvent("my_event");
            lEvent.NewRound();
            Assert.AreEqual(1, lEvent.Rounds.Count);
        }

        [TestMethod]
        public void Get_Round() {
            League league = new();
            LeagueEvent lEvent = league.NewLeagueEvent("my_event");
            lEvent.NewRound();
            Assert.IsNotNull(lEvent.Rounds[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Get_Round_Does_Not_Exist() {
            League league = new();
            LeagueEvent lEvent = league.NewLeagueEvent("my_event");
            Assert.IsNotNull(lEvent.Rounds[0]);
        }

        [TestMethod]
        public void Delete() {
            League league = new League();
            LeagueEvent lEvent = league.NewLeagueEvent("my_event");
            Round round = lEvent.NewRound();
            Match match = round.GetMatch(0);
            match.NewTeam();
            match.NewTeam();
            match.NewTeam();

            lEvent.Delete();

            Assert.IsTrue(lEvent.Deleted);
            Assert.AreEqual(0, league.EventDirectoryTable.Rows.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(DeletedException))]
        public void Delete_Twice() {
            League league = new League();
            LeagueEvent lEvent = league.NewLeagueEvent("my_event");
            lEvent.Delete();
            lEvent.Delete();
        }
    }
}
