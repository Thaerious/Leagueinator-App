using System.Reflection;

namespace Utility.Source.TimeTraceAspect {

    public class MemberCallRecord {
        public MemberCallRecord(MemberInfo memberInfo) {
            this.MemberRecord = memberInfo;
        }

        public readonly MemberInfo MemberRecord;

        public int CallCount { get; private set; } = 0;

        public long TotalTime { get; private set; } = 0;

        public void AddCall(CallRecord callRecord) {
            this.CallCount += callRecord.CallCount;
            this.TotalTime += callRecord.TotalTime;
        }
    }

    public class MemberRecord(MemberInfo memberInfo) {
        public readonly MemberInfo MemberInfo = memberInfo;
        public readonly MemberCallRecord Self = new(memberInfo);

        private Dictionary<MemberInfo, MemberCallRecord> CalleeMap = [];
        private Dictionary<MemberInfo, MemberCallRecord> CallerMap = [];


        public List<MemberCallRecord> Callees => CalleeMap.Values.ToList();
        public List<MemberCallRecord> Callers => CallerMap.Values.ToList();

        /// <summary>
        /// Add a call from this method to another.
        /// </summary>
        /// <param name="memberRecord">The member record doing the calling</param>
        /// <param name="count">The total times this call took place</param>
        /// <param name="time">The total time spent in the call</param>
        /// <exception cref="NotImplementedException"></exception>
        internal void AddCallee(CallRecord callRecord) {
            MemberInfo memberInfo = callRecord.MemberInfo;
            if (!CalleeMap.ContainsKey(memberInfo)) CalleeMap[memberInfo] = new(memberInfo);
            CalleeMap[memberInfo].AddCall(callRecord);
        }

        /// <summary>
        /// Add a call from another method to this.
        /// </summary>
        /// <param name="memberRecord">The member record doing the calling</param>
        /// <param name="count">The total times this call took place</param>
        /// <param name="time">The total time spent in the call</param>
        /// <exception cref="NotImplementedException"></exception>
        internal void AddCaller(CallRecord callRecord) {
            MemberInfo memberInfo = callRecord.MemberInfo;
            if (!CallerMap.ContainsKey(memberInfo)) CallerMap[memberInfo] = new(memberInfo);
            CallerMap[memberInfo].AddCall(callRecord);
        }

        internal void AddSelf(CallRecord callRecord) {
            Self.AddCall(callRecord);
        }
    }
}
