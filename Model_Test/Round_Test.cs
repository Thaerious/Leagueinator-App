using Model;
using Model.Tables;
using System.Diagnostics;

namespace Model_Test {
    [TestClass]
    public class Round_Test {

        [TestMethod]
        public void New_Round() {
            League league = new();
            EventRow eventRow = league.EventTable.AddRow("my_event");
            RoundRow roundRow = eventRow.Rounds.Add();
            Assert.IsNotNull(eventRow);
            Assert.AreEqual(0, roundRow.UID);
        }


        [TestMethod]
        public void Empty_Round() {
            League league = new League();
            EventRow eventRow = league.EventTable.AddRow("my_event");
            RoundRow roundRow = eventRow.Rounds.Add();
            Assert.IsNotNull(roundRow);
        }

        [TestMethod]
        public void Empty_Round_Get_Matches() {
            League league = new League();
            EventRow eventRow = league.EventTable.AddRow("my_event");
            RoundRow roundRow = eventRow.Rounds.Add();

            Assert.IsNotNull(roundRow);
            Assert.AreEqual(roundRow.Matches.Count, 0);
        }

        [TestMethod]
        public void Get_First_Round_By_Index() {
            League league = new League();
            EventRow eventRow = league.EventTable.AddRow("my_event");
            eventRow.Rounds.Add();

            Assert.IsNotNull(eventRow.Rounds[0]);
        }

        [TestMethod]
        public void Get_Second_Round_By_Index() {
            League league = new League();
            EventRow eventRow = league.EventTable.AddRow("my_event");
            eventRow.Rounds.Add();
            eventRow.Rounds.Add();

            Assert.IsNotNull(eventRow.Rounds[1]);
        }

        [TestMethod]
        public void Delete() {
            League league = new League();
            EventRow eventRow = league.EventTable.AddRow("my_event");
            eventRow.Rounds.Add();
            eventRow.Rounds.Add();

            Debug.WriteLine(league.PrettyPrint());
            eventRow.Rounds[0].DataRow.Delete();
            Debug.WriteLine(league.PrettyPrint());

            Assert.AreEqual(1, eventRow.Rounds.Count);
        }

        [TestMethod]
        public void Add_Player_To_Idle() {
            League league = new League();
            EventRow eventRow = league.EventTable.AddRow("my_event");
            RoundRow roundRow = eventRow.Rounds.Add();

            league.PlayerTable.AddRow("Zen");
            roundRow.IdlePlayers.Add("Zen");

            Assert.IsTrue(roundRow.IdlePlayers.Has("Player", "Zen"));
        }

        [TestMethod]
        public void Add_Non_Existant_Player_To_Idle() {
            League league = new League();
            EventRow eventRow = league.EventTable.AddRow("my_event");
            RoundRow roundRow = eventRow.Rounds.Add();

            roundRow.IdlePlayers.Add("Zen");

            Assert.IsTrue(roundRow.IdlePlayers.Has("Player", "Zen"));
        }

        [TestMethod]
        public void Idle_Remove_Contains() {
            League league = new League();
            EventRow eventRow = league.EventTable.AddRow("my_event");
            RoundRow roundRow = eventRow.Rounds.Add();

            league.PlayerTable.AddRow("Zen");
            roundRow.IdlePlayers.Add("Zen");
            roundRow.IdlePlayers.Get("Player", "Zen")!.Delete();

            Assert.IsFalse(roundRow.IdlePlayers.Has("Name", "Zen"));
        }

        //[TestMethod]
        //public void Idle_Iterator() {
        //    League league = new League();
        //    EventRow eventRow = league.EventTable.AddRow("my_event");
        //    RoundRow roundRow = eventRow.Rounds.Add();

        //    league.PlayerTable.AddRow("Zen");
        //    roundRow.IdlePlayers.Add("Zen");

        //    foreach (IdleRow row in roundRow.IdlePlayers) {
        //        Assert.AreEqual("Zen", row.Player.Player);
        //    }
        //}

        //[TestMethod]
        //public void AllPlayers() {
        //    League league = new League();
        //    LeagueEvent lEvent = league.NewLeagueEvent("my_event");
        //    Round round = lEvent.NewRound();
        //    Match match = round.GetMatch(0);

        //    Team team1 = match.NewTeam();
        //    team1.AddPlayer("Adam");
        //    team1.AddPlayer("Eve");

        //    Team team2 = match.NewTeam();
        //    team1.AddPlayer("Chucky");
        //    team1.AddPlayer("Dianne");

        //    round.IdlePlayers.Add("Zed");

        //    var expected = new List<string>() { "Adam", "Eve", "Chucky", "Dianne", "Zed" };

        //    Debug.WriteLine(league.PrettyPrint());
        //    CollectionAssert.AreEquivalent(expected, round.AllPlayers as System.Collections.ICollection);
        //}

        //[TestMethod]
        //public void Members() {
        //    League league = new League();
        //    LeagueEvent lEvent = league.NewLeagueEvent("my_event");
        //    Round round = lEvent.NewRound();
        //    Match match = round.GetMatch(0);

        //    Team team1 = match.NewTeam();
        //    team1.AddPlayer("Adam");
        //    team1.AddPlayer("Eve");

        //    Team team2 = match.NewTeam();
        //    team2.AddPlayer("Chucky");
        //    team2.AddPlayer("Dianne");

        //    var expected = new List<string>() { "Adam", "Eve", "Chucky", "Dianne"};

        //    Debug.WriteLine(league.PrettyPrint());
        //    CollectionAssert.AreEquivalent(expected, round.Members.ColValues());
        //}

        //[TestMethod]
        //public void Reset() {
        //    League league = new League();
        //    LeagueEvent lEvent = league.NewLeagueEvent("my_event");
        //    Round round = lEvent.NewRound();
        //    Match match = round.GetMatch(0);

        //    Team team1 = match.NewTeam();
        //    team1.AddPlayer("Adam");
        //    team1.AddPlayer("Eve");

        //    Team team2 = match.NewTeam();
        //    team2.AddPlayer("Chucky");
        //    team2.AddPlayer("Dianne");

        //    round.IdlePlayers.Add("Zed");
        //    round.ResetPlayers();

        //    var expected = new List<string>() { "Adam", "Eve", "Chucky", "Dianne", "Zed" };

        //    Debug.WriteLine(league.PrettyPrint());
        //    Debug.WriteLine(round.IdlePlayers.DelString());
        //    CollectionAssert.AreEquivalent(expected, round.IdlePlayers.ColValues());
        //}
    }
}
