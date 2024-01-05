using Model;
using Model.Tables;
using System.Data;
using System.Diagnostics;

namespace Model_Test {
    [TestClass]
    public class Extension_Test {

        [TestMethod]
        public void Join() {
            Mock1 league = new Mock1();
            Debug.WriteLine(league.PrettyPrint());
            var joined = league.TeamTable.LeftJoin<int>(league.EventTable, "event_uid", "uid");
        }

        [TestMethod]
        public void New_Table_From_Tables() {
            Mock1 league = new Mock1();
            DataTable table = new DataTable().MergeWith(league.TeamTable, league.IdleTable);

            Debug.WriteLine(table.PrettyPrint());

            Assert.IsTrue(table.ColumnNames().Contains("team.uid"));
            Assert.IsTrue(table.ColumnNames().Contains("team.event_uid"));
            Assert.IsTrue(table.ColumnNames().Contains("team.player_name"));
            Assert.IsTrue(table.ColumnNames().Contains("idle.uid"));
            Assert.IsTrue(table.ColumnNames().Contains("idle.event_dir_uid"));
            Assert.IsTrue(table.ColumnNames().Contains("idle.round"));
            Assert.IsTrue(table.ColumnNames().Contains("idle.player_name"));
            Assert.AreEqual(table.ColumnNames().Count, 7);
        }

        [TestMethod]
        public void As() {
            Mock1 league = new Mock1();
            DataTable merged = new DataTable().MergeWith(league.TeamTable, league.IdleTable);
            DataTable table = merged.As("uid", "event", "name");
            
            Assert.IsTrue(table.ColumnNames().Contains("uid"));
            Assert.IsTrue(table.ColumnNames().Contains("event"));
            Assert.IsTrue(table.ColumnNames().Contains("name"));
            Assert.IsTrue(table.ColumnNames().Contains("idle.uid"));
            Assert.IsTrue(table.ColumnNames().Contains("idle.event_dir_uid"));
            Assert.IsTrue(table.ColumnNames().Contains("idle.round"));
            Assert.IsTrue(table.ColumnNames().Contains("idle.player_name"));
            Assert.AreEqual(table.ColumnNames().Count, 7);
        }
    }
}
