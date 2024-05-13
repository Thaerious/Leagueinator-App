using Leagueinator.Model;
using Leagueinator.Model.Tables;
using Leagueinator.Model.Views;
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
            Assert.IsNotNull(eventRow.Rounds[0]);
        }

        [TestMethod]
        public void Set_Setting() {
            League league = new();
            EventRow eventRow = league.EventTable.AddRow("my_event");
            eventRow.Settings.Add("MyKey", "MyValue");
            Assert.AreEqual("MyValue", eventRow.Settings.Get("MyKey")!.Value);
        }

        [TestMethod]
        public void Set_Setting_Multiple_Tables() {
            League league = new();
            EventRow eventRow1 = league.EventTable.AddRow("my_event1");
            EventRow eventRow2 = league.EventTable.AddRow("my_event2");
            eventRow1.Settings.Add("MyKey", "MyValue1");
            eventRow2.Settings.Add("MyKey", "MyValue2");
            Assert.AreEqual("MyValue1", eventRow1.Settings.Get("MyKey")!.Value);
            Assert.AreEqual("MyValue2", eventRow2.Settings.Get("MyKey")!.Value);
        }

        [TestMethod]
        public void ReSet_Setting() {
            League league = new();
            EventRow eventRow = league.EventTable.AddRow("my_event");
            eventRow.Settings.Add("MyKey", "MyValue");
            eventRow.Settings.Get("MyKey")!.Value = "AnotherValue";
            Assert.AreEqual("AnotherValue", eventRow.Settings.Get("MyKey")!.Value);
        }

        [TestMethod]
        public void Delete_Setting() {
            League league = new();
            EventRow eventRow = league.EventTable.AddRow("my_event");
            eventRow.Settings.Add("MyKey", "MyValue");
            eventRow.Settings.Get("MyKey")!.Remove();
            Assert.IsFalse(eventRow.Settings.Has("MyKey"));
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void NonExistant_Setting() {
            League league = new();
            EventRow eventRow = league.EventTable.AddRow("my_event");
            Assert.IsNull(eventRow.Settings.Get("MyKey"));
        }

        [TestMethod]
        public void All_Teams_Simple() {
            League league = new();
            EventRow eventRow = league.EventTable.AddRow("my_event");
            RoundRow roundRow = eventRow.Rounds.Add();
            MatchRow matchRow = roundRow.Matches.Add(0, 10);
            TeamRow teamRow1 = matchRow.Teams.Add(0);
            TeamRow teamRow2 = matchRow.Teams.Add(1);

            league.PlayerTable.AddRow("Adam");
            league.PlayerTable.AddRow("Eve");
            teamRow1.Members.Add("Adam");
            teamRow1.Members.Add("Eve");

            league.PlayerTable.AddRow("Cain");
            league.PlayerTable.AddRow("Able");
            teamRow2.Members.Add("Cain");
            teamRow2.Members.Add("Able");

            int expected = 2;
            int actual = eventRow.MatchResults().Count;

            Console.WriteLine(league.PlayerTable.PrettyPrint());
            Console.WriteLine(league.MemberTable.PrettyPrint());
            Console.WriteLine(league.TeamTable.PrettyPrint());

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void All_Teams_Repeats() {
            League league = new();
            EventRow eventRow = league.EventTable.AddRow("my_event");

            RoundRow roundRow1 = eventRow.Rounds.Add();
            MatchRow matchRow1 = roundRow1.Matches.Add(0, 10);
            TeamRow teamRow1 = matchRow1.Teams.Add(0);
            TeamRow teamRow2 = matchRow1.Teams.Add(1);

            RoundRow roundRow2 = eventRow.Rounds.Add();
            MatchRow matchRow2 = roundRow2.Matches.Add(1, 10);
            TeamRow teamRow3 = matchRow2.Teams.Add(0);
            TeamRow teamRow4 = matchRow2.Teams.Add(1);

            league.PlayerTable.AddRow("Adam");
            league.PlayerTable.AddRow("Eve");
            teamRow1.Members.Add("Adam");
            teamRow1.Members.Add("Eve");
            teamRow3.Members.Add("Adam");
            teamRow3.Members.Add("Eve");

            league.PlayerTable.AddRow("Cain");
            league.PlayerTable.AddRow("Able");
            teamRow2.Members.Add("Cain");
            teamRow2.Members.Add("Able");
            teamRow4.Members.Add("Cain");
            teamRow4.Members.Add("Able");

            int expected = 2;
            var allTeams = eventRow.MatchResults();
            int actual = allTeams.Count;

            Console.WriteLine(league.PlayerTable.PrettyPrint());
            Console.WriteLine(league.MemberTable.PrettyPrint());
            Console.WriteLine(league.TeamTable.PrettyPrint());

            Assert.AreEqual(expected, actual);

            foreach (var pair in allTeams) {
                Console.WriteLine(pair.Key.Players.DelString());
            }
        }

        [TestMethod]
        public void All_Teams_Matches() {
            League league = new();
            EventRow eventRow = league.EventTable.AddRow("my_event");

            RoundRow roundRow1 = eventRow.Rounds.Add();
            MatchRow matchRow1 = roundRow1.Matches.Add(0, 10);
            TeamRow teamRow1 = matchRow1.Teams.Add(0);
            TeamRow teamRow2 = matchRow1.Teams.Add(1);

            RoundRow roundRow2 = eventRow.Rounds.Add();
            MatchRow matchRow2 = roundRow2.Matches.Add(1, 10);
            TeamRow teamRow3 = matchRow2.Teams.Add(0);
            TeamRow teamRow4 = matchRow2.Teams.Add(1);

            RoundRow roundRow3 = eventRow.Rounds.Add();
            MatchRow matchRow3 = roundRow3.Matches.Add(1, 10);
            TeamRow teamRow6 = matchRow3.Teams.Add(0);
            TeamRow teamRow7 = matchRow3.Teams.Add(1);

            league.PlayerTable.AddRow("Adam");
            league.PlayerTable.AddRow("Eve");
            teamRow1.Members.Add("Adam");
            teamRow1.Members.Add("Eve");
            teamRow3.Members.Add("Adam");
            teamRow3.Members.Add("Eve");

            league.PlayerTable.AddRow("Cain");
            league.PlayerTable.AddRow("Able");
            teamRow2.Members.Add("Cain");
            teamRow2.Members.Add("Able");
            teamRow4.Members.Add("Cain");
            teamRow4.Members.Add("Able");

            league.PlayerTable.AddRow("Barb");
            league.PlayerTable.AddRow("Lilith");
            teamRow6.Members.Add("Barb");
            teamRow6.Members.Add("Lilith");
            teamRow7.Members.Add("Cain");
            teamRow7.Members.Add("Able");

            var allTeams = eventRow.MatchResults();

            Assert.AreEqual(2, allTeams[new(teamRow1)].Count);
            Assert.AreEqual(3, allTeams[new(teamRow2)].Count);
            Assert.AreEqual(1, allTeams[new(teamRow6)].Count);

            foreach (MatchResults match in allTeams[new(teamRow2)]) {
                Console.WriteLine(match);
            }

        }

        [TestMethod]
        public void All_Teams_Sort() {
            League league = new();
            EventRow eventRow = league.EventTable.AddRow("my_event");

            RoundRow roundRow1 = eventRow.Rounds.Add();
            MatchRow matchRow1 = roundRow1.Matches.Add(0, 10);
            TeamRow teamRow1 = matchRow1.Teams.Add(0);
            TeamRow teamRow2 = matchRow1.Teams.Add(1);

            RoundRow roundRow2 = eventRow.Rounds.Add();
            MatchRow matchRow2 = roundRow2.Matches.Add(1, 10);
            TeamRow teamRow3 = matchRow2.Teams.Add(0);
            TeamRow teamRow4 = matchRow2.Teams.Add(1);

            RoundRow roundRow3 = eventRow.Rounds.Add();
            MatchRow matchRow3 = roundRow3.Matches.Add(1, 10);
            TeamRow teamRow6 = matchRow3.Teams.Add(0);
            TeamRow teamRow7 = matchRow3.Teams.Add(1);

            league.PlayerTable.AddRow("Adam");
            league.PlayerTable.AddRow("Eve");
            teamRow1.Members.Add("Adam");
            teamRow1.Members.Add("Eve");
            teamRow3.Members.Add("Adam");
            teamRow3.Members.Add("Eve");

            league.PlayerTable.AddRow("Cain");
            league.PlayerTable.AddRow("Able");
            teamRow2.Members.Add("Cain");
            teamRow2.Members.Add("Able");
            teamRow4.Members.Add("Cain");
            teamRow4.Members.Add("Able");

            league.PlayerTable.AddRow("Barb");
            league.PlayerTable.AddRow("Lilith");
            teamRow6.Members.Add("Barb");
            teamRow6.Members.Add("Lilith");
            teamRow7.Members.Add("Cain");
            teamRow7.Members.Add("Able");

            var allTeams = eventRow.MatchResults();

            Assert.AreEqual(2, allTeams[new(teamRow1)].Count);
            Assert.AreEqual(3, allTeams[new(teamRow2)].Count);
            Assert.AreEqual(1, allTeams[new(teamRow6)].Count);

            List<MatchResults> sorted = [.. allTeams[new(teamRow2)]];
            sorted.Reverse();
            //sorted.Sort(MatchResults.CompareByRound);

            foreach (MatchResults match in sorted) {
                Console.WriteLine(match);
            }

            Assert.AreEqual(0, sorted[0].Round);
            Assert.AreEqual(1, sorted[1].Round);
            Assert.AreEqual(2, sorted[2].Round);
        }
    }
}
