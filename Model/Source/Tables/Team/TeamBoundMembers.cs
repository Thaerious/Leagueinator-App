using Leagueinator.Model.Tables;
using System.Data;

namespace Model.Source.Tables.Team {
    public class TeamBoundMembers : DataView, IEnumerable<MemberRow> {

        internal TeamBoundMembers(TeamRow teamRow) : base(teamRow.League.MemberTable) {
            this.TeamRow = teamRow;
        }

        private MemberTable MemberTable => this.TeamRow.League.MemberTable;

        public MemberRow Add(string name) => this.MemberTable.AddRow(this.TeamRow.Match.UID, this.TeamRow.Index, name);

        public MemberRow Get(string name) => this.MemberTable.GetRow(this.TeamRow.Match.UID, this.TeamRow.Index, name);

        public bool Has(string name) => this.MemberTable.HasRow(this.TeamRow.Match.UID, this.TeamRow.Index, name);

        public new MemberRow this[int index] {
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

        private readonly TeamRow TeamRow;
    }
}
