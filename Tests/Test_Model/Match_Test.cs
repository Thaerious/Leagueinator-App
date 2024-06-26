using Leagueinator.Model;
using Leagueinator.Model.Tables;
using System.Data;

namespace Model_Test {
    [TestClass]
    public class Match_Test {

        [TestMethod]
        public void Match() {
            League league = new();
            EventRow eventRow = league.Events.Add("my_event");
            RoundRow roundRow = eventRow.Rounds.Add();
            MatchRow matchRow = roundRow.Matches.Add(0, 10);

            Assert.IsNotNull(matchRow);
        }

        [TestMethod]
        public void Size_For_Empty_Match() {
            League league = new();
            EventRow eventRow = league.Events.Add("my_event");
            RoundRow roundRow = eventRow.Rounds.Add();
            MatchRow matchRow = roundRow.Matches.Add(0, 10);

            Assert.AreEqual(0, matchRow.Teams.Count);
        }

        [TestMethod]
        public void Size_For_One_Match() {
            League league = new();
            EventRow eventRow = league.Events.Add("my_event");
            RoundRow roundRow = eventRow.Rounds.Add();
            MatchRow matchRow = roundRow.Matches.Add(0, 10);

            Assert.AreEqual(1, roundRow.Matches.Count);
        }

        [TestMethod]
        public void Size_For_Two_Matches() {
            League league = new();
            EventRow eventRow = league.Events.Add("my_event");
            RoundRow roundRow = eventRow.Rounds.Add();
            roundRow.Matches.Add(0, 10);
            roundRow.Matches.Add(1, 10);

            Assert.AreEqual(2, roundRow.Matches.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ConstraintException))]
        public void Lane_Must_Be_Unique() {
            League league = new();
            EventRow eventRow = league.Events.Add("my_event");
            RoundRow roundRow = eventRow.Rounds.Add();
            roundRow.Matches.Add(0, 10);
            roundRow.Matches.Add(0, 10);
        }

        [TestMethod]
        public void Delete() {
            League league = new();
            EventRow eventRow = league.Events.Add("my_event");
            RoundRow roundRow = eventRow.Rounds.Add();
            MatchRow matchRow = roundRow.Matches.Add(0, 10);
            matchRow.Remove();

            Assert.AreEqual(0, roundRow.Matches.Count);
        }

        [TestMethod]
        public void Players() {
            League league = new();
            EventRow eventRow = league.Events.Add("my_event");
            RoundRow roundRow = eventRow.Rounds.Add();
            MatchRow matchRow = roundRow.Matches.Add(0, 10);
            matchRow.Teams.Add(0);
            matchRow.Teams.Add(1);

            Assert.IsNotNull(matchRow.Teams[0]);
            Assert.IsNotNull(matchRow.Teams[1]);

            matchRow.Teams[0]!.Members.Add("Adam");
            matchRow.Teams[0]!.Members.Add("Eve");
            matchRow.Teams[1]!.Members.Add("Cain");
            matchRow.Teams[1]!.Members.Add("Able");

            Console.WriteLine(matchRow.Members.PrettyPrint());
        }

        /// <summary>
        /// A new match has no players
        /// </summary>
        [TestMethod]
        public void Empty_Players() {
            League league = new();
            EventRow eventRow = league.Events.Add("my_event");
            RoundRow roundRow = eventRow.Rounds.Add();
            MatchRow matchRow = roundRow.Matches.Add(0, 10);
            matchRow.Teams.Add(0);
            matchRow.Teams.Add(1);

            Assert.AreEqual(0, matchRow.Members.Count);
        }

        [TestMethod]
        public void Retrieve_Teams_From_Specific_Match() {
            League league = new();
            EventRow eventRow = league.Events.Add("my_event");
            RoundRow roundRow = eventRow.Rounds.Add();
            MatchRow matchRow1 = roundRow.Matches.Add(0);
            matchRow1.Teams.Add(0);
            matchRow1.Teams.Add(1);

            MatchRow matchRow2 = roundRow.Matches.Add(1);
            matchRow2.Teams.Add(0);
            matchRow2.Teams.Add(1);

            Console.WriteLine(league.TeamTable.PrettyPrint());
            Console.WriteLine(matchRow1.PrettyPrint());
            Console.WriteLine(matchRow1.Teams.PrettyPrint());
            Console.WriteLine(matchRow2.PrettyPrint());
            Console.WriteLine(matchRow2.Teams.PrettyPrint());

            Assert.AreEqual(2, matchRow1.Teams.Count);
            Assert.AreEqual(2, matchRow2.Teams.Count);
            Assert.AreEqual(matchRow1, matchRow1.Teams[0].Match);
            Assert.AreEqual(matchRow2, matchRow2.Teams[1].Match);
        }
    }
}
