using Model;

namespace Test_App {
    internal class Mock2 : League{
        public Mock2() {
            LeagueEvent myEvent = this.NewLeagueEvent("my_event");
            LeagueEvent myOtherEvent = this.NewLeagueEvent("my_other_event");

            var round = myEvent.NewRound();

            round.GetMatch(0).NewTeam();
            round.GetMatch(0).NewTeam();

            round.GetMatch(0).Teams[0].AddPlayer("Adam");
            round.GetMatch(0).Teams[0].AddPlayer("Betty");
            round.GetMatch(0).Teams[1].AddPlayer("Chuck");
            round.GetMatch(0).Teams[1].AddPlayer("Dianne");

            round.GetMatch(0).Teams[0].Bowls = 4;
            round.GetMatch(0).Teams[1].Bowls = 20;
            round.GetMatch(0).Teams[0].Ends = 10;
            round.GetMatch(0).Teams[1].Ends = 10;

            round.GetMatch(1).NewTeam();
            round.GetMatch(1).NewTeam();

            round.GetMatch(1).Teams[0].AddPlayer("Eve");
            round.GetMatch(1).Teams[0].AddPlayer("Fred");
            round.GetMatch(1).Teams[1].AddPlayer("Greg");
            round.GetMatch(1).Teams[1].AddPlayer("Hermione");

            round.GetMatch(1).Teams[0].Bowls = 4;
            round.GetMatch(1).Teams[1].Bowls = 9;
            round.GetMatch(1).Teams[0].Ends = 10;
            round.GetMatch(1).Teams[1].Ends = 10;

            round = myEvent.NewRound();

            round.GetMatch(0).NewTeam();
            round.GetMatch(0).NewTeam();

            round.GetMatch(0).Teams[0].AddPlayer("Eve");
            round.GetMatch(0).Teams[0].AddPlayer("Fred");
            round.GetMatch(0).Teams[1].AddPlayer("Chuck");
            round.GetMatch(0).Teams[1].AddPlayer("Dianne");

            round.GetMatch(0).Teams[0].Bowls = 4;
            round.GetMatch(0).Teams[1].Bowls = 4;
            round.GetMatch(0).Teams[0].Ends = 10;
            round.GetMatch(0).Teams[1].Ends = 10;
            round.GetMatch(0).Teams[0].Tie = 1;
            round.GetMatch(0).Teams[1].Tie = 0;

            round.GetMatch(1).NewTeam();
            round.GetMatch(1).NewTeam();

            round.GetMatch(1).Teams[0].AddPlayer("Adam");
            round.GetMatch(1).Teams[0].AddPlayer("Betty");
            round.GetMatch(1).Teams[1].AddPlayer("Greg");
            round.GetMatch(1).Teams[1].AddPlayer("Hermione");

            round.GetMatch(1).Teams[0].Bowls = 2;
            round.GetMatch(1).Teams[1].Bowls = 21;
            round.GetMatch(1).Teams[0].Ends = 10;
            round.GetMatch(1).Teams[1].Ends = 10;
        }
    }
}
