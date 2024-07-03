using System.Reflection;

namespace Utility.Source.TimeTraceAspect {

    public class MemberCallRecord {
        public MemberCallRecord(MemberInfo memberInfo) {
            this.MemberRecord = memberInfo;
        }

        public readonly MemberInfo MemberRecord;

        public int CallCount { get; private set; } = 0;

        public long TotalTime { get; private set; } = 0;

        public void AddCall(long time) {
            this.CallCount++;
            this.TotalTime += time;
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
        internal void AddCallee(MemberInfo memberInfo, long time) {
            if (!CalleeMap.ContainsKey(memberInfo)) CalleeMap[memberInfo] = new(memberInfo);
            CalleeMap[memberInfo].AddCall(time);
        }

        /// <summary>
        /// Add a call from another method to this.
        /// </summary>
        /// <param name="memberRecord">The member record doing the calling</param>
        /// <param name="count">The total times this call took place</param>
        /// <param name="time">The total time spent in the call</param>
        /// <exception cref="NotImplementedException"></exception>
        internal void AddCaller(MemberInfo memberInfo, long time) {
            if (!CallerMap.ContainsKey(memberInfo)) CallerMap[memberInfo] = new(memberInfo);
            CallerMap[memberInfo].AddCall(time);
        }

        internal void AddSelf(long time) {
            Self.AddCall(time);
        }
    }
}
