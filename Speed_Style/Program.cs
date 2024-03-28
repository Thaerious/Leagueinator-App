using SpeedStyle;
using System.Diagnostics;

var root = Loader.LoadResources("large.xml", "large.style");

Stopwatch stopwatch = Stopwatch.StartNew();
for (float i = 0; i < 100; i++){
    root.Style.DoLayout();
    Console.WriteLine($"{i} : {stopwatch.ElapsedMilliseconds / (i + 1f)} ms");
}
stopwatch.Stop();

Console.WriteLine($"{stopwatch.ElapsedMilliseconds / 100f} ms");
Console.ReadLine();
