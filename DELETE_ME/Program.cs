using Leagueinator.Model;
using Leagueinator.Printer;
using Leagueinator.Utility;
using Leagueinator_Model.Model.Tables;
using System.Diagnostics;
using System.Reflection;

Assembly assembly = Assembly.GetExecutingAssembly();

var xmlLoader = new XMLLoader();
xmlLoader.LoadStyleResource(assembly, "DELETE_ME.assets.style.css");

var documentXML = xmlLoader.LoadXMLResource(assembly, "DELETE_ME.assets.document.xml");
var teamXML = xmlLoader.LoadXMLResource(assembly, "DELETE_ME.assets.team.xml");
var roundXML = xmlLoader.LoadXMLResource(assembly, "DELETE_ME.assets.round.xml");

ApplicationConfiguration.Initialize();
var form = new Form1();
form.canvas.Root = documentXML;

var eventData = new MockEvent().ToDataSet();
var teamTable = new TeamTable(eventData);
var eventTable = new EventTable(eventData);
var summaryTable = new SummaryTable(eventData);

Debug.WriteLine(eventTable);
Debug.WriteLine(teamTable);
Debug.WriteLine(summaryTable);

var teamElement = documentXML.AddChild(teamXML.Clone());
var roundElement = roundXML.Clone();

for (int i = 0; i < 2; i++) {
    teamElement["rounds"][0].AddChild(roundXML.Clone());
}

Application.Run(form);
