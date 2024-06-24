using Leagueinator.Model.Tables;
using Leagueinator.Model.Views;

namespace Model.Source.Tables.Team {
    public class TeamBoundMembers(TeamRow teamRow)
        : ARowBoundView<MemberTable, TeamRow, MemberRow>(teamRow.League.MemberTable, teamRow) {

        public MemberRow Add(string name) => this.ChildTable.AddRow(teamRow.Match.UID, teamRow.Index, name);

        public MemberRow Get(string name) => this.ChildTable.GetRow(teamRow.Match.UID, teamRow.Index, name);

        public bool Has(string name) => this.ChildTable.HasRow(teamRow.Match.UID, teamRow.Index, name);
    }
}
