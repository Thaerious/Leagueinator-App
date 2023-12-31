using Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model_Test {
    internal class Mock : League{
        public Mock() {
            LeagueEvent myEvent = this.NewLeagueEvent("my_event");
            LeagueEvent myOtherEvent = this.NewLeagueEvent("my_other_event");

            myEvent.NewRound();
            myEvent.NewRound();
            myEvent.NewRound();

            myEvent.Rounds[0].GetMatch(0).NewTeam();
            myEvent.Rounds[0].GetMatch(0).NewTeam();

            myEvent.Rounds[0].GetMatch(0).Teams[0].AddPlayer("Adam");
            myEvent.Rounds[0].GetMatch(0).Teams[0].AddPlayer("Betty");
            myEvent.Rounds[0].GetMatch(0).Teams[1].AddPlayer("Chuck");
            myEvent.Rounds[0].GetMatch(0).Teams[1].AddPlayer("Dianne");

            myEvent.NewRound();
            myEvent.Rounds[1].GetMatch(0).NewTeam();
            myEvent.Rounds[1].GetMatch(0).NewTeam();

            //myEvent.Rounds[1].GetMatch(0).Teams[0].AddPlayer("Adam");
            //myEvent.Rounds[1].GetMatch(0).Teams[0].AddPlayer("Dianne");
            //myEvent.Rounds[1].GetMatch(0).Teams[1].AddPlayer("Chuck");
            //myEvent.Rounds[1].GetMatch(0).Teams[1].AddPlayer("Betty");
        }
    }
}
