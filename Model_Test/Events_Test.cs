using Model;
using Model.Tables;

namespace Model_Test {
    [TestClass]
    public class Events_Test {

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

        [TestMethod]
        public void Set_Setting() {
            League league = new();
            EventRow eventRow = league.EventTable.AddRow("my_event");
            eventRow.Settings["MyKey"] = "MyValue";
            Assert.AreEqual("MyValue", eventRow.Settings["MyKey"]);
        }

        [TestMethod]
        public void Set_Setting_Multiple_Tables() {
            League league = new();
            EventRow eventRow1 = league.EventTable.AddRow("my_event1");
            EventRow eventRow2 = league.EventTable.AddRow("my_event2");
            eventRow1.Settings["MyKey"] = "MyValue1";
            eventRow2.Settings["MyKey"] = "MyValue2";
            Assert.AreEqual("MyValue1", eventRow1.Settings["MyKey"]);
            Assert.AreEqual("MyValue2", eventRow2.Settings["MyKey"]);
        }

        [TestMethod]
        public void ReSet_Setting() {
            League league = new();
            EventRow eventRow = league.EventTable.AddRow("my_event");
            eventRow.Settings["MyKey"] = "MyValue";
            eventRow.Settings["MyKey"] = "AnotherValue";
            Assert.AreEqual("AnotherValue", eventRow.Settings["MyKey"]);
        }

        [TestMethod]
        public void Delete_Setting() {
            League league = new();
            EventRow eventRow = league.EventTable.AddRow("my_event");
            eventRow.Settings.Delete("MyKey");
            Assert.IsNull(eventRow.Settings["MyKey"]);
        }

        [TestMethod]
        public void NonExistant_Setting() {
            League league = new();
            EventRow eventRow = league.EventTable.AddRow("my_event");
            Assert.IsNull(eventRow.Settings["MyKey"]);
        }

        [TestMethod]
        public void Set_To_Null_Deletes_Setting() {
            League league = new();
            EventRow eventRow = league.EventTable.AddRow("my_event");
            eventRow.Settings["MyKey"] = "MyValue";
            eventRow.Settings["MyKey"] = null;
            Assert.IsFalse(eventRow.Settings.HasKey("MyKey"));
        }
    }
}
