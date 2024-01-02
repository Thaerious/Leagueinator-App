using Model;
using Leagueinator.Printer;
using Leagueinator.Utility;
using Model.Tables;
using Printer_Dev_Form;
using System.Data;
using System.Diagnostics;
using System.Numerics;
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

var eventData = league.EventTable;
var teamTable = league.TeamTable;
var eventTable = league.EventTable;
var summaryTable = new SummaryTable(eventData);

Debug.WriteLine(eventTable);
Debug.WriteLine(teamTable);
Debug.WriteLine(summaryTable);
ApplyData();

Application.Run(form);

void ApplyData() {
    foreach (int teamUID in teamTable.AllIDs()) InsertTeam(teamUID);
    xmlLoader.ApplyStyles(documentXML);
    documentXML.Update();
}

void InsertTeam(int teamUID) {
    var teamElement = documentXML.AddChild(teamXML.Clone());

    foreach (var name in teamTable.GetNames(teamUID)) {
        var player = new PrinterElement("player") {
            InnerText = name
        };
        teamElement["players"][0].AddChild(player);
    }

    var roundElement = roundXML.Clone();

    foreach (var row in eventTable.GetRowsByTeam(teamUID)) {
        var newRoundXML = roundXML.Clone();
        newRoundXML.ApplyRowAsText(row);
        teamElement["rounds"][0].AddChild(newRoundXML);
    }

    var summaryRow = summaryTable.GetRowsByTeam(teamUID)[0];
    var summaryXML = roundXML.Clone();
    summaryXML.ApplyRowAsText(summaryRow);
    summaryXML["#round"][0].InnerText = "\u03A3";
    teamElement["rounds"][0].AddChild(summaryXML);

    documentXML.Update();
    Debug.WriteLine(documentXML.OuterRect.ToString());
}
