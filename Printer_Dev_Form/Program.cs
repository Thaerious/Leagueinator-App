using Leagueinator.Printer;
using Printer_Dev_Form;
using System.Diagnostics;
using System.Reflection;

Assembly assembly = Assembly.GetExecutingAssembly();

string xmlResource = "Printer_Dev_Form.assets.layout.xml";
string ssResource = "Printer_Dev_Form.assets.style.ss";
string templResource = "Printer_Dev_Form.assets.template.xml";

using Stream? xmlStream = assembly.GetManifestResourceStream(xmlResource) ?? throw new NullReferenceException();
using StreamReader xmlReader = new StreamReader(xmlStream);

using Stream? ssStream = assembly.GetManifestResourceStream(ssResource) ?? throw new NullReferenceException();
using StreamReader ssReader = new StreamReader(ssStream);

using Stream? templStream = assembly.GetManifestResourceStream(templResource) ?? throw new NullReferenceException();
using StreamReader templReader = new StreamReader(templStream);

string xmlString = xmlReader.ReadToEnd();
string ssString = ssReader.ReadToEnd();
string templString = templReader.ReadToEnd();

var document = XMLLoader.Load(xmlString, ssString);
var template = XMLLoader.Load(templString, ssString);

ApplicationConfiguration.Initialize();
var form = new Form1();

form.canvas.DocElement.AddChild(document);
Debug.WriteLine(form.canvas.DocElement.ToXML());

Application.Run(form);
