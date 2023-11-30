using Leagueinator.CSSParser;
using Leagueinator.Printer;
using System.Diagnostics;

Console.WriteLine(" ----- START -----");
var foobar = new FooBar();
var method = foobar.GetType().GetMethod("Report");

try {
    method.Invoke(foobar, null);
}
catch (Exception ex) {
    Console.WriteLine("CAUGHT: " + ex.ToString());
}
Console.WriteLine(" -----  END  -----");
Console.ReadKey();

class FooBar {
    public void Report() {
        Console.WriteLine("Before Exception");
        throw new Exception("Weeeeee I'm an exception!");
        Console.WriteLine("After Exception");
    }
}


