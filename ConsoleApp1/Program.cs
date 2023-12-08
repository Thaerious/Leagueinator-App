using DevPrint;
using Leagueinator.Printer;
using Leagueinator.Utility;


var type = typeof(DaysOfWeek);

Console.WriteLine(Enum.GetNames(type).DelString());
Console.WriteLine(((DaysOfWeek[])type.GetEnumValues())[0]);
Console.WriteLine((int)DaysOfWeek.Monday);
Console.ReadKey();

enum DaysOfWeek {
    Sunday = 2,
    Monday = 3,
    Tuesday = 4,
    Wednesday = 5,
    Thursday = 6,
    Friday = 7,
    Saturday = 8
}

//var mockEvent = new MockEvent();
//mockEvent.ToDataSet();

//var input = @"
//<div class='root' id='du_root'>
//    <div id='uid'/>
//    <div id='round'/>
//    <div id='lane'/>
//    <div id='team'/>
//    <div id='bowls'/>
//    <div id='ends'/>
//</div>
//";

//var eventTable = mockEvent.ToDataSet().Tables["event"];

//var root = XMLLoader.LoadFromString(input, "");
//Console.WriteLine(root.ToXML());
//Console.WriteLine(eventTable.Rows[0].ItemArray.DelString());
//root.ApplyRowAsText(eventTable.Rows[0]);
//Console.WriteLine(root.ToXML());


