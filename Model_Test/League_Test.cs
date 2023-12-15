using Model;
using Model.Tables;
using System.Data;
using System.Diagnostics;

namespace Model_Test {
    [TestClass]
    public class League_Test {
        public static League Mock() {
            League league = new();
            LeagueEvent lEvent1 = league.AddLeagueEvent("my_event");

            lEvent1.AddRow(round: 1, lane: 1, teamID: 0, bowls: 0, ends: 0, tiebreaker: 0);
            lEvent1.AddRow(round: 1, lane: 1, teamID: 1, bowls: 0, ends: 0, tiebreaker: 0);
            lEvent1.AddRow(round: 1, lane: 2, teamID: 2, bowls: 0, ends: 0, tiebreaker: 0);
            lEvent1.AddRow(round: 1, lane: 2, teamID: 3, bowls: 0, ends: 0, tiebreaker: 0);

            lEvent1.AddRow(round: 2, lane: 1, teamID: 0, bowls: 0, ends: 0, tiebreaker: 0);
            lEvent1.AddRow(round: 2, lane: 2, teamID: 1, bowls: 0, ends: 0, tiebreaker: 0);
            lEvent1.AddRow(round: 2, lane: 1, teamID: 2, bowls: 0, ends: 0, tiebreaker: 0);
            lEvent1.AddRow(round: 2, lane: 2, teamID: 3, bowls: 0, ends: 0, tiebreaker: 0);

            lEvent1.GetRound(1).GetMatch(1).GetTeam(0).AddPlayer("Adam");

            LeagueEvent lEvent2 = league.AddLeagueEvent("my_other_event");

            return league;
        }

        [TestMethod]
        public void Constructor_Sanity() {
            League league = new();
            Assert.IsTrue(league != null);
        }

        /// <summary>
        /// When you call GetEventsTable it will create a new table if it
        /// doesn't already exist.
        /// </summary>
        [TestMethod]
        public void Add_League_Event() {
            League league = new();
            LeagueEvent lEvent = league.AddLeagueEvent("my_event");
            Assert.IsTrue(lEvent != null);
            Assert.IsTrue(lEvent.Table != null);
        }

        /// <summary>
        /// You can not add two events with the same name.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ConstraintException))]
        public void Add_League_Event_Repeat_Gives_Exception() {
            League league = new();
            league.AddLeagueEvent("my_event");
            league.AddLeagueEvent("my_event");
        }

        /// <summary>
        /// You can not retrieve events that haven't been added.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void Retrive_Unknown_Event_Gives_Exception() {
            League league = new();
            var lEvent = league.LeagueEvents["my_event"];
        }

        /// <summary>
        /// When you call GetEventsTable it will create a new table if it
        /// doesn't already exist.
        /// </summary>
        [TestMethod]
        public void Get_League_Event() {
            League league = new();
            LeagueEvent lEvent = league.AddLeagueEvent("my_event");
            Assert.IsTrue(lEvent != null);
            Assert.IsTrue(lEvent.Table != null);
        }

        [TestMethod]
        public void Get_League_Events_List() {
            League league = Mock();
            var list = league.LeagueEvents;
            Assert.IsTrue(list != null);
            Assert.AreEqual(2, list.Count);
            Assert.IsTrue(list.ContainsKey("my_event"));
            Assert.IsTrue(list.ContainsKey("my_other_event"));
            Assert.IsNotNull(list["my_event"]);
            Assert.IsNotNull(list["my_other_event"]);
        }

        /// <summary>
        /// New league does not contain any data.
        /// </summary>
        [TestMethod]
        public void New_League_Event_Is_Empty() {
            League league = new();
            LeagueEvent lEvent = league.AddLeagueEvent("my_event");
            Assert.AreEqual(0, lEvent.Count);
        }

        /// <summary>
        /// Adding a row to the view changes the count.
        /// </summary>
        [TestMethod]
        public void Add_Row_To_League_Event() {
            League league = new();
            LeagueEvent lEvent = league.AddLeagueEvent("my_event");
            lEvent.AddRow(
                round: 1,
                lane: 1,
                teamID: 0,
                bowls: 0,
                ends: 0,
                tiebreaker: 0
            );
            Assert.AreEqual(1, lEvent.Count);
        }

        [TestMethod]
        public void Count_Rounds_From_League_Event() {
            League league = Mock();
            LeagueEvent lEvent = league.LeagueEvents["my_event"];
            Debug.WriteLine(lEvent.PrettyPrint());
            Assert.IsTrue(lEvent.Count == 8);
        }

        [TestMethod]
        public void Retrieve_Round_From_League_Event() {
            League league = Mock();
            LeagueEvent lEvent = league.LeagueEvents["my_event"];
            Round round = lEvent.GetRound(1);

            Debug.WriteLine(round.PrettyPrint());
            Assert.IsTrue(round.Count == 4);
        }

        [TestMethod]
        public void Retrieve_Match_From_Round() {
            League league = Mock();
            LeagueEvent lEvent = league.LeagueEvents["my_event"];
            Round round = lEvent.GetRound(1);
            Match match = round.GetMatch(1);

            Debug.WriteLine(round.PrettyPrint());
            Debug.WriteLine(match.PrettyPrint());            

            Assert.IsTrue(match.Count == 2);
        }
    }
}
