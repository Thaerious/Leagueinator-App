using Leagueinator.Model;
using Leagueinator.Model.Tables;
using Leagueinator.Model.Views;
using Leagueinator.Utility;
using System.Data;
using System.Diagnostics;

namespace Model_Test {
    [TestClass]
    public class TeamView_Test {

        [TestMethod]
        public void From_Event() {
            League league = new();
            EventRow eventRow = league.Events.Add("my_event");

            eventRow.Rounds.Add();

            eventRow.Rounds[0].Matches.Add(0);
            eventRow.Rounds[0].Matches.Add(1);

            eventRow.Rounds[0].Matches[0].Teams.Add(0);
            eventRow.Rounds[0].Matches[0].Teams.Add(1);

            eventRow.Rounds[0].Matches[1].Teams.Add(0);
            eventRow.Rounds[0].Matches[1].Teams.Add(1);

            eventRow.Rounds[0].Matches[1].Teams[0].Members.Add("Adam");
            eventRow.Rounds[0].Matches[1].Teams[0].Members.Add("Eve");

            eventRow.Rounds[0].Matches[1].Teams[1].Members.Add("Cain");
            eventRow.Rounds[0].Matches[1].Teams[1].Members.Add("Able");

            foreach (TeamView teamView in eventRow.TeamViews()) {
                Console.WriteLine($"({teamView.Players.DelString()})");
            }

            Assert.AreEqual(2, eventRow.TeamViews().Count);
            Assert.AreEqual(2, eventRow.Rounds[0].TeamViews().Count);
        }

        [TestMethod]
        public void No_Repeats_Across_Rounds() {
            League league = new();
            EventRow eventRow = league.Events.Add("my_event");

            eventRow.Rounds.Add();

            eventRow.Rounds[0].Matches.Add(0);
            eventRow.Rounds[0].Matches.Add(1);

            eventRow.Rounds[0].Matches[0].Teams.Add(0);
            eventRow.Rounds[0].Matches[0].Teams.Add(1);

            eventRow.Rounds[0].Matches[1].Teams.Add(0);
            eventRow.Rounds[0].Matches[1].Teams.Add(1);

            eventRow.Rounds[0].Matches[1].Teams[0].Members.Add("Adam");
            eventRow.Rounds[0].Matches[1].Teams[0].Members.Add("Eve");

            eventRow.Rounds[0].Matches[1].Teams[1].Members.Add("Cain");
            eventRow.Rounds[0].Matches[1].Teams[1].Members.Add("Able");

            eventRow.Rounds.Add();

            eventRow.Rounds[1].Matches.Add(0);
            eventRow.Rounds[1].Matches.Add(1);

            eventRow.Rounds[1].Matches[0].Teams.Add(0);
            eventRow.Rounds[1].Matches[0].Teams.Add(1);

            eventRow.Rounds[1].Matches[1].Teams.Add(0);
            eventRow.Rounds[1].Matches[1].Teams.Add(1);

            eventRow.Rounds[1].Matches[1].Teams[0].Members.Add("Adam");
            eventRow.Rounds[1].Matches[1].Teams[0].Members.Add("Eve");

            eventRow.Rounds[1].Matches[1].Teams[1].Members.Add("Cain");
            eventRow.Rounds[1].Matches[1].Teams[1].Members.Add("Able");

            foreach (TeamView teamView in eventRow.TeamViews()) {
                Console.WriteLine($"({teamView.Players.DelString()})");
            }

            Assert.AreEqual(2, eventRow.TeamViews().Count);
        }

        [TestMethod]
        public void Matches() {
            League league = new();
            EventRow eventRow = league.Events.Add("my_event");

            eventRow.Rounds.Add();

            eventRow.Rounds[0].Matches.Add(0);
            eventRow.Rounds[0].Matches.Add(1);

            eventRow.Rounds[0].Matches[0].Teams.Add(0);
            eventRow.Rounds[0].Matches[0].Teams.Add(1);

            eventRow.Rounds[0].Matches[1].Teams.Add(0);
            eventRow.Rounds[0].Matches[1].Teams.Add(1);

            eventRow.Rounds[0].Matches[1].Teams[0].Members.Add("Adam");
            eventRow.Rounds[0].Matches[1].Teams[0].Members.Add("Eve");

            eventRow.Rounds[0].Matches[1].Teams[1].Members.Add("Cain");
            eventRow.Rounds[0].Matches[1].Teams[1].Members.Add("Able");

            eventRow.Rounds.Add();

            eventRow.Rounds[1].Matches.Add(0);
            eventRow.Rounds[1].Matches.Add(1);

            eventRow.Rounds[1].Matches[0].Teams.Add(0);
            eventRow.Rounds[1].Matches[0].Teams.Add(1);

            eventRow.Rounds[1].Matches[1].Teams.Add(0);
            eventRow.Rounds[1].Matches[1].Teams.Add(1);

            eventRow.Rounds[1].Matches[1].Teams[0].Members.Add("Adam");
            eventRow.Rounds[1].Matches[1].Teams[0].Members.Add("Eve");

            eventRow.Rounds[1].Matches[1].Teams[1].Members.Add("Cain");
            eventRow.Rounds[1].Matches[1].Teams[1].Members.Add("Able");

            TeamView teamView = new(eventRow.Rounds[1].Matches[1].Teams[0]);

            Assert.AreEqual(2, teamView.Matches.Count);
        }
    }
}
