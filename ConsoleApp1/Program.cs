using DevPrint;
using Leagueinator.CSSParser;
using Leagueinator.Printer;
using Leagueinator.Utility;
using System.Diagnostics;

var mockEvent = new MockEvent();
mockEvent.ToDataSet();

var input = @"
<div class='root' id='du_root'>
    <div id='uid'/>
    <div id='round'/>
    <div id='lane'/>
    <div id='team'/>
    <div id='bowls'/>
    <div id='ends'/>
</div>
";

var eventTable = mockEvent.ToDataSet().Tables["event"];

var root = XMLLoader.Load(input, "");
Console.WriteLine(root.ToXML());
Console.WriteLine(eventTable.Rows[0].ItemArray.DelString());
root.ApplyRowAsText(eventTable.Rows[0]);
Console.WriteLine(root.ToXML());
Console.ReadKey();


