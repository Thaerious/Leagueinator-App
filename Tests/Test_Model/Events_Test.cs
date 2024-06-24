using Leagueinator.Model;
using Leagueinator.Model.Tables;
using Leagueinator.Utility;
using System.Data;
using System.Diagnostics;

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
        public void League_EventTable_Sanity() {
            League league = new();
            Assert.IsNotNull(league.EventTable);
        }

        /// <summary>
        /// When adding a row, the result is not null.
        /// </summary>
        [TestMethod]
        public void Add_Row_Sanity() {
            League league = new();
            EventRow eventRow = league.EventTable.AddRow("my_event");
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
            Debug.WriteLine(league.EventTable.PrettyPrint());
            Debug.WriteLine(league.RoundTable.PrettyPrint());

            var view = ((DataView)eventRow.Rounds);

            Debug.WriteLine(view.Count);
            Debug.WriteLine(view[0]);
            Assert.IsNotNull(eventRow.Rounds[0]);
        }
    }
}
