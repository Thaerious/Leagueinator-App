using System.Reflection;

namespace Utility.Source.TimeTraceAspect {
    public class CallRecord
    {
        public readonly MemberInfo MemberInfo;

        private Dictionary<MemberInfo, CallRecord> calls = [];

        private long EntryTime = DateTime.Now.Ticks;

        private CallRecord? Caller = null;

        public int CallCount { get; private set; } = 0;
        public long TotalTime { get; private set; } = 0;

        public List<CallRecord> Calls { get {
                return [.. calls.Values];
            }
        }

        public CallRecord(MemberInfo MemberInfo) {
            this.MemberInfo = MemberInfo;
        }

        /// <summary>
        /// From this call record, call the method represented by memberInfo.
        /// This builds the call record tree.
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        public CallRecord Call(MemberInfo memberInfo) {
            if (!calls.ContainsKey(memberInfo)) calls[memberInfo] = new CallRecord(memberInfo);
            calls[memberInfo].Enter(this);
            return calls[memberInfo];
        }

        private void Enter(CallRecord? caller) {
            this.Caller = caller;
            this.EntryTime = DateTime.Now.Ticks;
            this.CallCount++;
        }

        internal CallRecord Exit() {
            this.TotalTime = DateTime.Now.Ticks - this.EntryTime;
            return this.Caller!;
        }
    }
}
