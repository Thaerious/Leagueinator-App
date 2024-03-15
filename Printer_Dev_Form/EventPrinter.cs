using Model;
using System.Drawing.Printing;
using System.Reflection;

namespace Printer_Dev_Form {
    internal class EventPrinter {
        public readonly Element documentXML;
        private readonly Element teamXML;
        private readonly Element roundXML;
        private readonly PlusScore plusScore;
        private readonly XMLLoader xmlLoader;

        public EventPrinter(League league) {
            Assembly assembly = Assembly.GetExecutingAssembly();

            this.xmlLoader = new XMLLoader();
            this.xmlLoader.LoadStyleResource(assembly, "Printer_Dev_Form.assets.style.css");

            this.documentXML = this.xmlLoader.LoadXMLResource(assembly, "Printer_Dev_Form.assets.document.xml");
            this.teamXML = this.xmlLoader.LoadXMLResource(assembly, "Printer_Dev_Form.assets.team.xml");
            this.roundXML = this.xmlLoader.LoadXMLResource(assembly, "Printer_Dev_Form.assets.round.xml");

            //this.plusScore = new PlusScore(league);

            //Debug.WriteLine(plusScore.PrettyPrint());
            this.ApplyData();
        }

        void ApplyData() {
            //foreach (int teamUID in plusScore.PlusTeams.AllIDs()) InsertTeam(teamUID);
            //xmlLoader.ApplyStyles(documentXML);
            //documentXML.Update();
        }

        //void InsertTeam(int teamUID) {
        //    var teamElement = documentXML.AddChild(teamXML.Clone());
        //    var names = plusScore.PlusTeams.TeamView(teamUID).ToList<string>(PlusTeams.COL.NAME);

        //    foreach (var name in names) {
        //        var player = new Element("player") {
        //            InnerText = name
        //        };
        //        teamElement["players"][0].AddChild(player);
        //    }

        //    var roundElement = roundXML.Clone();

        //    foreach (DataRowView _row in plusScore.PlusRounds.GetRowsByTeam(teamUID)) {
        //        var newRoundXML = roundXML.Clone();
        //        var row = _row.Row.Clone();

        //        row[PlusRounds.COL.ROUND] = (int)row[PlusRounds.COL.ROUND] + 1;
        //        row[PlusRounds.COL.LANE] = (int)row[PlusRounds.COL.LANE] + 1;

        //        newRoundXML.ApplyRowAsText(row);
        //        teamElement["rounds"][0].AddChild(newRoundXML);
        //    }

        //    var summaryRow = plusScore.PlusSummary.GetRow(teamUID);
        //    var summaryXML = roundXML.Clone();
        //    summaryXML.ApplyRowAsText(summaryRow);
        //    summaryXML["#round"][0].InnerText = "\u03A3";
        //    teamElement["rounds"][0].AddChild(summaryXML);

        //    documentXML.Update();
        //    Debug.WriteLine(documentXML.OuterRect.ToString());
        //}

        private bool DrawNextPage(Graphics? graphics) {
            return false;
        }

        public void HndPrint(object sender, PrintPageEventArgs e) {
            e.HasMorePages = this.DrawNextPage(e.Graphics);
        }
    }
}
