﻿using Leagueinator.Model;
using Leagueinator.Model.Tables;
using System.Data;

namespace Model_Test {
    [TestClass]
    public class Team_Row_Test {

        [TestMethod]
        public void TeamTable_Add() {
            League league = new();
            EventRow eventRow = league.Events.Add("my_event");
            RoundRow roundRow = eventRow.Rounds.Add();
            MatchRow matchRow = roundRow.Matches.Add(0, 10); // lane, ends
            TeamRow teamRow = matchRow.Teams.Add(0);

            Assert.IsNotNull(teamRow);            
        }

        [TestMethod]
        public void Add_Player() {
            League league = new();
            EventRow eventRow = league.Events.Add("my_event");
            RoundRow roundRow = eventRow.Rounds.Add();
            MatchRow matchRow = roundRow.Matches.Add(0, 10);
            TeamRow teamRow = matchRow.Teams.Add(1);

            teamRow.Members.Add("Adam");

            Assert.IsNotNull(teamRow);
        }

        /// <summary>
        /// Adding a player to the idle table for a given round will
        /// remove it from any team it is in.
        /// </summary>
        [TestMethod]
        public void Add_Player_To_Idle() {
            League league = new League();
            EventRow eventRow = league.Events.Add("my_event");
            RoundRow roundRow = eventRow.Rounds.Add();
            MatchRow matchRow = roundRow.Matches.Add(0, 10);
            TeamRow teamRow = matchRow.Teams.Add(1);

            teamRow.Members.Add("Adam");
            Assert.IsTrue(teamRow.Members.Has("Adam"));

            roundRow.IdlePlayers.Add("Adam");
            Assert.IsTrue(roundRow.IdlePlayers.Has("Adam"));
            Assert.IsFalse(teamRow.Members.Has("Adam"));
        }

        /// <summary>
        /// For a given round, adding a player to a team will remove it from any 
        /// other teams it is in.
        /// </summary>
        [TestMethod]
        public void Add_Player_To_Second_Team() {
            League league = new League();
            EventRow eventRow = league.Events.Add("my_event");
            RoundRow roundRow = eventRow.Rounds.Add();
            MatchRow matchRow = roundRow.Matches.Add(0, 10);
            TeamRow teamRow0 = matchRow.Teams.Add(0);
            TeamRow teamRow1 = matchRow.Teams.Add(1);

            teamRow0.Members.Add("Adam");
            Assert.IsTrue(teamRow0.Members.Has("Adam"));

            teamRow1.Members.Add("Adam");
            Assert.IsTrue(teamRow1.Members.Has("Adam"));
            Assert.IsFalse(teamRow0.Members.Has("Adam"));
        }

        /// <summary>
        /// For a given round, adding a player to the idle table will remove 
        /// that player from the team table.
        /// </summary>
        [TestMethod]
        public void Add_Player_From_Idle() {
            League league = new();
            EventRow eventRow = league.Events.Add("my_event");
            RoundRow roundRow = eventRow.Rounds.Add();
            MatchRow matchRow = roundRow.Matches.Add(0, 10);
            TeamRow teamRow = matchRow.Teams.Add(1);

            roundRow.IdlePlayers.Add("Adam");
            Assert.IsTrue(roundRow.IdlePlayers.Has("Adam"));
            teamRow.Members.Add("Adam");
            Assert.IsFalse(roundRow.IdlePlayers.Has("Adam"));
            Assert.IsTrue(teamRow.Members.Has("Adam"));
        }

        [TestMethod]
        public void Retrieve_Player() {
            League league = new();
            EventRow eventRow = league.Events.Add("my_event");
            RoundRow roundRow = eventRow.Rounds.Add();
            MatchRow matchRow = roundRow.Matches.Add(0, 10);
            TeamRow teamRow = matchRow.Teams.Add(1);

            teamRow.Members.Add("Adam");

            Assert.AreEqual("Adam", teamRow.Members[0].Player);
        }

        [TestMethod]
        [ExpectedException(typeof(ConstraintException))]
        public void Add_Existing_Player() {
            League league = new();
            EventRow eventRow = league.Events.Add("my_event");
            RoundRow roundRow = eventRow.Rounds.Add();
            MatchRow matchRow = roundRow.Matches.Add(0, 10);
            TeamRow teamRow = matchRow.Teams.Add(1);

            teamRow.Members.Add("Adam");
            teamRow.Members.Add("Adam");
        }

        //[TestMethod]
        //public void Add_Multiple_Players_Mulitple_Teams() {
        //    League league = new();
        //    var eventRow = league.Events.Add("My Event");
        //    var roundRow = eventRow.Rounds.Add();
        //    var matchRow = roundRow.Matches.Add(0, 10);
        //    var teamRow1 = matchRow.Teams.Add();
        //    var teamRow2 = matchRow.Teams.Add();

        //    Console.WriteLine(teamRow1.UID);
        //    Console.WriteLine(teamRow2.UID);
        //    Assert.AreNotEqual(teamRow1.UID, teamRow2.UID);

