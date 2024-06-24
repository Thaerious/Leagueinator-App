using AspectInjector.Broker;
using System.Diagnostics;

namespace Leagueinator.Printer.Utility {

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    [Injection(typeof(EnableTabbedDebugAspect))]
    public class EnableTabbedDebugAttribute : Attribute {
        public bool Enabled = true;
        public EnableTabbedDebugAttribute(bool enabled) {
            this.Enabled = enabled;
        }
    }

    [Aspect(Scope.Global)]
    [Injection(typeof(EnableTabbedDebugAspect))]
    public class EnableTabbedDebugAspect : Attribute {
        [Advice(Kind.Before, Targets = Target.Any)]
        public void Enter([Argument(Source.Triggers)] Attribute[] attrs) {
            var attribute = (EnableTabbedDebugAttribute)attrs[0];
            TabbedDebug.Enabled = attribute.Enabled;
        }
    }
}


public static class TabbedDebug {
    private static int indent = 0;
    public static bool Enabled = true;

    public static void ResetBlock(string s = "") {
        if (!Enabled) return;
        indent = 0;
        StartBlock(s);
    }

    public static void StartBlock(string s = "") {
        if (!Enabled) return;
        if (indent == 0) {
            Debug.WriteLine($"{s}");
            indent++;
            return;
        }
        var line = string.Concat(Enumerable.Repeat(":  ", indent));
        Debug.WriteLine($"{line}|--{s}");
        indent++;
    }

    public static void EndBlock() {
        if (!Enabled) return;
        if (indent > 0) indent--;
    }

    public static void WriteLine(object s) {
        if (!Enabled) return;
        var line = String.Concat(Enumerable.Repeat(":  ", indent));
        Debug.WriteLine($"{line}| {s}");
    }
}

