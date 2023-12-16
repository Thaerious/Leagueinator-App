using Model;
using Model.Tables;
using System.Data;
using System.Diagnostics;

namespace Model_Test {
    [TestClass]
    public class Integration_Test {
        public static League Mock() {
            League league = new();
            LeagueEvent myEvent = league.AddLeagueEvent("my_event");
            LeagueEvent myOtherEvent = league.AddLeagueEvent("my_other_event");
            
            myEvent.NewRound();
            myEvent.NewRound();
            myEvent.NewRound();

            myEvent.Rounds[0].GetMatch(0).NewTeam();
            myEvent.Rounds[0].GetMatch(0).NewTeam();

            myEvent.Rounds[0].Matches[0].Teams[0].AddPlayer("Adam");
            myEvent.Rounds[0].Matches[0].Teams[0].AddPlayer("Betty");
            myEvent.Rounds[0].Matches[0].Teams[1].AddPlayer("Chuck");
            myEvent.Rounds[0].Matches[0].Teams[1].AddPlayer("Dianne");

            //myEvent.AddRow(round: 1, lane: 1, teamID: 0, bowls: 0, ends: 0, tiebreaker: 0);
            //myEvent.AddRow(round: 1, lane: 1, teamID: 1, bowls: 0, ends: 0, tiebreaker: 0);
            //myEvent.AddRow(round: 1, lane: 2, teamID: 2, bowls: 0, ends: 0, tiebreaker: 0);
            //myEvent.AddRow(round: 1, lane: 2, teamID: 3, bowls: 0, ends: 0, tiebreaker: 0);

            //myEvent.AddRow(round: 2, lane: 1, teamID: 0, bowls: 0, ends: 0, tiebreaker: 0);
            //myEvent.AddRow(round: 2, lane: 2, teamID: 1, bowls: 0, ends: 0, tiebreaker: 0);
            //myEvent.AddRow(round: 2, lane: 1, teamID: 2, bowls: 0, ends: 0, tiebreaker: 0);
            //myEvent.AddRow(round: 2, lane: 2, teamID: 3, bowls: 0, ends: 0, tiebreaker: 0);

            return league;
        }

        [TestMethod]
        public void Constructor_Sanity() {
            League league = new();
            Assert.IsTrue(league != null);
        }

        [TestMethod]
        public void Count_Rounds_From_League_Event() {
            League league = Mock();
            LeagueEvent lEvent = league.LeagueEvents["my_event"];
            Debug.WriteLine(lEvent.PrettyPrint());
            Assert.IsTrue(lEvent.Count == 8);
        }

        //[TestMethod]
        //public void Retrieve_Round_From_League_Event() {
        //    League league = Mock();
        //    LeagueEvent lEvent = league.LeagueEvents["my_event"];
        //    Round round = lEvent.Rounds[1];

        //    Debug.WriteLine(round.PrettyPrint());
        //    Assert.IsTrue(round.Count == 4);
        //}

        //[TestMethod]
        //public void Retrieve_Match_From_Round() {
        //    League league = Mock();
        //    LeagueEvent lEvent = league.LeagueEvents["my_event"];
        //    Round round = lEvent.Rounds[1];
        //    Match match = round.Matches[1];

        //    Debug.WriteLine(round.PrettyPrint());
        //    Debug.WriteLine(match.PrettyPrint());            

        //    //Assert.IsTrue(match.Count == 2);
        //}
    }
}
