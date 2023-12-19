using Leagueinator.Utility;
using Model;
using System.Diagnostics;

namespace Model_Test {
    [TestClass]
    public class Round_Test {

        [TestMethod]
        public void League_Event() {
            League league = new League();
            LeagueEvent lEvent = league.NewLeagueEvent("my_event");
            Round round = lEvent.NewRound();

            Assert.IsNotNull(round.LeagueEvent);
            Assert.AreEqual(lEvent, round.LeagueEvent);
        }

        [TestMethod]
        public void Empty_Round() {
            League league = new League();
            LeagueEvent lEvent = league.NewLeagueEvent("my_event");
            Round round = lEvent.NewRound();

            Assert.IsNotNull(round);
        }

        [TestMethod]
        public void Empty_Round_Get_Matches() {
            League league = new League();
            LeagueEvent lEvent = league.NewLeagueEvent("my_event");
            Round round = lEvent.NewRound();

            Assert.AreEqual(round.Matches.Count, 0);
        }

        [TestMethod]
        public void First_Round_Index() {
            League league = new League();
            LeagueEvent lEvent = league.NewLeagueEvent("my_event");
            Round round = lEvent.NewRound();

            Assert.AreEqual(0, round.RoundIndex);
        }

        [TestMethod]
        public void Second_Round_Index() {
            League league = new League();
            LeagueEvent lEvent = league.NewLeagueEvent("my_event");
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
            LeagueEvent lEvent = league.NewLeagueEvent("my_event");
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
            LeagueEvent lEvent = league.NewLeagueEvent("my_event");
            Round round = lEvent.NewRound();
            Match match = round.GetMatch(0);
            match.NewTeam();

            Assert.AreEqual(round.Matches.Count, 1);
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

            Debug.WriteLine(league.PrettyPrint());
            round.Delete();
            Debug.WriteLine(league.PrettyPrint());

            Assert.AreEqual(0, lEvent.RoundCount);
        }

        [TestMethod]
        public void Delete_Team() {
            League league = new League();
            LeagueEvent lEvent = league.NewLeagueEvent("my_event");
            Round round = lEvent.NewRound();
            Match match = round.GetMatch(0);
            Team team = match.NewTeam();
            team.AddPlayer("Adam");
            team.Delete();

            Assert.IsTrue(team.Deleted);
        }

        [TestMethod]
        [ExpectedException(typeof(DeletedException))]
        public void Delete_Team_Twice() {
            League league = new League();
            LeagueEvent lEvent = league.NewLeagueEvent("my_event");
            Round round = lEvent.NewRound();
            Match match = round.GetMatch(0);
            Team team = match.NewTeam();
            team.AddPlayer("Adam");
            team.Delete();
            team.Delete();

            List<string> list = team.Players;

            Assert.AreEqual(0, list.Count);
            Assert.AreEqual(0, match.Teams.Count);
        }

        [TestMethod]
        public void Idle_Add_Contains() {
            League league = new League();
            LeagueEvent lEvent = league.NewLeagueEvent("my_event");
            Round round = lEvent.NewRound();
            Match match = round.GetMatch(0);
            Team team = match.NewTeam();

            round.IdlePlayers.Add("Zen");

            Assert.IsTrue(round.IdlePlayers.Contains("Zen"));
        }

        [TestMethod]
        public void Idle_Remove_Contains() {
            League league = new League();
            LeagueEvent lEvent = league.NewLeagueEvent("my_event");
            Round round = lEvent.NewRound();
            Match match = round.GetMatch(0);
            Team team = match.NewTeam();

            round.IdlePlayers.Add("Zen");
            round.IdlePlayers.Remove("Zen");

            Assert.IsFalse(round.IdlePlayers.Contains("Zen"));
        }

        [TestMethod]
        public void Idle_Iterator() {
            League league = new League();
            LeagueEvent lEvent = league.NewLeagueEvent("my_event");
            Round round = lEvent.NewRound();
            Match match = round.GetMatch(0);
            Team team = match.NewTeam();

            round.IdlePlayers.Add("Zen");

            foreach (string player in round.IdlePlayers.Cast<string>()) {
                Assert.AreEqual("Zen", player);
            }
        }

        [TestMethod]
        public void AllPlayers() {
            League league = new League();
            LeagueEvent lEvent = league.NewLeagueEvent("my_event");
            Round round = lEvent.NewRound();
            Match match = round.GetMatch(0);

            Team team1 = match.NewTeam();
            team1.AddPlayer("Adam");
            team1.AddPlayer("Eve");

            Team team2 = match.NewTeam();
            team1.AddPlayer("Chucky");
            team1.AddPlayer("Dianne");

            round.IdlePlayers.Add("Zed");

            var expected = new List<string>() { "Adam", "Eve", "Chucky", "Dianne", "Zed" };

            Debug.WriteLine(league.PrettyPrint());
            CollectionAssert.AreEquivalent(expected, round.AllPlayers as System.Collections.ICollection);
        }

        [TestMethod]
        public void Players() {
            League league = new League();
            LeagueEvent lEvent = league.NewLeagueEvent("my_event");
            Round round = lEvent.NewRound();
            Match match = round.GetMatch(0);

            Team team1 = match.NewTeam();
            team1.AddPlayer("Adam");
            team1.AddPlayer("Eve");

            Team team2 = match.NewTeam();
            team2.AddPlayer("Chucky");
            team2.AddPlayer("Dianne");

            var expected = new List<string>() { "Adam", "Eve", "Chucky", "Dianne"};

            Debug.WriteLine(league.PrettyPrint());
            CollectionAssert.AreEquivalent(expected, round.Players.ToList());
        }

        [TestMethod]
        public void Reset() {
            League league = new League();
            LeagueEvent lEvent = league.NewLeagueEvent("my_event");
            Round round = lEvent.NewRound();
            Match match = round.GetMatch(0);

            Team team1 = match.NewTeam();
            team1.AddPlayer("Adam");
            team1.AddPlayer("Eve");

            Team team2 = match.NewTeam();
            team2.AddPlayer("Chucky");
            team2.AddPlayer("Dianne");

            round.IdlePlayers.Add("Zed");
            round.ResetPlayers();

            var expected = new List<string>() { "Adam", "Eve", "Chucky", "Dianne", "Zed" };

            Debug.WriteLine(league.PrettyPrint());
            Debug.WriteLine(round.IdlePlayers.DelString());
            CollectionAssert.AreEquivalent(expected, round.IdlePlayers.ToList());
        }
    }
}
