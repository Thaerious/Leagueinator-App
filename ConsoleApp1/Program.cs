
using Antlr4.Runtime.Tree;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Leagueinator.CSSParser;
using System.Drawing;
using System.Diagnostics;

string test = @"
div {
    zindex: 15;
    z-index: 15;
    width: 9;
}

#red {
    background-color: Red;
}

#red {
    color : Red ;
}
";

var styles = StyleLoader.Load(test);
foreach (var style in styles) {
    Console.WriteLine(style.Value);
}


var target = new Target();
styles["#red"].ApplyTo(target);
Console.WriteLine("RESULT " + target.color);

Console.ReadKey();

class Target {
    [CSS] public Color color;
}
