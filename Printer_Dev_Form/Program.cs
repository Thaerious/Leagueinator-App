using Leagueinator.Model;
using Leagueinator.Printer;
using Leagueinator.Utility;
using Leagueinator_Model.Model.Tables;
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
form.canvas.DocElement.AddChild(documentXML);


var eventData = new MockEvent().ToDataSet();
var teamTable = new TeamTable(eventData);
var eventTable = new EventTable(eventData);

Debug.WriteLine(eventTable);
Debug.WriteLine(teamTable);
ApplyData();
//Debug.WriteLine(documentXML.ToXML());

Application.Run(form);

void ApplyData() {
    foreach (int teamUID in teamTable.AllIDs()) InsertTeam(teamUID);
    xmlLoader.ApplyStyles(documentXML);
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
    var rows = eventTable.GetRowsByTeam(teamUID);

    roundElement.ApplyRowAsText(rows[0]);
    teamElement["rounds"][0].AddChild(roundElement);
}
