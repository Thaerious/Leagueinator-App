using Model;
using Model.Tables;
using System.Diagnostics;

namespace Model_Test {
    [TestClass]
    public class IdleTable_Test {

        [TestMethod]
        public void AddRow() {
            IdleTable table = new IdleTable();
            table.AddRow(0, 0, "name");
            Assert.AreEqual(1, table.Rows.Count);
        }
    }
}
