using Leagueinator.Model;
using Leagueinator.Model.Tables;
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
        public void VerifyColumnProperties() {
            League league = new();
            DataTable eventTable = league.EventTable;

            Assert.AreEqual(typeof(int), eventTable.Columns[EventTable.COL.UID]!.DataType);
            Assert.AreEqual(true, eventTable.Columns[EventTable.COL.UID]!.Unique);
            Assert.AreEqual(typeof(string), eventTable.Columns[EventTable.COL.NAME]!.DataType);
            Assert.AreEqual(typeof(string), eventTable.Columns[EventTable.COL.DATE]!.DataType);
            Assert.AreEqual(typeof(int), eventTable.Columns[EventTable.COL.LANE_COUNT]!.DataType);
            Assert.AreEqual(8, eventTable.Columns[EventTable.COL.LANE_COUNT]!.DefaultValue);
            Assert.AreEqual(typeof(int), eventTable.Columns[EventTable.COL.ENDS_DEFAULT]!.DataType);
            Assert.AreEqual(10, eventTable.Columns[EventTable.COL.ENDS_DEFAULT]!.DefaultValue);
            Assert.AreEqual(typeof(EventFormat), eventTable.Columns[EventTable.COL.EVENT_FORMAT]!.DataType);
            Assert.AreEqual(EventFormat.AssignedLadder, eventTable.Columns[EventTable.COL.EVENT_FORMAT]!.DefaultValue);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddRow_InvalidName_ThrowsException() {
            League league = new();
            league.EventTable.AddRow(null);
        }

        [DataTestMethod]
        [DataRow("Event1", "2024-06-01")]
        [DataRow("Event2", null)] // Test with default date
        public void AddRow_Parameterized(string eventName, string date) {
            League league = new();
            EventRow eventRow = league.EventTable.AddRow(eventName, date);

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

        [TestMethod]
        public void GetRowByName_Exists() {
            League league = new();
            league.EventTable.AddRow("existing_event");
            EventRow eventRow = league.EventTable.GetRow("existing_event");

            Assert.AreEqual("existing_event", eventRow.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void GetRowByName_NotExists() {
            League league = new();
            league.EventTable.GetRow("nonexistent_event");
        }

        [TestMethod]
        public void HasRow() {
            League league = new();
            league.EventTable.AddRow("test_event");

            Assert.IsTrue(league.EventTable.HasRow("test_event"));
            Assert.IsFalse(league.EventTable.HasRow("nonexistent_event"));
        }

        [TestMethod]
        public void GetLast() {
            League league = new();
            league.EventTable.AddRow("first_event");
            EventRow lastRow = league.EventTable.AddRow("last_event");

            Assert.AreEqual(lastRow.Name, league.EventTable.GetLast().Name);
        }

        [TestMethod]
        public void CountRows_Test() {
            League league = new();

            // Initially, the table should be empty
            Assert.AreEqual(0, league.EventTable.Rows.Count);

            // Add a few rows
            league.EventTable.AddRow("event_1");
            league.EventTable.AddRow("event_2");
            league.EventTable.AddRow("event_3");

            // Verify the count of rows in the table
            Assert.AreEqual(3, league.EventTable.Rows.Count);
        }

        [TestMethod]
        public void SetEndsDefault_Test() {
            League league = new();
            EventRow eventRow = league.EventTable.AddRow("event_with_ends_default");

            // Verify the default value
            Assert.AreEqual(10, eventRow[EventTable.COL.ENDS_DEFAULT]);

            // Change the value of EndsDefault
            eventRow[EventTable.COL.ENDS_DEFAULT] = 15;

            // Verify the new value
            Assert.AreEqual(15, eventRow[EventTable.COL.ENDS_DEFAULT]);
        }

        [TestMethod]
        public void SetLaneCount_Test() {
            League league = new();
            EventRow eventRow = league.EventTable.AddRow("event_with_lanes_default");

            // Verify the default value
            Assert.AreEqual(8, eventRow[EventTable.COL.LANE_COUNT]);

            // Change the value of LaneCount
            eventRow[EventTable.COL.LANE_COUNT] = 12;

            // Verify the new value
            Assert.AreEqual(12, eventRow[EventTable.COL.LANE_COUNT]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SetLaneCount_LessThanZero_ThrowsException() {
            League league = new();
            EventRow eventRow = league.EventTable.AddRow("event_with_invalid_lanes");

            // Attempt to set LaneCount to a value less than 0
            eventRow.LaneCount = -1;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SetEndsDefault_LessThanZero_ThrowsException() {
            League league = new();
            EventRow eventRow = league.EventTable.AddRow("event_with_invalid_ends");

            // Attempt to set LaneCount to a value less than 0
            eventRow.EndsDefault = -1;
        }
    }
}
