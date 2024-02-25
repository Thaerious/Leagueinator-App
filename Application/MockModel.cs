﻿using Model;

namespace Leagueinator {
    internal class MockModel : League{

        static int count = 0;

        public MockModel() {
            if (count > 0) throw new Exception();
            count++;

            var eventRow = this.EventTable.AddRow("My Event");
            var roundRow = eventRow.Rounds.Add();
            var matchRow = roundRow.Matches.Add(0, 10);
            var teamRow1 = matchRow.Teams.Add();
            var teamRow2 = matchRow.Teams.Add();

            teamRow1.Members.Add("Adam");
            teamRow2.Members.Add("Eve");
        }
    }
}
