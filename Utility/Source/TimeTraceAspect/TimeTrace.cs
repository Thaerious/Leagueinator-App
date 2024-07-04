using AspectInjector.Broker;
using System.Diagnostics;
using System.Reflection;
using Utility.Source.TimeTraceAspect;

namespace Leagueinator.Utility {
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    [Injection(typeof(TimeTraceAspect))]
    public class TimeTraceAttribute : Attribute {
        public TimeTraceAttribute() { }
    }

    [Aspect(Scope.Global)]
    [Injection(typeof(TimeTraceAspect))]
    public class TimeTraceAspect : Attribute {
        /// <summary>
        /// Invoked whenever a method annotated with [TimeTrace] is entered.
        /// Creates a CallRecord and adds it to the call tree.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="args"></param>
        /// <exception cref="NullReferenceException"></exception>
        [Advice(Kind.Before, Targets = Target.Any)]
        public void Enter([Argument(Source.Type)] Type type, [Argument(Source.Name)] string name, [Argument(Source.Arguments)] object[] args) {
            if (TimeTrace.Terminated) return;

            CallRecord caller = TimeTrace.Calls.Peek();
            MemberInfo? memberInfo = GetMemberInfo(type, name, args);
            if (memberInfo is null) return;

            TimeTrace.Calls.Push(new CallRecord(memberInfo));
            Debug.WriteLine($"Enter {caller.MemberInfo.DeclaringType.Name}::{caller.MemberInfo.Name} ==> {type.Name}::{name}");
        }

        [Advice(Kind.After, Targets = Target.Any)]
        public void Exit() {
            if (TimeTrace.Terminated) return;
            if (TimeTrace.Calls.Count <= 1) return;

            CallRecord callee = TimeTrace.Calls.Pop();            
            CallRecord caller = TimeTrace.Calls.Peek();

            long startTime = callee.EntryTime;
            long endTime = DateTime.Now.Ticks;
            long elapsedTime = endTime - startTime;

            TimeTrace.Report.AddCall(caller.MemberInfo, callee.MemberInfo, elapsedTime);
            Debug.WriteLine($"Exit  {callee.MemberInfo.DeclaringType.Name}::{callee.MemberInfo.Name} ");
        }

        private MemberInfo? GetMemberInfo(Type type, string name, object[] args) {
            MemberInfo? memberInfo = null;

            if (name.StartsWith(".")) {
                Type[] argTypes = args.Select(arg => arg.GetType()).ToArray();
                memberInfo = type.GetConstructor(argTypes);
            }
            
            if (memberInfo is null) {
                memberInfo = type.GetMethod(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            }

            if (memberInfo is null) {
                memberInfo = type.GetProperty(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            }

            //if (memberInfo is null) throw new NullReferenceException($"{type.Name}::{name}");
            return memberInfo;
        }
    }

    public static class TimeTrace {
        static public readonly long ProgramStartTime = DateTime.Now.Ticks;
        static public double Elapsed = Math.Round((double)(DateTime.Now.Ticks - ProgramStartTime) / 10000, 1);

        public static readonly Report Report = new();
        public static readonly Stack<CallRecord> Calls = [];

        internal static bool Terminated = false;

        static TimeTrace(){
            MemberInfo memberInfo = typeof(User).GetMethod("Action")!;
            Calls.Push(new CallRecord(memberInfo));
        }

        public static void Terminate() {
            Terminated = true;
            if (TimeTrace.Calls.Count <= 1) return;

            CallRecord callee = TimeTrace.Calls.Pop();
            CallRecord caller = TimeTrace.Calls.Peek();

            long startTime = callee.EntryTime;
            long endTime = DateTime.Now.Ticks;
            long elapsedTime = endTime - startTime;

            TimeTrace.Report.AddCall(caller.MemberInfo, callee.MemberInfo, elapsedTime);
        }
    }

    public class User{
        public void Action() {}
    };
}
