using Model;
using System.Diagnostics;

namespace Model_Test {
    [TestClass]
    public class Team_Test {

        [TestMethod]
        public void Add_New_Player() {
            League league = new League();
            LeagueEvent lEvent = league.AddLeagueEvent("my_event");
            Round round = lEvent.NewRound();
            Match match = round.GetMatch(0);
            Team team = match.NewTeam();
            bool actual = team.AddPlayer("Adam");

            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void Add_Exising_Player() {
            League league = new League();
            LeagueEvent lEvent = league.AddLeagueEvent("my_event");
            Round round = lEvent.NewRound();
            Match match = round.GetMatch(0);
            Team team = match.NewTeam();
            team.AddPlayer("Adam");
            bool actual = team.AddPlayer("Adam");

            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void Has_Player_True() {
            League league = new League();
            LeagueEvent lEvent = league.AddLeagueEvent("my_event");
            Round round = lEvent.NewRound();
            Match match = round.GetMatch(0);
            Team team = match.NewTeam();
            team.AddPlayer("Adam");
            bool actual = team.HasPlayer("Adam");

            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void Has_Player_False() {
            League league = new League();
            LeagueEvent lEvent = league.AddLeagueEvent("my_event");
            Round round = lEvent.NewRound();
            Match match = round.GetMatch(0);
            Team team = match.NewTeam();
            bool actual = team.HasPlayer("Adam");

            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void Get_Players_Empty() {
            League league = new League();
            LeagueEvent lEvent = league.AddLeagueEvent("my_event");
            Round round = lEvent.NewRound();
            Match match = round.GetMatch(0);
            Team team = match.NewTeam();
            List<String> list = team.GetPlayers();

            Assert.AreEqual(0, list.Count);
        }

        [TestMethod]
        public void Get_Players_One() {
            League league = new League();
            LeagueEvent lEvent = league.AddLeagueEvent("my_event");
            Round round = lEvent.NewRound();
            Match match = round.GetMatch(0);
            Team team = match.NewTeam();
            team.AddPlayer("Adam");
            List<string> list = team.GetPlayers();

            Assert.AreEqual(1, list.Count);
            Assert.IsTrue(list.Contains("Adam"));
        }

        [TestMethod]
        public void Get_Players_Many() {
            League league = new League();
            LeagueEvent lEvent = league.AddLeagueEvent("my_event");
            Round round = lEvent.NewRound();
            Match match = round.GetMatch(0);
            Team team = match.NewTeam();
            team.AddPlayer("Adam");
            team.AddPlayer("Bart");
            team.AddPlayer("Carly");
            team.AddPlayer("Dianne");
            List<String> list = team.GetPlayers();

            Assert.AreEqual(4, list.Count);
            Assert.IsTrue(list.Contains("Adam"));
            Assert.IsTrue(list.Contains("Bart"));
            Assert.IsTrue(list.Contains("Carly"));
            Assert.IsTrue(list.Contains("Dianne"));
        }

    }
}