        //    MemberRow memberRow1 = teamRow1.Members.Add("Adam");
        //    MemberRow memberRow2 = teamRow2.Members.Add("Eve");

        //    Console.WriteLine(memberRow1.Team);
        //    Console.WriteLine(memberRow2.Team);
        //    Assert.AreNotEqual(memberRow1.Team.UID, memberRow2.Team.UID);
        //}

        //[TestMethod]
        //public void List_Players() {
        //    League league = new();
        //    EventRow eventRow = league.Events.Add("my_event");
        //    RoundRow roundRow = eventRow.Rounds.Add();
        //    MatchRow matchRow = roundRow.Matches.Add(0, 10);
        //    TeamRow teamRow = matchRow.Teams.Add();

        //    league.PlayerTable.Add("Adam");
        //    league.PlayerTable.Add("Eve");
        //    league.PlayerTable.Add("Cain");
        //    league.PlayerTable.Add("Able");

        //    teamRow.Members.Add("Adam");
        //    teamRow.Members.Add("Eve");
        //    teamRow.Members.Add("Cain");
        //    teamRow.Members.Add("Able");

        //    List<string> expected = ["Adam", "Eve", "Cain", "Able"];
        //    Console.WriteLine(teamRow.Members[0]);
        //    List<string> actual = teamRow.Members.Cast<string>();
        //    CollectionAssert.AreEquivalent(expected, actual);
        //}

        //[TestMethod]
        //public void Has_Player_True() {
        //    League league = new League();
        //    LeagueEvent lEvent = league.NewLeagueEvent("my_event");
        //    RoundIndex round = lEvent.NewRound();
        //    MatchResultsPlus match = round.GetMatch(0);
        //    Team team = match.NewTeam();
        //    team.AddPlayer("Adam");
        //    bool actual = team.HasPlayer("Adam");

        //    Assert.IsTrue(actual);
        //}

        //[TestMethod]
        //public void Has_Player_True_Many() {
        //    League league = new League();
        //    LeagueEvent lEvent = league.NewLeagueEvent("my_event");
        //    RoundIndex round = lEvent.NewRound();
        //    MatchResultsPlus match = round.GetMatch(0);
        //    Team team = match.NewTeam();
        //    team.AddPlayer("Adam");
        //    team.AddPlayer("Bart");
        //    team.AddPlayer("Carly");
        //    team.AddPlayer("Dianne");

        //    Debug.WriteLine(league.PrettyPrint());

        //    Assert.IsTrue(team.HasPlayer("Adam"));
        //    Assert.IsTrue(team.HasPlayer("Bart"));
        //    Assert.IsTrue(team.HasPlayer("Carly"));
        //    Assert.IsTrue(team.HasPlayer("Dianne"));
        //}

        //[TestMethod]
        //public void Has_Player_False() {
        //    League league = new League();
        //    LeagueEvent lEvent = league.NewLeagueEvent("my_event");
        //    RoundIndex round = lEvent.NewRound();
        //    MatchResultsPlus match = round.GetMatch(0);
        //    Team team = match.NewTeam();
        //    bool actual = team.HasPlayer("Adam");

        //    Assert.IsFalse(actual);
        //}

        //[TestMethod]
        //public void Get_Players_Empty() {
        //    League league = new League();
        //    LeagueEvent lEvent = league.NewLeagueEvent("my_event");
        //    RoundIndex round = lEvent.NewRound();
        //    MatchResultsPlus match = round.GetMatch(0);
        //    Team team = match.NewTeam();
        //    List<String> list = team.Members;

        //    Assert.AreEqual(0, list.Count);
        //}

        //[TestMethod]
        //public void Get_Players_One() {
        //    League league = new League();
        //    LeagueEvent lEvent = league.NewLeagueEvent("my_event");
        //    RoundIndex round = lEvent.NewRound();
        //    MatchResultsPlus match = round.GetMatch(0);
        //    Team team = match.NewTeam();
        //    team.AddPlayer("Adam");
        //    List<string> list = team.Members;

        //    Assert.AreEqual(1, list.Count);
        //    Assert.IsTrue(list.Has("Adam"));
        //}

        //[TestMethod]
        //public void Get_Players_Many() {
        //    League league = new League();
        //    LeagueEvent lEvent = league.NewLeagueEvent("my_event");
        //    RoundIndex round = lEvent.NewRound();
        //    MatchResultsPlus match = round.GetMatch(0);
        //    Team team = match.NewTeam();
        //    team.AddPlayer("Adam");
        //    team.AddPlayer("Bart");
        //    team.AddPlayer("Carly");
        //    team.AddPlayer("Dianne");
        //    List<string> list = team.Members;

        //    Debug.WriteLine(league.PrettyPrint());

