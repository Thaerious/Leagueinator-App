using Model;
using Model.Tables;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model_Test {
    internal class Mock1 : League{
        public Mock1() {
            EventRow myEvent = this.EventTable.AddRow("my_event");
            EventRow myOtherEvent = this.EventTable.AddRow("my_other_event");

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
