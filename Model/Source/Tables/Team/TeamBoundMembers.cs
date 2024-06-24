using Leagueinator.Model.Tables;
using System.Data;

namespace Model.Source.Tables.Team {
    public class TeamBoundMembers(TeamRow teamRow) : DataView(teamRow.League.MemberTable), IEnumerable<MemberRow> {

        private MemberTable MemberTable { get; } = teamRow.League.MemberTable;

        public MemberRow Add(string name) => this.MemberTable.AddRow(teamRow.Match.UID, teamRow.Index, name);

        public MemberRow Get(string name) => this.MemberTable.GetRow(teamRow.Match.UID, teamRow.Index, name);

        public bool Has(string name) => this.MemberTable.HasRow(teamRow.Match.UID, teamRow.Index, name);

        public new MemberRow? this[int index] {
            get => new(base[index].Row);
        }

        /// <summary>
        /// Enumerator for child rows.
        /// </summary>
        /// <returns></returns>
        public new IEnumerator<MemberRow> GetEnumerator() {
            for (int i = 0; i < this.Count; i++) {
                if (this[i] is not null) yield return this[i]!;
            }
        }
    }
}
