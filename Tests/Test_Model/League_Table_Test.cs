using Leagueinator.Model;
using Leagueinator.Model.Tables;
using Leagueinator.Model.Views;

namespace Model_Test {
    [TestClass]
    public class League_Table_Test {

        [TestMethod]
        public void Add_String() {
            League league = new();
            league.LeagueTable.Set("Ima Key", "Ima Value");

            Console.WriteLine(league.LeagueTable.PrettyPrint());

            Assert.IsTrue(league.LeagueTable.Has("Ima Key"));
            Assert.AreEqual("Ima Value", league.LeagueTable.Get<string>("Ima Key"));
        }

        [TestMethod]
        public void Change_Value() {
            League league = new();
            league.LeagueTable.Set("Ima Key", "Ima Value");
            league.LeagueTable.Set("Ima Key", "Ima Different Value");

            Assert.AreEqual("Ima Different Value", league.LeagueTable.Get<string>("Ima Key"));
        }

        [TestMethod]
        public void Remove_Value() {
            League league = new();
            league.LeagueTable.Set("Ima Key", "Ima Value");
            league.LeagueTable.UnSet("Ima Key");

            Assert.IsFalse(league.LeagueTable.Has("Ima Key"));
        }
    }
}
