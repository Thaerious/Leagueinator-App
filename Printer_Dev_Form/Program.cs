using Leagueinator.Printer;
using Model.Scoring.Plus;
using Model.Tables;
using Printer_Dev_Form;
using System.Data;
using System.Diagnostics;
using System.Reflection;

Assembly assembly = Assembly.GetExecutingAssembly();

var xmlLoader = new XMLLoader();
xmlLoader.LoadStyleResource(assembly, "Printer_Dev_Form.assets.style.css");

var documentXML = xmlLoader.LoadXMLResource(assembly, "Printer_Dev_Form.assets.document.xml");
var teamXML = xmlLoader.LoadXMLResource(assembly, "Printer_Dev_Form.assets.team.xml");
var roundXML = xmlLoader.LoadXMLResource(assembly, "Printer_Dev_Form.assets.round.xml");

ApplicationConfiguration.Initialize();
var form = new Form1();
form.canvas.Root = documentXML;

var league = new Mock();
var plusScore = new PlusScore(league.LeagueEvents[0]);

Debug.WriteLine(plusScore.PrettyPrint());
ApplyData();

Application.Run(form);

void ApplyData() {
    foreach (int teamUID in plusScore.PlusTeams.AllIDs()) InsertTeam(teamUID);
    xmlLoader.ApplyStyles(documentXML);
    documentXML.Update();
}

void InsertTeam(int teamUID) {
    var teamElement = documentXML.AddChild(teamXML.Clone());
    var names = plusScore.PlusTeams.TeamView(teamUID).ToList<string>(PlusTeams.COL.NAME);

    foreach (var name in names) {
        var player = new PrinterElement("player") {
            InnerText = name
        };
        teamElement["players"][0].AddChild(player);
    }

    var roundElement = roundXML.Clone();

    foreach (DataRowView _row in plusScore.PlusRounds.GetRowsByTeam(teamUID)) {
        var newRoundXML = roundXML.Clone();
        var row = _row.Row.Clone();

        row[PlusRounds.COL.ROUND] = (int)row[PlusRounds.COL.ROUND] + 1;
        row[PlusRounds.COL.LANE] = (int)row[PlusRounds.COL.LANE] + 1;

        newRoundXML.ApplyRowAsText(row);
        teamElement["rounds"][0].AddChild(newRoundXML);
    }

    var summaryRow = plusScore.PlusSummary.GetRow(teamUID);
    var summaryXML = roundXML.Clone();
    summaryXML.ApplyRowAsText(summaryRow);
    summaryXML["#round"][0].InnerText = "\u03A3";
    teamElement["rounds"][0].AddChild(summaryXML);

    documentXML.Update();
    Debug.WriteLine(documentXML.OuterRect.ToString());
}
