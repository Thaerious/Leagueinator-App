using Model;
using Model.Tables;
using System.Data;
using System.Diagnostics;

namespace Model_Test {
    [TestClass]
    public class LeagueEventSettings_Test {

        [TestMethod]
        public void Set_Retrieve() {
            League league = new();
            LeagueEvent lEvent = league.NewLeagueEvent("my_event");
            lEvent.Settings["MyKey"] = "MyValue";

            Assert.AreEqual("MyValue", lEvent.Settings["MyKey"]);
        }

        [TestMethod]
        public void Just_Retrieve() {
            League league = new();
            LeagueEvent lEvent = league.NewLeagueEvent("my_event");
            Assert.AreEqual("", lEvent.Settings["MyKey"]);
        }

        [TestMethod]
        public void Set_Reset () {
            League league = new();
            LeagueEvent lEvent = league.NewLeagueEvent("my_event");
            lEvent.Settings["MyKey"] = "MyValue";
            lEvent.Settings["MyKey"] = "MySecondValue";
            Assert.AreEqual("MySecondValue", lEvent.Settings["MyKey"]);
        }
    }
}
