using Leagueinator.CSSParser;
using Leagueinator.Printer;
using System.Diagnostics;

string source = "45px 30% 15px 0px";

bool r1 = MultiParse.TryParse("45px", typeof(Func<float, float?>), out object? isFloat);
Debug.WriteLine($"{r1} {isFloat != null}");

bool r2 = MultiParse.TryParse(source, typeof(Cardinal<Func<float, float?>>), out object? result);
var asCardinal = (Cardinal<Func<float, float?>>)result;

Debug.WriteLine($"{r2} {result != null}");
Debug.WriteLine($"{asCardinal.Top(100)} {asCardinal.Right(100)} {asCardinal.Bottom(100)} {asCardinal.Left(100)}");
