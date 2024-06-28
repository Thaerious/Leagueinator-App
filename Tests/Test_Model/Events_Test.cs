using Leagueinator.Model;
using Leagueinator.Model.Tables;
using System.Data;
using System.Diagnostics;

namespace Model_Test {
    [TestClass]
    public class Events_Test {

        [TestMethod]
        public void Add_Row_To_Event_Table() {
            League league = new();
            EventRow eventRow = league.Events.Add("my_event");
            Assert.AreEqual(league, eventRow.League);
            Assert.IsNotNull(eventRow);
        }

        /// <summary>
        /// You can not add two events with the same name.
        /// </summary>
        [TestMethod]
        public void Add_League_Event_Repeat_Gives_Exception() {
            League league = new();

            Assert.ThrowsException<ConstraintException>(() => {
                league.Events.Add("my_event");
                league.Events.Add("my_event");
            });
        }

        [DataTestMethod]
        [DataRow("Event1", "2024-06-01")]
        [DataRow("Event2", null)] // Test with default date
        public void AddRow_Parameterized(string eventName, string date) {
            League league = new();
            EventRow eventRow = league.Events.Add(eventName, date);

            Assert.AreEqual(eventName, eventRow.Name);
            Assert.AreEqual(date ?? DateTime.Today.ToString("yyyy-MM-dd"), eventRow.Date);
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
            EventRow eventRow = league.Events.Add("my_event");
            Assert.IsNotNull(eventRow);
        }

        [TestMethod]
        public void Event_Name_Matched() {
            League league = new();
            EventRow eventRow = league.Events.Add("my_event");
            Assert.AreEqual("my_event", eventRow.Name);
        }

        [TestMethod]
        public void Round_Count_Empty() {
            League league = new();
            EventRow eventRow = league.Events.Add("my_event");
            Assert.IsNotNull(eventRow);
            Assert.AreEqual(0, eventRow.Rounds.Count);
        }

        [TestMethod]
        public void Round_Count_One() {
            League league = new();
            EventRow eventRow = league.Events.Add("my_event");
            eventRow.Rounds.Add();
            Assert.AreEqual(1, eventRow.Rounds.Count);
        }

        [TestMethod]
        public void Round_Count_Many() {
            League league = new();
            EventRow eventRow = league.Events.Add("my_event");
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
            EventRow eventRow = league.Events.Add("my_event");
            eventRow.Rounds.Add();
            Assert.IsNotNull(eventRow.Rounds[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void Get_Round_Does_Not_Exist() {
            League league = new();
            EventRow eventRow = league.Events.Add("my_event");
            Debug.WriteLine(league.EventTable.PrettyPrint());
            Debug.WriteLine(league.RoundTable.PrettyPrint());

            var view = ((DataView)eventRow.Rounds);

            Debug.WriteLine(view.Count);
            Debug.WriteLine(view[0]);
            Assert.IsNotNull(eventRow.Rounds[0]);
        }

        [TestMethod]
        public void GetRowByName_Exists() {
            League league = new();
            league.Events.Add("existing_event");
            EventRow eventRow = league.Events.Get("existing_event");

            Assert.AreEqual("existing_event", eventRow.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void GetRowByName_NotExists() {
            League league = new();
            league.Events.Get("nonexistent_event");
        }

        [TestMethod]
        public void HasRow() {
            League league = new();
            league.Events.Add("test_event");

            Assert.IsTrue(league.Events.Has("test_event"));
            Assert.IsFalse(league.Events.Has("nonexistent_event"));
        }

        [TestMethod]
        public void GetLast() {
            League league = new();
            league.Events.Add("first_event");
            EventRow lastRow = league.Events.Add("last_event");
            Assert.AreEqual(lastRow.Name, league.Events[^1].Name);
        }

        [TestMethod]
        public void CountRows_Test() {
            League league = new();

            // Initially, the table should be empty
            Assert.AreEqual(0, league.EventTable.Rows.Count);

            // Set a few rows
            league.Events.Add("event_1");
            league.Events.Add("event_2");
            league.Events.Add("event_3");

            // Verify the count of rows in the table
            Assert.AreEqual(3, league.EventTable.Rows.Count);
        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(8)]
        [DataRow(16)]
        public void SetEndsDefault_Test(int endsDefault) {
            League league = new();
            EventRow eventRow = league.Events.Add("event_with_ends_default");
            eventRow.EndsDefault = endsDefault;
            Assert.AreEqual(endsDefault, eventRow.EndsDefault);
        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(8)]
        [DataRow(16)]
        public void Set_LaneCount_Test(int laneCount) {
            League league = new();
            EventRow eventRow = league.Events.Add("event_with_lanes_default");
            eventRow.LaneCount = laneCount;
            Assert.AreEqual(laneCount, eventRow.LaneCount);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Set_LaneCount_LessThanOne_ThrowsException() {
            League league = new();
            EventRow eventRow = league.Events.Add("event_with_invalid_lanes");

            // Attempt to set LaneCount to a value less than 1
            eventRow.LaneCount = 0;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Set_EndsDefault_LessThanOne_ThrowsException() {
            League league = new();
            EventRow eventRow = league.Events.Add("event_with_invalid_ends");

            // Attempt to set LaneCount to a value less than 1
            eventRow.EndsDefault = 0;
        }
    }
}
