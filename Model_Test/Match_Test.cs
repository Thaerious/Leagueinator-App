using Model;
using Model.Tables;
using System.Data;
using System.Diagnostics;

namespace Model_Test {
    [TestClass]
    public class Match_Test {

        [TestMethod]
        public void Round() {
            League league = new League();
            LeagueEvent lEvent = league.NewLeagueEvent("my_event");
            Round round = lEvent.NewRound();
            Match match = round.GetMatch(0);

            Assert.AreEqual(round, match.Round);
        }

        [TestMethod]
        public void Size_For_Empty_Match() {
            League league = new League();
            LeagueEvent lEvent = league.NewLeagueEvent("my_event");
            Round round = lEvent.NewRound();
            Match match = round.GetMatch(0);
            Assert.AreEqual(0, match.Size);
        }

        [TestMethod]
        public void Size_For_One_Player() {
            League league = new League();
            LeagueEvent lEvent = league.NewLeagueEvent("my_event");
            Round round = lEvent.NewRound();
            Match match = round.GetMatch(0);
            match.NewTeam().AddPlayer("Adam");

            Assert.AreEqual(1, match.Size);
            Assert.AreEqual(1, match.Teams.Count);
        }

        [TestMethod]
        public void Size_For_Two_Teams_Two_Players() {
            League league = new League();
            LeagueEvent lEvent = league.NewLeagueEvent("my_event");
            Round round = lEvent.NewRound();
            Match match = round.GetMatch(0);
            match.NewTeam().AddPlayer("Adam");
            match.NewTeam().AddPlayer("Eve");

            Assert.AreEqual(2, match.Size);
            Assert.AreEqual(2, match.Teams.Count);
        }

        [TestMethod]
        public void Size_For_One_Team_Two_Players() {
            League league = new League();
            LeagueEvent lEvent = league.NewLeagueEvent("my_event");
            Round round = lEvent.NewRound();
            Match match = round.GetMatch(0);
            match.NewTeam().AddPlayer("Adam");
            match.Teams[0].AddPlayer("Eve");

            Debug.WriteLine(league.PrettyPrint());

            Assert.AreEqual(1, match.Size);
            Assert.AreEqual(1, match.Teams.Count);
        }

        [TestMethod]
        public void New_Team() {
            League league = new League();
            LeagueEvent lEvent = league.NewLeagueEvent("my_event");
            Round round = lEvent.NewRound();
            Match match = round.GetMatch(0);
            Team t1 = match.NewTeam();
            Team t2 = match.NewTeam();

            Assert.AreEqual(0, t1.TeamIndex);
            Assert.AreEqual(1, t2.TeamIndex);
        }

        [TestMethod]
        public void Get_Empty_Match() {
            League league = new League();
            LeagueEvent lEvent = league.NewLeagueEvent("my_event");
            Round round = lEvent.NewRound();
            Match match = round.GetMatch(0);
            Assert.IsNotNull(match);
        }

        [TestMethod]
        public void Get_Team_Exists() {
            League league = new League();
            LeagueEvent lEvent = league.NewLeagueEvent("my_event");
            Round round = lEvent.NewRound();
            Match match = round.GetMatch(0);
            match.NewTeam();
            Team team = match.Teams[0];

            Debug.WriteLine($"filter = '{match.RowFilter}'");
            Debug.WriteLine(match.PrettyPrint());

            Assert.IsNotNull(team);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Get_Team_Does_Not_Exist() {
            League league = new League();
            LeagueEvent lEvent = league.NewLeagueEvent("my_event");
            Round round = lEvent.NewRound();
            Match match = round.GetMatch(0);
            Team team = match.Teams[0];
            Assert.IsNotNull(team);
        }

        [TestMethod]
        public void Add_One_Team() {
            League league = new League();
            LeagueEvent lEvent = league.NewLeagueEvent("my_event");
            Round round = lEvent.NewRound();
            Match match = round.GetMatch(0);
            Team team = match.NewTeam();

            Assert.AreEqual(1, match.Size);
        }

        [TestMethod]
        public void Add_Many_Teams() {
            League league = new League();
            LeagueEvent lEvent = league.NewLeagueEvent("my_event");
            Round round = lEvent.NewRound();
            Match match = round.GetMatch(0);
            match.NewTeam();
            match.NewTeam();
            match.NewTeam();

            Assert.AreEqual(3, match.Size);
            Assert.AreEqual(3, match.Teams.Count);
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

            match.Delete();

            Assert.IsTrue(match.Deleted);
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

            var expected = new List<string>() { "Adam", "Eve", "Chucky", "Dianne" };

            Debug.WriteLine(league.PrettyPrint());
            CollectionAssert.AreEquivalent(expected, match.Players as System.Collections.ICollection);
        }

        [TestMethod]
        public void Team_Index() {
            League league = new League();
            LeagueEvent myEvent = league.NewLeagueEvent("my_event");

            myEvent.NewRound();
            myEvent.NewRound();

            myEvent.Rounds[0].GetMatch(0).NewTeam();
            myEvent.Rounds[0].GetMatch(0).NewTeam();

            myEvent.Rounds[0].GetMatch(0).Teams[0].AddPlayer("Adam");
            myEvent.Rounds[0].GetMatch(0).Teams[0].AddPlayer("Betty");
            myEvent.Rounds[0].GetMatch(0).Teams[1].AddPlayer("Chuck");
            myEvent.Rounds[0].GetMatch(0).Teams[1].AddPlayer("Dianne");

            var team1 = myEvent.Rounds[1].GetMatch(0).NewTeam();
            var team2 = myEvent.Rounds[1].GetMatch(0).NewTeam();

            Debug.WriteLine(myEvent.Rounds[0].GetMatch(0).PrettyPrint());

            Assert.AreEqual(0, team1.TeamIndex);
            Assert.AreEqual(1, team2.TeamIndex);
        }
    }
}
