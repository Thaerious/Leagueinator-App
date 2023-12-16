using Model;
using System.Data;

namespace Model_Test {
    [TestClass]
    public class League_Test {

        [TestMethod]
        public void Sanity_Check() {
            League league = new();
            LeagueEvent lEvent = league.AddLeagueEvent("my_event");
            Assert.IsNotNull(lEvent);
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
            League league = new();
            league.AddLeagueEvent("my_event");
            league.AddLeagueEvent("my_other_event");

            var list = league.LeagueEvents;
            Assert.IsTrue(list != null);
            Assert.AreEqual(2, list.Count);
            Assert.IsTrue(list.ContainsKey("my_event"));
            Assert.IsTrue(list.ContainsKey("my_other_event"));
            Assert.IsNotNull(list["my_event"]);
            Assert.IsNotNull(list["my_other_event"]);
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
        /// New league does not contain any data.
        /// </summary>
        [TestMethod]
        public void New_League_Event_Is_Empty() {
            League league = new();
            LeagueEvent lEvent = league.AddLeagueEvent("my_event");
            Assert.AreEqual(0, lEvent.Count);
        }
    }
}
