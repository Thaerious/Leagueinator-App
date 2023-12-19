using Model;
using Model.Tables;
using System.Diagnostics;

namespace Model_Test {
    [TestClass]
    public class Team_Test {

        [TestMethod]
        public void Match() {
            League league = new League();
            LeagueEvent lEvent = league.NewLeagueEvent("my_event");
            Round round = lEvent.NewRound();
            Match match = round.GetMatch(0);
            Team team = match.NewTeam();
            Assert.AreEqual(match, team.Match);
        }

        [TestMethod]
        public void Index_0() {
            League league = new League();
            LeagueEvent lEvent = league.NewLeagueEvent("my_event");
            Round round = lEvent.NewRound();
            Match match = round.GetMatch(0);
            Team team = match.NewTeam();            
            Assert.AreEqual(0, team.TeamIndex);
        }

        [TestMethod]
        public void Index_1() {
            League league = new League();
            LeagueEvent lEvent = league.NewLeagueEvent("my_event");
            Round round = lEvent.NewRound();
            Match match = round.GetMatch(0);
            match.NewTeam();
            Team team = match.NewTeam();
            Assert.AreEqual(1, team.TeamIndex);
        }

        [TestMethod]
        public void Add_New_Player() {
            League league = new League();
            LeagueEvent lEvent = league.NewLeagueEvent("my_event");
            Round round = lEvent.NewRound();
            Match match = round.GetMatch(0);
            Team team = match.NewTeam();
            bool actual = team.AddPlayer("Adam");

            Debug.WriteLine(league.PrettyPrint());

            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void Add_Exising_Player() {
            League league = new League();
            LeagueEvent lEvent = league.NewLeagueEvent("my_event");
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
            LeagueEvent lEvent = league.NewLeagueEvent("my_event");
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
            LeagueEvent lEvent = league.NewLeagueEvent("my_event");
            Round round = lEvent.NewRound();
            Match match = round.GetMatch(0);
            Team team = match.NewTeam();
            bool actual = team.HasPlayer("Adam");

            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void Get_Players_Empty() {
            League league = new League();
            LeagueEvent lEvent = league.NewLeagueEvent("my_event");
            Round round = lEvent.NewRound();
            Match match = round.GetMatch(0);
            Team team = match.NewTeam();
            List<String> list = team.Players;

            Assert.AreEqual(0, list.Count);
        }

        [TestMethod]
        public void Get_Players_One() {
            League league = new League();
            LeagueEvent lEvent = league.NewLeagueEvent("my_event");
            Round round = lEvent.NewRound();
            Match match = round.GetMatch(0);
            Team team = match.NewTeam();
            team.AddPlayer("Adam");
            List<string> list = team.Players;

            Assert.AreEqual(1, list.Count);
            Assert.IsTrue(list.Contains("Adam"));
        }

        [TestMethod]
        public void Get_Players_Many() {
            League league = new League();
            LeagueEvent lEvent = league.NewLeagueEvent("my_event");
            Round round = lEvent.NewRound();
            Match match = round.GetMatch(0);
            Team team = match.NewTeam();
            team.AddPlayer("Adam");
            team.AddPlayer("Bart");
            team.AddPlayer("Carly");
            team.AddPlayer("Dianne");
            List<String> list = team.Players;

            Assert.AreEqual(4, list.Count);
            Assert.IsTrue(list.Contains("Adam"));
            Assert.IsTrue(list.Contains("Bart"));
            Assert.IsTrue(list.Contains("Carly"));
            Assert.IsTrue(list.Contains("Dianne"));
        }

        [TestMethod]
        public void Remove_Player_Exists() {
            League league = new League();
            LeagueEvent lEvent = league.NewLeagueEvent("my_event");
            Round round = lEvent.NewRound();
            Match match = round.GetMatch(0);
            Team team = match.NewTeam();
            team.AddPlayer("Adam");

            bool result = team.RemovePlayer("Adam");
            List<string> list = team.Players;

            Assert.AreEqual(0, list.Count);
            Assert.IsFalse(list.Contains("Adam"));
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Remove_Player_Not_Exists() {
            League league = new League();
            LeagueEvent lEvent = league.NewLeagueEvent("my_event");
            Round round = lEvent.NewRound();
            Match match = round.GetMatch(0);
            Team team = match.NewTeam();
            bool result = team.RemovePlayer("Adam");
            List<string> list = team.Players;

            Assert.AreEqual(0, list.Count);
            Assert.IsFalse(list.Contains("Adam"));
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Remove_Player_Multiple_Matches() {
            League league = new League();
            LeagueEvent lEvent = league.NewLeagueEvent("my_event");

            lEvent.NewRound()
                .GetMatch(0)
                .NewTeam()
                .AddPlayer("Adam");

            lEvent.NewRound()
                .GetMatch(0)
                .NewTeam()
                .AddPlayer("Adam");

            bool result = lEvent.Rounds[0].GetMatch(0).Teams[0].RemovePlayer("Adam");

            // player is removed from first round
            List<string> list1 = lEvent.Rounds[0].GetMatch(0).Teams[0].Players;
            Assert.AreEqual(0, list1.Count);
            Assert.IsFalse(list1.Contains("Adam"));
            Assert.IsTrue(result);

            // player is not removed from second round
            List<string> list2 = lEvent.Rounds[1].GetMatch(0).Teams[0].Players;
            Assert.AreEqual(1, list2.Count);
            Assert.IsTrue(list2.Contains("Adam"));
        }
    }
}
