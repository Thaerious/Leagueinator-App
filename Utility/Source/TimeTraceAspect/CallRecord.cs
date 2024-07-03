using System.Reflection;

namespace Utility.Source.TimeTraceAspect {
    public class CallRecord(MemberInfo MemberInfo) {
        public readonly MemberInfo MemberInfo = MemberInfo;
        public readonly long EntryTime = DateTime.Now.Ticks;
    }
}
