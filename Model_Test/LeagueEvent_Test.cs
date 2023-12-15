using Model;
using Model.Tables;
using System.Data;
using System.Diagnostics;

namespace Model_Test {
    [TestClass]
    public class LeagueEvent_Test {

        private class Mock : League {
            public Mock() : base() {
                LeagueEvent lEvent1 = this.AddLeagueEvent("my_event");
            }
        }

        [TestMethod]
        public void Sanity_Check() {
            League league = new Mock();
            LeagueEvent lEvent = league.LeagueEvents["my_event"];
            Assert.IsTrue(lEvent != null);
        }

        [TestMethod]
        public void Event_Name_Matched() {
            League league = new Mock();
            LeagueEvent lEvent = league.LeagueEvents["my_event"];
            Assert.AreEqual("my_event", lEvent.EventName);
        }
    }
}
