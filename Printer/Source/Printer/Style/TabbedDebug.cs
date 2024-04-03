using System.Diagnostics;

namespace Leagueinator.Printer.Styles {
    public static class TabbedDebug {
        private static int indent = 0;

        public static void StartBlock(string s = "") {
            if (indent == 0) {
                Debug.WriteLine($"{s}");
                indent++;
                return;
            }
            var line = string.Concat(Enumerable.Repeat(":  ", indent - 1));
            Debug.WriteLine($"{line}|--{s}");
            indent++;
        }

        public static void EndBlock() {
            if (indent > 0) indent--;
        }

        public static void WriteLine(string s) {
            var line = String.Concat(Enumerable.Repeat(":  ", indent - 1));
            Debug.WriteLine($"{line}| {s}");
        }
    }
}
