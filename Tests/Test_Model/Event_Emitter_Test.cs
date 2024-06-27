using Leagueinator.Model.Tables;
using Leagueinator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leagueinator.Utility;

namespace Model_Test {
    [TestClass]
    public class Event_Emitter_Test {
        [TestMethod]
        public void Add_Event_Triggers_RowAdded() {
            bool isAdded = false;
            League league = new();

            league.EventTable.RowAdded += (s, args) => {
                Console.WriteLine(args.Row.PrettyPrint("Row Added"));
                isAdded = true;
            };

            EventRow eventRow = league.Events.Add("my_event");
            Assert.IsTrue(isAdded);
        }

        [TestMethod]
        public void Remove_Event_Triggers_RowDeleted() {
            bool isDeleted = false;
            League league = new();

            league.EventTable.RowDeleting += (s, args) => {
                Console.WriteLine(args.Row.PrettyPrint("Row Deleting"));
                isDeleted = true;
            };

            EventRow eventRow = league.Events.Add("my_event");
            eventRow.Remove();
            Assert.IsTrue(isDeleted);
        }

        [TestMethod]
        public void Update_Event_Triggers_RowUpdated() {
            bool isChanged = false;
            League league = new();

            league.EventTable.RowUpdated += (s, args) => {
                Console.WriteLine(args.Row.PrettyPrint("Row Updating"));
                Console.WriteLine(args.Changes.DelString(c=>$"{c.Column}, {c.OldValue}, {c.NewValue}", "\n"));
                isChanged = true;
            };

            EventRow eventRow = league.Events.Add("my_event");
            eventRow.Name = "new event name";
            Assert.IsTrue(isChanged);
        }
    }
}
