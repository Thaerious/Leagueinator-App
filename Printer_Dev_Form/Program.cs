using Leagueinator.Model;
using Leagueinator.Printer;
using Leagueinator_Model.Model.Tables;
using Printer_Dev_Form;
using System.Diagnostics;
using System.Reflection;

Assembly assembly = Assembly.GetExecutingAssembly();

var documentXML = assembly.LoadResource(
    "Printer_Dev_Form.assets.document.xml", 
    "Printer_Dev_Form.assets.style.css"
);
var teamXML = assembly.LoadResource(
    "Printer_Dev_Form.assets.team.xml", 
    "Printer_Dev_Form.assets.style.css"
);
var roundXML = assembly.LoadResource(
    "Printer_Dev_Form.assets.round.xml",
    "Printer_Dev_Form.assets.style.css"
);

ApplicationConfiguration.Initialize();
var form = new Form1();
form.canvas.DocElement.AddChild(documentXML);
ApplyData(documentXML, new MockEvent());

Debug.WriteLine(form.canvas.DocElement.ToXML());

Application.Run(form);

void ApplyData(PrinterElement docroot, LeagueEvent lEvent) {
    var teamElement = docroot.AddChild(teamXML.Clone());
    var roundElement = teamXML["rounds"][0].AddChild(roundXML.Clone());

    var eventData = lEvent.ToDataSet();
    var teamTable = new TeamTable(eventData);

    foreach (int id in teamTable.AllIDs()) {
        foreach (var name in teamTable.GetNames(id)) {
            var player = new PrinterElement("player") {
                InnerText = name
            };
            teamElement["players"][0].AddChild(player);
        }
    }
}
