using Leagueinator.Model;
using Leagueinator.Model.Tables;
using Leagueinator.Model.Views;

namespace Model_Test {
    [TestClass]
    public class League_Table_Test {

        [TestMethod]
        public void Add_String() {
            League league = new();
            league.LeagueSettingsTable.Set("Ima Key", "Ima Value");

            Console.WriteLine(league.LeagueSettingsTable.PrettyPrint());

            Assert.IsTrue(league.LeagueSettingsTable.Has("Ima Key"));
            Assert.AreEqual("Ima Value", league.LeagueSettingsTable.Get<string>("Ima Key"));
        }

        [TestMethod]
        public void Change_Value() {
            League league = new();
            league.LeagueSettingsTable.Set("Ima Key", "Ima Value");
            league.LeagueSettingsTable.Set("Ima Key", "Ima Different Value");

            Assert.AreEqual("Ima Different Value", league.LeagueSettingsTable.Get<string>("Ima Key"));
        }

        [TestMethod]
        public void Remove_Value() {
            League league = new();
            league.LeagueSettingsTable.Set("Ima Key", "Ima Value");
            league.LeagueSettingsTable.UnSet("Ima Key");

            Assert.IsFalse(league.LeagueSettingsTable.Has("Ima Key"));
        }
    }
}
