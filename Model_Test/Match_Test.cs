using Model;
using Model.Tables;
using System.Data;
using System.Diagnostics;

namespace Model_Test {
    [TestClass]
    public class Match_Test {

        [TestMethod]
        public void Match() {
            League league = new();
            EventRow eventRow = league.EventTable.AddRow("my_event");
            RoundRow roundRow = eventRow.Rounds.Add();
            MatchRow matchRow = roundRow.Matches.Add(0, 10);

            Assert.IsNotNull(matchRow);
        }

        [TestMethod]
        public void Size_For_Empty_Match() {
            League league = new();
            EventRow eventRow = league.EventTable.AddRow("my_event");
            RoundRow roundRow = eventRow.Rounds.Add();
            MatchRow matchRow = roundRow.Matches.Add(0, 10);

            Assert.AreEqual(0, matchRow.Teams.Count);
        }

        [TestMethod]
        public void Size_For_One_Match() {
            League league = new();
            EventRow eventRow = league.EventTable.AddRow("my_event");
            RoundRow roundRow = eventRow.Rounds.Add();
            MatchRow matchRow = roundRow.Matches.Add(0, 10);
            
            Assert.AreEqual(1, roundRow.Matches.Count);
        }

        [TestMethod]
        public void Size_For_Two_Matches() {
            League league = new();
            EventRow eventRow = league.EventTable.AddRow("my_event");
            RoundRow roundRow = eventRow.Rounds.Add();
            roundRow.Matches.Add(0, 10);
            roundRow.Matches.Add(1, 10);

            Assert.AreEqual(2, roundRow.Matches.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ConstraintException))]
        public void Lane_Must_Be_Unique() {
            League league = new();
            EventRow eventRow = league.EventTable.AddRow("my_event");
            RoundRow roundRow = eventRow.Rounds.Add();
            roundRow.Matches.Add(0, 10);
            roundRow.Matches.Add(0, 10);

            Assert.AreEqual(2, roundRow.Matches.Count);
        }

        [TestMethod]
        public void Delete() {
            League league = new();
            EventRow eventRow = league.EventTable.AddRow("my_event");
            RoundRow roundRow = eventRow.Rounds.Add();
            MatchRow matchRow = roundRow.Matches.Add(0, 10);
            matchRow.DataRow.Delete();

            Assert.AreEqual(0, roundRow.Matches.Count);
        }

        [TestMethod]
        public void Players() {
            League league = new();
            EventRow eventRow = league.EventTable.AddRow("my_event");
            RoundRow roundRow = eventRow.Rounds.Add();
            MatchRow matchRow = roundRow.Matches.Add(0, 10);
            matchRow.Teams.Add(0);
            matchRow.Teams.Add(1);

            league.PlayerTable.AddRow("Adam");
            league.PlayerTable.AddRow("Eve");
            league.PlayerTable.AddRow("Cain");
            league.PlayerTable.AddRow("Able");

            matchRow.Teams[0].Members.Add("Adam");
            matchRow.Teams[0].Members.Add("Eve");
            matchRow.Teams[1].Members.Add("Cain");
            matchRow.Teams[1].Members.Add("Able");

            var a = matchRow.Members;
            Console.WriteLine(a.PrettyPrint());
        }

        [TestMethod]
        public void Empty_Players() {
            League league = new();
            EventRow eventRow = league.EventTable.AddRow("my_event");
            RoundRow roundRow = eventRow.Rounds.Add();
            MatchRow matchRow = roundRow.Matches.Add(0, 10);
            matchRow.Teams.Add(0);
            matchRow.Teams.Add(1);

            Console.WriteLine("Before");
            var a = matchRow.Members;
            Console.WriteLine(a.PrettyPrint());
            Console.WriteLine("After");
            Assert.IsTrue(true);
        }
    }
}
