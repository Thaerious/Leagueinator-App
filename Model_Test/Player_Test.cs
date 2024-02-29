using Model;
using Model.Tables;

namespace Model_Test {
    [TestClass]
    public class PlayerTest {

        [TestMethod]
        public void PlayerRow() {
            League league = new();
            PlayerRow playerRow = league.PlayerTable.AddRow("Zen");

            Assert.IsNotNull(playerRow);
        }

        [TestMethod]
        public void Cast_PlayerRow_String() {
            League league = new();
            PlayerRow playerRow = league.PlayerTable.AddRow("Zen");
            string asString = (string)playerRow;

            Assert.IsTrue(asString is string);
        }
    }
}
