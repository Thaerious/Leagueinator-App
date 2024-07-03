using Leagueinator.Utility;
using System.Diagnostics;

[TimeTrace]
class Program {
    static void Main() {
        FooBar();        
        TimeTrace.Report.WriteFiles("D:/scratch/web/trace/");
        OpenBrowser("D:/scratch/web/trace/index.html");
    }

    static void FooBar() {
        Foo foo = new ();
        Bar bar = new ();

        bar.Report();
        bar.Report();
        bar.Report();
        //foo.Recursive(10);
    }

    static void OpenBrowser(string url) {
        try {
            Process.Start(new ProcessStartInfo {
                FileName = url,
                UseShellExecute = true
            });
        }
        catch (Exception ex) {
            Console.WriteLine("Could not open browser: " + ex.Message);
        }
    }

    [TimeTrace]
    public class Foo {
        public void Recursive(int i) {
            if (i > 0) Recursive(i - 1);
        }
    }

    [TimeTrace]
    public class Bar {
        public void Report() {
            Thread.Sleep(1000);
        }
    }
}