        //    Assert.AreEqual(4, list.Count);
        //    Assert.IsTrue(list.Has("Adam"));
        //    Assert.IsTrue(list.Has("Bart"));
        //    Assert.IsTrue(list.Has("Carly"));
        //    Assert.IsTrue(list.Has("Dianne"));
        //}

        //[TestMethod]
        //public void Remove_Player_Exists() {
        //    League league = new League();
        //    LeagueEvent lEvent = league.NewLeagueEvent("my_event");
        //    RoundIndex round = lEvent.NewRound();
        //    MatchResultsPlus match = round.GetMatch(0);
        //    Team team = match.NewTeam();
        //    team.AddPlayer("Adam");

        //    bool result = team.RemovePlayer("Adam");
        //    List<string> list = team.Members;

        //    Assert.AreEqual(0, list.Count);
        //    Assert.IsFalse(list.Has("Adam"));
        //    Assert.IsTrue(result);
        //}

        //[TestMethod]
        //public void Remove_Player_Multiple_Teams() {
        //    League league = new League();
        //    LeagueEvent lEvent = league.NewLeagueEvent("my_event");
        //    RoundIndex round = lEvent.NewRound();
        //    MatchResultsPlus match = round.GetMatch(0);
        //    match.NewTeam().AddPlayer("Adam");
        //    match.NewTeam().AddPlayer("Eve");

        //    bool result = match.Teams[1].RemovePlayer("Eve");
        //    List<string> list = match.Teams[1].Members;

        //    Assert.AreEqual(0, list.Count);
        //    Assert.IsTrue(result);
        //}

        //[TestMethod]
        //public void Remove_Second_Of_Two() {
        //    League league = new League();
        //    LeagueEvent lEvent = league.NewLeagueEvent("my_event");
        //    RoundIndex round = lEvent.NewRound();
        //    MatchResultsPlus match = round.GetMatch(0);
        //    Team team = match.NewTeam();
        //    team.AddPlayer("Adam");
        //    team.AddPlayer("Eve");

        //    bool result = team.RemovePlayer("Eve");
        //    List<string> list = team.Members;

        //    Debug.WriteLine(league.PrettyPrint());
        //    Assert.AreEqual(1, list.Count);
        //    Assert.IsTrue(list.Has("Adam"));
        //    Assert.IsFalse(list.Has("Eve"));
        //    Assert.IsTrue(result);
        //}

        //[TestMethod]
        //public void Remove_First_Of_Two() {
        //    League league = new League();
        //    LeagueEvent lEvent = league.NewLeagueEvent("my_event");
        //    RoundIndex round = lEvent.NewRound();
        //    MatchResultsPlus match = round.GetMatch(0);
        //    Team team = match.NewTeam();
        //    team.AddPlayer("Adam");
        //    team.AddPlayer("Eve");

        //    bool result = team.RemovePlayer("Adam");
        //    List<string> list = team.Members;

        //    Debug.WriteLine(league.PrettyPrint());
        //    Assert.AreEqual(1, list.Count);
        //    Assert.IsFalse(list.Has("Adam"));
        //    Assert.IsTrue(list.Has("Eve"));
        //    Assert.IsTrue(result);
        //}

        //[TestMethod]
        //public void Remove_Player_Not_Exists() {
        //    League league = new League();
        //    LeagueEvent lEvent = league.NewLeagueEvent("my_event");
        //    RoundIndex round = lEvent.NewRound();
        //    MatchResultsPlus match = round.GetMatch(0);
        //    Team team = match.NewTeam();
        //    bool result = team.RemovePlayer("Adam");
        //    List<string> list = team.Members;

        //    Assert.AreEqual(0, list.Count);
        //    Assert.IsFalse(list.Has("Adam"));
        //    Assert.IsFalse(result);
        //}

        //[TestMethod]
        //public void Remove_Player_Multiple_Rounds() {
        //    League league = new League();
        //    LeagueEvent lEvent = league.NewLeagueEvent("my_event");

        //    bool r1 = lEvent.NewRound()
        //        .GetMatch(0)
        //        .NewTeam()
        //        .AddPlayer("Adam");

        //    bool r2 = lEvent.NewRound()
        //        .GetMatch(0)
        //        .NewTeam()
        //        .AddPlayer("Adam");

        //    Assert.IsTrue(r1);
        //    Assert.IsTrue(r2);

        //    bool result = lEvent.Rounds[0].GetMatch(0).Teams[0].RemovePlayer("Adam");

        //    Debug.WriteLine(league.PrettyPrint());

        //    // player is removed from first round
        //    List<string> list1 = lEvent.Rounds[0].GetMatch(0).Teams[0].Members;
        //    Assert.AreEqual(0, list1.Count);
        //    Assert.IsFalse(list1.Has("Adam"));
        //    Assert.IsTrue(result);

        //    // player is not removed from second round
        //    List<string> list2 = lEvent.Rounds[1].GetMatch(0).Teams[0].Members;
        //    Assert.AreEqual(1, list2.Count);
        //    Assert.IsTrue(list2.Has("Adam"));
        //}
    }
}
