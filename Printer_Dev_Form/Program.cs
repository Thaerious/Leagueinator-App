using Model;
using Printer_Dev_Form;

League league = new Mock();
EventPrinter eventPrinter = new(league.LeagueEvents[0]);

ApplicationConfiguration.Initialize();
var form = new Form1();
form.canvas.RootElement = eventPrinter.documentXML;
Application.Run(form);
