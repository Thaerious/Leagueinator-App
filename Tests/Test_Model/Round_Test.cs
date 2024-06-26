﻿using Leagueinator.Model;
using Leagueinator.Model.Tables;
using Leagueinator.Utility;
using System.Data;
using System.Diagnostics;

namespace Model_Test {
    [TestClass]
    public class Round_Test {

        [TestMethod]
        public void New_Round() {
            League league = new();
            EventRow eventRow = league.Events.Add("my_event");
            RoundRow roundRow = eventRow.Rounds.Add();
            Assert.IsNotNull(roundRow);
        }

        /// <summary>
        /// Populate matches fills the round with 'LaneCount' number of matches.
        /// </summary>
        [DataTestMethod]
        [DataRow(16)]
        [DataRow(8)]
        [DataRow(1)]
        public void Populate_Matches(int laneCount) {
            League league = new League();
            EventRow eventRow = league.Events.Add("my_event");
            RoundRow roundRow = eventRow.Rounds.Add();

            eventRow.LaneCount = laneCount;
            roundRow.PopulateMatches();

            Assert.AreEqual(laneCount, roundRow.Matches.Count);
            Console.WriteLine(league.MatchTable.PrettyPrint());
        }

        /// <summary>
        /// An empty round returns an empty match collection.
        /// </summary>
        [TestMethod]
        public void Empty_Round_Get_Matches() {
            League league = new League();
            EventRow eventRow = league.Events.Add("my_event");
            RoundRow roundRow = eventRow.Rounds.Add();

            Assert.IsNotNull(roundRow);
            Assert.AreEqual(roundRow.Matches.Count, 0);
        }

        [TestMethod]
        public void Get_First_Round_By_Index() {
            League league = new League();
            EventRow eventRow = league.Events.Add("my_event");
            eventRow.Rounds.Add();

            Assert.IsNotNull(eventRow.Rounds[0]);
        }

        [TestMethod]
        public void Get_Second_Round_By_Index() {
            League league = new League();
            EventRow eventRow = league.Events.Add("my_event");
            eventRow.Rounds.Add();
            eventRow.Rounds.Add();

            Assert.IsNotNull(eventRow.Rounds[1]);
        }

        [TestMethod]
        public void Round_Row_Remove() {
            League league = new League();
            EventRow eventRow = league.Events.Add("my_event");
            eventRow.Rounds.Add();
            eventRow.Rounds.Add();

            Debug.WriteLine(league.PrettyPrint());
            eventRow.Rounds[0]!.Remove();
            Debug.WriteLine(league.PrettyPrint());

            Assert.AreEqual(1, eventRow.Rounds.Count);
        }

        [TestMethod]
        public void Remove_Player_From_Teams() {
            League league = new League();
            EventRow eventRow = league.Events.Add("my_event");
            RoundRow roundRow = eventRow.Rounds.Add();
            MatchRow matchRow = roundRow.Matches.Add(0);
            TeamRow teamRow = matchRow.Teams.Add();

            teamRow.Members.Add("Adam");
            roundRow.RemoveNameFromTeams("Adam");
            Assert.IsFalse(teamRow.Members.Has("Adam"));
        }

        [TestMethod]
        public void Add_Player_To_Idle() {
            League league = new League();
            EventRow eventRow = league.Events.Add("my_event");
            RoundRow roundRow = eventRow.Rounds.Add();

            roundRow.IdlePlayers.Add("Zen");

            Assert.IsTrue(roundRow.IdlePlayers.Has("Zen"));
        }

        [TestMethod]
        public void Remove_Player_From_Idle() {
            League league = new League();
            EventRow eventRow = league.Events.Add("my_event");
            RoundRow roundRow = eventRow.Rounds.Add();

            roundRow.IdlePlayers.Add("Zen");
            roundRow.IdlePlayers.Get("Zen").Remove();

            Assert.IsFalse(roundRow.IdlePlayers.Has("Zen"));
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void Remove_Player_From_Idle_That_Is_Not_In_Idle() {
            League league = new League();
            EventRow eventRow = league.Events.Add("my_event");
            RoundRow roundRow = eventRow.Rounds.Add();

            roundRow.IdlePlayers.Get("Zen").Remove();
        }

        [TestMethod]
        public void Add_Idle_Player() {
            League league = new League();
            EventRow eventRow = league.Events.Add("my_event");
            RoundRow roundRow = eventRow.Rounds.Add();

            roundRow.IdlePlayers.Add("Zen");
            Assert.IsTrue(roundRow.IdlePlayers.Has("Zen"));
        }

        [DataTestMethod]
        [DataRow(["Zen"])]
        [DataRow(["Adam", "Eve"])]
        public void All_Members_List(String[] names) {
            League league = new League();
            EventRow eventRow = league.Events.Add("my_event");
            RoundRow roundRow = eventRow.Rounds.Add();
            MatchRow matchRow = roundRow.Matches.Add(0);
            TeamRow teamRow = matchRow.Teams.Add();

            foreach (string name in names) teamRow.Members.Add(name);

            List<MemberRow> memberList = roundRow.Members.ToList();
            List<string> nameList = roundRow.Members.Select(row => row.Player).ToList();

            Console.WriteLine(memberList.PrettyPrint("All Members"));
            Console.WriteLine(nameList.DelString());
            Console.WriteLine(memberList.Count);

            Assert.AreEqual(names.Length, memberList.Count);
        }

        [TestMethod]
        public void Set_Index_On_Create_Round() {
            League league = new League();
            EventRow eventRow1 = league.Events.Add("my_event1");
            EventRow eventRow2 = league.Events.Add("my_event2");

            // First round in an event has index of 0.
            Assert.AreEqual(0, eventRow1.Rounds.Add().Index);

            // Second round in an event has index of 1.
            Assert.AreEqual(1, eventRow1.Rounds.Add().Index);

            // First round in a different event has index of 0.
            Assert.AreEqual(0, eventRow2.Rounds.Add().Index);
        }

        [TestMethod]
        public void Get_Has_Row_By_Index() {
            League league = new League();
            EventRow eventRow1 = league.Events.Add("my_event1");
            EventRow eventRow2 = league.Events.Add("my_event2");

            var e1r0 = eventRow1.Rounds.Add();
            var e1r1 = eventRow1.Rounds.Add();
            var e2r0 = eventRow2.Rounds.Add();

            Assert.IsTrue(eventRow1.Rounds.Has(0));
            Assert.IsTrue(eventRow1.Rounds.Has(1));
            Assert.IsFalse(eventRow1.Rounds.Has(2));
            Assert.IsTrue(eventRow2.Rounds.Has(0));
            Assert.IsFalse(eventRow2.Rounds.Has(1));

            Assert.AreEqual(e1r0, eventRow1.Rounds.Get(0));
            Assert.AreEqual(e1r1, eventRow1.Rounds.Get(1));
            Assert.AreEqual(e2r0, eventRow2.Rounds.Get(0));
        }
    }
}
