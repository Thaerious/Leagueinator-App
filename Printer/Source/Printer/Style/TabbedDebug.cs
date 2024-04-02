using System.Diagnostics;

namespace Leagueinator.Printer.Styles {
    public static class TabbedDebug {
        private static int indent = 0;

        public static void StartBlock(string s = "") {
            Debug.WriteLine($"{new string('\t', indent)}{s}");
            indent++;
        }

        public static void NextBlock(string s = "") {
            if (indent > 0) indent--;
            Debug.WriteLine($"{new string('\t', indent)}{s}");
            indent++;
        }

        public static void EndBlock(string s = "") {
            if (indent > 0) indent--;
            Debug.WriteLine($"{new string('\t', indent)}{s}");
        }

        public static void WriteLine(string s) {
            Debug.WriteLine($"{new string('\t', indent)}{s}");
        }
    }
}
