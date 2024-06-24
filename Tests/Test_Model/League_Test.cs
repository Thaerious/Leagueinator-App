using Leagueinator.Model;
using Model.Source.Tables.Event;
using System.Data;

namespace Model_Test {
    [TestClass]
    public class League_Test {

        [TestMethod]
        public void Sanity() {
            League league = new();
            EventRow eventRow = league.EventTable.AddRow("my_event");
            Assert.IsNotNull(eventRow);
        }

        /// <summary>
        /// You can not add two events with the same name.
        /// </summary>
        [TestMethod]
        public void Add_League_Event_Repeat_Gives_Exception() {
            League league = new();

            Assert.ThrowsException<ConstraintException>(() => {
                league.EventTable.AddRow("my_event");
                league.EventTable.AddRow("my_event");
            });
        }

        [TestMethod]
        public void Get_League_Events_List() {
            League league = new();
            league.EventTable.AddRow("my_first_event");
            league.EventTable.AddRow("my_second_event");
            Assert.AreEqual(2, league.EventTable.Rows.Count);
        }

        [TestMethod]
        public void Retrieve_Event() {
            League league = new();
            EventRow eventRowIn = league.EventTable.AddRow("my_event");
            EventRow eventRowOut = league.EventTable.GetRow("my_event");
            Assert.IsNotNull(eventRowOut);
            Assert.AreEqual(eventRowIn.Name, eventRowOut.Name);
        }
    }
}
