﻿using Leagueinator.Model;
using Leagueinator.Model.Tables;
using Model.Source.Tables.Event;

namespace Model_Test {
    [TestClass]
    public class TableExtensions_Test {
        [TestMethod]
        public void Has_True() {
            League league = new();
            EventRow eventRow = league.EventTable.AddRow("my_event");
            RoundRow roundRow = eventRow.Rounds.Add();
            MatchRow matchRow = roundRow.Matches.Add(0, 10);
            TeamRow teamRow = matchRow.Teams.Add(1);

            league.PlayerTable.AddRow("Adam");
            league.PlayerTable.AddRow("Eve");
            league.PlayerTable.AddRow("Cain");

            teamRow.Members.Add("Adam");
            teamRow.Members.Add("Eve");
            teamRow.Members.Add("Cain");

            bool actual = league.TeamTable.Has(
                [TeamTable.COL.MATCH, TeamTable.COL.INDEX],
                [matchRow.UID, 1]
            );

            Assert.AreEqual(true, actual);
        }

        [TestMethod]
        public void Has_False() {
            League league = new();
            EventRow eventRow = league.EventTable.AddRow("my_event");
            RoundRow roundRow = eventRow.Rounds.Add();
            MatchRow matchRow = roundRow.Matches.Add(0, 10);
            TeamRow teamRow = matchRow.Teams.Add(0);

            league.PlayerTable.AddRow("Adam");
            league.PlayerTable.AddRow("Eve");
            league.PlayerTable.AddRow("Cain");

            teamRow.Members.Add("Adam");
            teamRow.Members.Add("Eve");
            teamRow.Members.Add("Cain");

            bool actual = league.TeamTable.Has(
                [TeamTable.COL.MATCH, TeamTable.COL.INDEX],
                [matchRow.UID, 2]
            );

            Assert.AreEqual(false, actual);
        }
    }
}
