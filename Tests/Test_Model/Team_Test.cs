using Leagueinator.Model;
using Leagueinator.Model.Tables;
using Leagueinator.Model.Views;

namespace Model_Test {
    [TestClass]
    public class Team_Test {

        [TestMethod]
        public void Same_Hash_Code() {
            League league = new();
            EventRow eventRow = league.Events.Add("my_event");
            RoundRow roundRow = eventRow.Rounds.Add();
            MatchRow matchRow = roundRow.Matches.Add(0, 10);
            TeamRow teamRow1 = matchRow.Teams.Add(0);

            teamRow1.Members.Add("Adam");
            teamRow1.Members.Add("Eve");

            Team team1 = new(teamRow1);
            Team team2 = new(teamRow1);
            Console.WriteLine(team1.GetHashCode());
            Console.WriteLine(team2.GetHashCode());

            Assert.AreEqual(team1.GetHashCode(), team2.GetHashCode());
        }

        [TestMethod]
        public void Equals() {
            League league = new();
            EventRow eventRow = league.Events.Add("my_event");
            RoundRow roundRow = eventRow.Rounds.Add();
            MatchRow matchRow = roundRow.Matches.Add(0, 10);
            TeamRow teamRow1 = matchRow.Teams.Add(0);

            teamRow1.Members.Add("Adam");
            teamRow1.Members.Add("Eve");

            Team team1 = new(teamRow1);
            Team team2 = new(teamRow1);
            Console.WriteLine(team1.GetHashCode());
            Console.WriteLine(team2.GetHashCode());

            Assert.AreEqual(team1, team2);
        }

        [TestMethod]
        public void Retrieve_Members_From_Specific_Team() {
            League league = new();
            EventRow eventRow = league.Events.Add("my_event");
            RoundRow roundRow = eventRow.Rounds.Add();
            roundRow.Matches.Add(0);
            roundRow.Matches.Add(1);

            roundRow.Matches[0].Teams.Add(0);
            roundRow.Matches[0].Teams.Add(1);

            roundRow.Matches[1].Teams.Add(0);
            roundRow.Matches[1].Teams.Add(1);

            roundRow.Matches[0].Teams[0].Members.Add("Adam");
            roundRow.Matches[0].Teams[0].Members.Add("Bart");
            roundRow.Matches[0].Teams[1].Members.Add("Cain");
            roundRow.Matches[0].Teams[1].Members.Add("Dave");

            roundRow.Matches[1].Teams[0].Members.Add("Einstein");
            roundRow.Matches[1].Teams[0].Members.Add("Fred");

            Console.WriteLine(roundRow.Matches[0].Teams[0].Members.PrettyPrint());
            Console.WriteLine(roundRow.Matches[0].Teams[1].Members.PrettyPrint());
            Console.WriteLine(roundRow.Matches[1].Teams[0].Members.PrettyPrint());
            Console.WriteLine(roundRow.Matches[1].Teams[1].Members.PrettyPrint());

            Assert.AreEqual(2, roundRow.Matches[0].Teams[0].Members.Count);
            Assert.AreEqual(2, roundRow.Matches[0].Teams[1].Members.Count);
            Assert.AreEqual(2, roundRow.Matches[1].Teams[0].Members.Count);
            Assert.AreEqual(0, roundRow.Matches[1].Teams[1].Members.Count);

            Assert.AreEqual("Adam", roundRow.Matches[0].Teams[0].Members[0].Player);
            Assert.AreEqual("Bart", roundRow.Matches[0].Teams[0].Members[1].Player);
            Assert.AreEqual("Cain", roundRow.Matches[0].Teams[1].Members[0].Player);
            Assert.AreEqual("Dave", roundRow.Matches[0].Teams[1].Members[1].Player);
            Assert.AreEqual("Einstein", roundRow.Matches[1].Teams[0].Members[0].Player);
            Assert.AreEqual("Fred", roundRow.Matches[1].Teams[0].Members[1].Player);
        }
    }
}
