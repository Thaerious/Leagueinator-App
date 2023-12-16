using Model;
using System.Data;
using System.Diagnostics;

namespace Model_Test {
    [TestClass]
    public class Match_Test {

        [TestMethod]
        public void Round() {
            League league = new League();
            LeagueEvent lEvent = league.AddLeagueEvent("my_event");
            Round round = lEvent.NewRound();
            Match match = round.GetMatch(0);

            Assert.AreEqual(round, match.Round);
        }

        [TestMethod]
        public void Size_For_Empty_Match() {
            League league = new League();
            LeagueEvent lEvent = league.AddLeagueEvent("my_event");
            Round round = lEvent.NewRound();
            Match match = round.GetMatch(0);
            Assert.AreEqual(0, match.Size);
        }

        [TestMethod]
        public void Get_Empty_Match() {
            League league = new League();
            LeagueEvent lEvent = league.AddLeagueEvent("my_event");
            Round round = lEvent.NewRound();
            Match match = round.GetMatch(0);
            Assert.IsNotNull(match);
        }

        [TestMethod]
        public void Get_Team_Exists() {
            League league = new League();
            LeagueEvent lEvent = league.AddLeagueEvent("my_event");
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
            LeagueEvent lEvent = league.AddLeagueEvent("my_event");
            Round round = lEvent.NewRound();
            Match match = round.GetMatch(0);
            Team team = match.Teams[0];
            Assert.IsNotNull(team);
        }

        [TestMethod]
        public void Add_One_Team() {
            League league = new League();
            LeagueEvent lEvent = league.AddLeagueEvent("my_event");
            Round round = lEvent.NewRound();
            Match match = round.GetMatch(0);
            Team team = match.NewTeam();

            Assert.AreEqual(1, match.Size);
        }

        [TestMethod]
        public void Add_Many_Teams() {
            League league = new League();
            LeagueEvent lEvent = league.AddLeagueEvent("my_event");
            Round round = lEvent.NewRound();
            Match match = round.GetMatch(0);
            match.NewTeam();
            match.NewTeam();
            match.NewTeam();

            Assert.AreEqual(3, match.Size);
        }
    }
}
