using Leagueinator.Utility;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace Utility.Source.TimeTraceAspect {

    public static class ReportExt {
        /// <summary>
        /// Load a string from an embedded resource.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public static string LoadResource(this Assembly assembly, string path) {
            using Stream? stream = assembly.GetManifestResourceStream(path) ?? throw new NullReferenceException($"Resource Not Found: {path}");
            using StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        public static double ToMS(this long value) {
            double ms = value / TimeSpan.TicksPerMillisecond;
            return Math.Truncate(ms * 10) / 10;
        }
    }

    public class Report() {
        private Dictionary<Type, ClassRecord> ClassRecords = [];

        internal void AddCall(MemberInfo caller, MemberInfo callee, long time) {
            Debug.WriteLine($"AddCall {caller.DeclaringType.Name}::{caller.Name} {callee.DeclaringType.Name}::{callee.Name} {time.ToMS()}");

            MemberRecord callerRecord = this.MemberRecord(caller);
            MemberRecord calleeRecord = this.MemberRecord(callee);
            
            callerRecord.AddCallee(callee, time);
            calleeRecord.AddCaller(caller, time);            
            calleeRecord.Self.AddCall(time);

            Debug.WriteLine($"Callee elapsed time {calleeRecord.Self.TotalTime}");
        }
              

        private ClassRecord ClassRecord(CallRecord callRecord) {
            var declaringType = callRecord.MemberInfo.DeclaringType;
            if (!ClassRecords.ContainsKey(declaringType)) ClassRecords[declaringType] = new(declaringType);
            return ClassRecords[declaringType];
        }

        private ClassRecord ClassRecord(MemberInfo memberInfo) {
            var declaringType = memberInfo.DeclaringType!;
            if (!ClassRecords.ContainsKey(declaringType)) ClassRecords[declaringType] = new(declaringType);
            return ClassRecords[declaringType];
        }

        internal MemberRecord MemberRecord(CallRecord callRecord) {
            ClassRecord classRecord = this.ClassRecord(callRecord);
            return classRecord.MemberRecord(callRecord);
        }

        private MemberRecord MemberRecord(MemberInfo memberInfo) {
            ClassRecord classRecord = this.ClassRecord(memberInfo);
            return classRecord.MemberRecord(memberInfo);
        }

        public void WriteFiles(string rootDir) {
            TimeTrace.Terminate();
            StringBuilder sb = new();
            Directory.CreateDirectory(rootDir);
            
            sb.AppendLine(
                Assembly.GetExecutingAssembly().LoadResource("Utility.Source.TimeTraceAspect.Assets.ClassList.Prefix.html")
            );

            int i = 0;
            foreach (ClassRecord classRecord in this.ClassRecords.Values) {
                string className = classRecord.Type.Name;
                sb.AppendLine($"<li class=\"item\" onclick=\"toggleVisibility('sublist{i}')\">{className}");
                sb.AppendLine($"<ul id=\"sublist{i}\" class=\"sub-item\" onclick=\"event.stopPropagation();\">");

                int j = 0;
                foreach (MemberRecord memberRecord in classRecord.MemberRecords) {
                    string methodName = memberRecord.MemberInfo.Name;
                    sb.AppendLine($"<li><a href=\"{className}.{methodName}.html\">{methodName}</a></li>");

                    this.WriteFile(rootDir, memberRecord, $"{className}.{methodName}.html");

                    j++;
                }
                sb.AppendLine("</ul></li>");
                i++;
            }


            sb.AppendLine(
                Assembly.GetExecutingAssembly().LoadResource("Utility.Source.TimeTraceAspect.Assets.ClassList.Suffix.html")
            );

            File.WriteAllText($"{rootDir}/index.html", sb.ToString());
        }

        private void WriteFile(string rootDir, MemberRecord memberRecord, string fileName) {
            StringBuilder sb = new();

            if (memberRecord.MemberInfo.DeclaringType == typeof(User)) return;

            string className = memberRecord.MemberInfo.DeclaringType!.Name;
            string methodName = memberRecord.MemberInfo.Name;
            string selfName = $"{className}.{methodName}";

            string prefix = Assembly.GetExecutingAssembly().LoadResource("Utility.Source.TimeTraceAspect.Assets.Method.Prefix.html");
            var replacements = new Dictionary<string, string>{
                { "Title", selfName}
            };

            prefix = prefix.Interpolate(replacements);

            sb.AppendLine(prefix);

            foreach (MemberCallRecord record in memberRecord.Callers) {
                string callerClassName = record.MemberRecord.DeclaringType!.Name;
                string callerMethodName = record.MemberRecord.Name;
                string fullName = $"{callerClassName }.{callerMethodName}";

                sb.AppendLine(
                    $"<tr class='caller_row'>" +
                         $"<td><a href='{fullName}.html'>{fullName}</a></td>" +
                         $"<td> {record.CallCount} </td>" +
                         $"<td> {record.TotalTime.ToMS()} </td>" +
                         $"<td> {(record.TotalTime / record.CallCount).ToMS()} </td>" +
                         $"<td> Self Time </td>" +
                    $"</tr >"
                );
            }

            sb.AppendLine(
                $"<tr class='self'>" +
                     $"<td>{selfName}</td>" +
                     $"<td> {memberRecord.Self.CallCount} </td>" +
                     $"<td> {memberRecord.Self.TotalTime.ToMS()} </td>" +
                     $"<td> {(memberRecord.Self.TotalTime / memberRecord.Self.CallCount).ToMS()} </td>" +
                     $"<td> Self Time </td>" +
                $"</tr >"
            );

            foreach (MemberCallRecord record in memberRecord.Callees) {
                string calleeClassName = record.MemberRecord.DeclaringType!.Name;
                string calleeMethodName = record.MemberRecord.Name;
                string fullName = $"{calleeClassName}.{calleeMethodName}";

                sb.AppendLine(
                    $"<tr class='callee_row'>" +
                         $"<td><a href='{fullName}.html'>{fullName}</a></td>" +
                         $"<td> {record.CallCount} </td>" +
                         $"<td> {record.TotalTime.ToMS()} </td>" +
                         $"<td> {(record.TotalTime / record.CallCount).ToMS()} </td>" +
                         $"<td> Self Time </td>" +
                    $"</tr >"
                );
            }

            sb.AppendLine(
                Assembly.GetExecutingAssembly().LoadResource("Utility.Source.TimeTraceAspect.Assets.Method.Suffix.html")
            );

            File.WriteAllText($"{rootDir}/{fileName}", sb.ToString());
        }
    }
}
