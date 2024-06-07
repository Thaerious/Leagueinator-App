using System.Text;
using System.Xml.Linq;
using AspectInjector.Broker;

namespace Leagueinator.Utility {

    class TimeTraceRec(string methodName, Type type) {
        public long Start = DateTime.Now.Ticks;
        public long End = 0;
        public readonly Type Type = type;
        public readonly string MethodName = methodName;
        public Stack<TimeTraceRec> Calls = [];

        public double Duration => End - Start;

        public override string ToString() {            
            return $"[{this.Duration/10000}ms] {Type.Name}.{MethodName}";
        }
    }

    class MarkRec(string methodName, Type type) : TimeTraceRec(methodName, type) {}

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    [Injection(typeof(TimeTraceAspect))]
    public class TimeTraceAttribute : Attribute {
        public TimeTraceAttribute() {}
    }

    [Aspect(Scope.Global)]
    [Injection(typeof(TimeTraceAspect))]
    public class TimeTraceAspect : Attribute {

        [Advice(Kind.Before, Targets = Target.Any)]
        public void Enter([Argument(Source.Type)] Type type, [Argument(Source.Name)] string name) {
            TimeTrace.Enter(type, name);
        }

        [Advice(Kind.After, Targets = Target.Any)]
        public void Exit() {
            TimeTrace.Exit();
        }
    }

    public class MethodRecord(string className) {
        public readonly string ClassName = className;
        public int CallCount { get; internal set; } = 0;
        public double SelfTime { get; internal set; } = 0;
        public double TotalTime { get; internal set; } = 0;

        public Report Calls = [];

        internal void AddInstance(TimeTraceRec ttRecord) {
            this.CallCount++;
            this.TotalTime = this.TotalTime + ttRecord.Duration;

            foreach (TimeTraceRec ttCallRecord in ttRecord.Calls) {
                this.Calls.Add(ttCallRecord);
            }
        }
    }

    public class ClassRecord : Dictionary<string, MethodRecord> {
        internal void Add(TimeTraceRec ttRecord) {
            string methodName = ttRecord.MethodName;            
            if (!this.ContainsKey(methodName)) this[methodName] = new(ttRecord.Type.Name);
            this[methodName].AddInstance(ttRecord);
        }
    }

    public class Report : Dictionary<string, ClassRecord> {
        internal void Add(TimeTraceRec ttRecord ) {
            if (!this.ContainsKey(ttRecord.Type.Name)) this[ttRecord.Type.Name] = new();
            ClassRecord classRecord = this[ttRecord.Type.Name];
            classRecord.Add(ttRecord);
        }

        public override string ToString() {
            StringBuilder sb = new();

            foreach (string className in this.Keys) {
                ClassRecord classRecord = this[className];
                sb.AppendLine($"{className}");

                foreach (string methodName in classRecord.Keys) {
                    MethodRecord methodRecord = classRecord[methodName];
                    string totalTime = (methodRecord.TotalTime / 10000).ToString("F1");
                    string avgTime = (methodRecord.TotalTime / 10000 / methodRecord.CallCount).ToString("F1");
                    sb.AppendLine($"|-- {methodName} {methodRecord.CallCount} x {avgTime}ms = {totalTime}ms ");
                    this.AppendCallsToString(sb, methodRecord.Calls);                    
                }
                sb.AppendLine($"");
            }

            return sb.ToString();
        }

        private void AppendCallsToString(StringBuilder sb, Report calls) {
            foreach (string className in calls.Keys) {
                ClassRecord classRecord = calls[className];

                foreach (string methodName in classRecord.Keys) {
                    MethodRecord methodRecord = classRecord[methodName];
                    string totalTime = (methodRecord.TotalTime / 10000).ToString("F1");
                    string avgTime = (methodRecord.TotalTime / 10000 / methodRecord.CallCount).ToString("F1");
                    sb.AppendLine($"    |-- {className}.{methodName} {methodRecord.CallCount} x {avgTime}ms = {totalTime}ms");
                }
            }
        }
    }

    public static class TimeTrace {
        internal static Stack<TimeTraceRec> Records = [];
        internal static Stack<TimeTraceRec> History = [];

        public static Report Report() {
            Report report = new();

            foreach (TimeTraceRec record in History) {
                report.Add(record);
            }

            return report;
        }

        public static void Mark(Type type, string annotation) {
            if (TimeTrace.Records.Peek() is MarkRec) {
                TimeTraceRec mark = TimeTrace.Records.Pop();
                mark.End = DateTime.Now.Ticks;
                TimeTrace.History.Push(mark);
            }

            MarkRec nextCallRec = new(annotation, type);
            if (TimeTrace.Records.Count > 0) TimeTrace.Records.Peek().Calls.Push(nextCallRec);
            TimeTrace.Records.Push(nextCallRec);
        }

        internal static void Enter(Type type, string name) {
            TimeTraceRec nextCallRec = new(name, type);
            if (TimeTrace.Records.Count > 0) TimeTrace.Records.Peek().Calls.Push(nextCallRec);
            TimeTrace.Records.Push(nextCallRec);
        }

        internal static void Exit() {
            if (TimeTrace.Records.Peek() is MarkRec) {
                TimeTraceRec mark = TimeTrace.Records.Pop();
                mark.End = DateTime.Now.Ticks;
                TimeTrace.History.Push(mark);
            }

            TimeTraceRec record = TimeTrace.Records.Pop();
            record.End = DateTime.Now.Ticks;
            TimeTrace.History.Push(record);
        }
    }
}
