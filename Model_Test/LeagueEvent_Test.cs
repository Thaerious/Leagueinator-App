using Model;
using Model.Tables;

namespace Model_Test {
    [TestClass]
    public class LeagueEvent_Test {

        [TestMethod]
        public void League() {
            League league = new();
            EventRow eventRow = league.EventTable.AddRow("my_event");
            Assert.AreEqual(league, eventRow.League);
            Assert.IsNotNull(eventRow);
        }

        [TestMethod]
        public void Event_Name_Matched() {
            League league = new();
            EventRow eventRow = league.EventTable.AddRow("my_event");
            Assert.AreEqual("my_event", eventRow.Name);
        }

        [TestMethod]
        public void Round_Count_Empty() {
            League league = new();
            EventRow eventRow = league.EventTable.AddRow("my_event");
            Assert.IsNotNull(eventRow);
            Assert.AreEqual(0, eventRow.Rounds.Count);
        }

        [TestMethod]
        public void Round_Count_One() {
            League league = new();
            EventRow eventRow = league.EventTable.AddRow("my_event");
            eventRow.Rounds.Add();
            Assert.AreEqual(1, eventRow.Rounds.Count);
        }

        [TestMethod]
        public void Round_Count_Many() {
            League league = new();
            EventRow eventRow = league.EventTable.AddRow("my_event");
            eventRow.Rounds.Add();
            eventRow.Rounds.Add();
            eventRow.Rounds.Add();
            eventRow.Rounds.Add();
            eventRow.Rounds.Add();
            Assert.AreEqual(5, eventRow.Rounds.Count);
        }


        [TestMethod]
        public void Get_Round() {
            League league = new();
            EventRow eventRow = league.EventTable.AddRow("my_event");
            eventRow.Rounds.Add();
            Assert.IsNotNull(eventRow.Rounds[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void Get_Round_Does_Not_Exist() {
            League league = new();
            EventRow eventRow = league.EventTable.AddRow("my_event");
            Assert.IsNotNull(eventRow.Rounds[0]);
        }
    }
}
