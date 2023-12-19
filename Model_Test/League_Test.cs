using Model;
using System.Data;
using System.Diagnostics;

namespace Model_Test {
    [TestClass]
    public class League_Test {

        [TestMethod]
        public void Sanity_Check() {
            League league = new();
            LeagueEvent lEvent = league.NewLeagueEvent("my_event");
            
            Debug.WriteLine(league.PrettyPrint());
            
            Assert.IsNotNull(lEvent);
        }

        /// <summary>
        /// When you call GetEventsTable it will create a new table if it
        /// doesn't already exist.
        /// </summary>
        [TestMethod]
        public void Add_League_Event() {
            League league = new();
            LeagueEvent lEvent = league.NewLeagueEvent("my_event");
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
            league.NewLeagueEvent("my_event");
            league.NewLeagueEvent("my_event");
        }

        /// <summary>
        /// When you call GetEventsTable it will create a new table if it
        /// doesn't already exist.
        /// </summary>
        [TestMethod]
        public void Get_League_Event() {
            League league = new();
            LeagueEvent lEvent = league.NewLeagueEvent("my_event");
            Assert.IsTrue(lEvent != null);
            Assert.IsTrue(lEvent.Table != null);
        }

        [TestMethod]
        public void Get_League_Events_List() {
            League league = new();
            league.NewLeagueEvent("my_event");
            league.NewLeagueEvent("my_other_event");

            var list = league.LeagueEvents;
            Assert.IsTrue(list != null);
            Assert.AreEqual(2, list.Count);
        }

        /// <summary>
        /// You can not retrieve events that haven't been added.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void Retrive_Unknown_Event_Gives_Exception() {
            League league = new();
            var lEvent = league.GetLeagueEvent("my_event");
        }

        /// <summary>
        /// New league does not contain any data.
        /// </summary>
        [TestMethod]
        public void New_League_Event_Is_Empty() {
            League league = new();
            LeagueEvent lEvent = league.NewLeagueEvent("my_event");
            Assert.AreEqual(0, lEvent.Count);
        }
    }
}
