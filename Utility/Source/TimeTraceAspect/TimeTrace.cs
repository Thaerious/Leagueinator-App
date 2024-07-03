using AspectInjector.Broker;
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
            MemberInfo? memberInfo = null;

            if (name is ".ctor") {
                Type[] argTypes = args.Select(arg => arg.GetType()).ToArray();
                memberInfo = type.GetConstructor(argTypes);
            }
            else {
                memberInfo = type.GetMethod(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);                
            }

            if (memberInfo is null) throw new NullReferenceException(name);

            if (TimeTrace.CurrentRecord is null) {
                TimeTrace.Root = new(memberInfo);
                TimeTrace.CurrentRecord = TimeTrace.Root;
            }
            else {
                TimeTrace.CurrentRecord = TimeTrace.CurrentRecord.Call(memberInfo);
            }           
        }

        [Advice(Kind.After, Targets = Target.Any)]
        public void Exit() {
            TimeTrace.CurrentRecord = TimeTrace.CurrentRecord!.Exit();
        }
    }

    public static class TimeTrace {
        public static CallRecord? Root;
        internal static CallRecord? CurrentRecord;
    }
}
