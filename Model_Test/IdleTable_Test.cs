using Model;
using Model.Tables;
using System.Diagnostics;

namespace Model_Test {
    [TestClass]
    public class IdleTable_Test {

        [TestMethod]
        public void AddRow() {
            IdleTable table = new IdleTable();
            table.AddRow(0, 1, "name");
            Assert.AreEqual(1, table.Rows.Count);
        }

        [TestMethod]
        public void Add_Row_From_League() {
            League league = new League();
            var lEvent = league.NewLeagueEvent("myEvent");
            var round = lEvent.NewRound();

            IdleTable table = new IdleTable();
            table.AddRow(lEvent.UID, round.RoundIndex, "name");
            Assert.AreEqual(1, table.Rows.Count);
        }
    }
}
