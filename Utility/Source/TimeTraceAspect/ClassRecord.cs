using System.Reflection;

namespace Utility.Source.TimeTraceAspect {
    public class ClassRecord (Type type) {
        public readonly Type Type = type;

        private Dictionary<MemberInfo, MemberRecord> members = [];

        public MemberRecord[] MemberRecords => members.Values.ToArray();

        /// <summary>
        /// Add a member memberInfo to this class.
        /// </summary>
        /// <param name="record"></param>
        internal MemberRecord MemberRecord(CallRecord record) {
            if (!members.ContainsKey(record.MemberInfo)) {
                members[record.MemberInfo] = new(record.MemberInfo);
            }
            return members[record.MemberInfo];
        }

        /// <summary>
        /// Add a member memberInfo to this class.
        /// </summary>
        /// <param name="memberInfo"></param>
        internal MemberRecord MemberRecord(MemberInfo memberInfo) {
            if (!members.ContainsKey(memberInfo)) {
                members[memberInfo] = new(memberInfo);
            }
            return members[memberInfo];
        }
    }
}
