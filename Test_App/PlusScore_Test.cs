using Leagueinator.App.Scoring.Plus;
using Leagueinator.Utility;
using Model;
using Model.Tables;
using System.Diagnostics;

namespace Test_App {
    [TestClass]
    public class PlusScore_Test {
        [TestMethod]
        public void Sanity() {
            League league = new Mock();
            PlusScore plusScore = new(league.LeagueEvents[0]);
            Debug.WriteLine(plusScore.PrettyPrint());
        }

        [TestMethod]
        public void NextIndex_Empty() {
            PlusTeams plusTeam = new();
            Assert.AreEqual(0, plusTeam.NextIndex);
        }

        [TestMethod]
        public void Add_Team_If_One() {
            PlusTeams plusTeam = new();
            int index = plusTeam.AddTeamIf(["Adam", "Eve"]);

            Assert.AreEqual(0, index);
            Assert.AreEqual(0, plusTeam.FindTeam(["Adam", "Eve"]));
        }

        [TestMethod]
        public void Add_Team_If_Two() {
            PlusTeams plusTeam = new();
            plusTeam.AddTeamIf(["Adam", "Eve"]);
            int index = plusTeam.AddTeamIf(["Cain", "Able"]);

            //Assert.AreEqual(0, plusTeam.FindTeam(["Adam", "Eve"]));            
            //Assert.AreEqual(1, plusTeam.FindTeam(["Cain", "Able"]));
            Assert.AreEqual(1, index);
        }

        [TestMethod]
        public void Find_Team() {
            PlusTeams plusTeam = new();
            plusTeam.AddRow(0, "Adam");
            plusTeam.AddRow(0, "Eve");
            plusTeam.AddRow(1, "Cain");
            plusTeam.AddRow(1, "Able");

            Debug.WriteLine(plusTeam.GetView(0).PrettyPrint());
            Debug.WriteLine(plusTeam.GetView(0).ToList<string>("name").DelString());

            // exists
            Assert.AreEqual(0, plusTeam.FindTeam(["Adam", "Eve"]));
            Assert.AreEqual(1, plusTeam.FindTeam(["Cain", "Able"]));

            // exists, reverse order 
            Assert.AreEqual(0, plusTeam.FindTeam(["Eve", "Adam"]));
            Assert.AreEqual(1, plusTeam.FindTeam(["Able", "Cain"]));

            // doesn't exist
            Assert.AreEqual(-1, plusTeam.FindTeam(["Adam"]));
            Assert.AreEqual(-1, plusTeam.FindTeam(["Able", "Cain", "Billy"]));
        }

        [TestMethod]
        public void NextIndex_One() {
            PlusTeams plusTeam = new();
            plusTeam.AddRow(0, "Adam");
            Assert.AreEqual(1, plusTeam.NextIndex);
        }

        [TestMethod]
        public void Check_Ranks() {
            League league = new Mock();
            PlusRounds plusScore = new(league.LeagueEvents[0]);

            Assert.AreEqual(3, plusScore.GetRow(0)[PlusRounds.COL.RANK]);
            Assert.AreEqual(1, plusScore.GetRow(1)[PlusRounds.COL.RANK]);
            Assert.AreEqual(4, plusScore.GetRow(2)[PlusRounds.COL.RANK]);
            Assert.AreEqual(2, plusScore.GetRow(3)[PlusRounds.COL.RANK]);
            Assert.AreEqual(2, plusScore.GetRow(4)[PlusRounds.COL.RANK]);
            Assert.AreEqual(3, plusScore.GetRow(5)[PlusRounds.COL.RANK]);
            Assert.AreEqual(4, plusScore.GetRow(6)[PlusRounds.COL.RANK]);
            Assert.AreEqual(1, plusScore.GetRow(7)[PlusRounds.COL.RANK]);
        }
    }
}
