using Leagueinator.Model.Tables;
using Leagueinator.Model.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Source.Tables.Team {
    public class TeamBoundMembers(MemberTable childTable, TeamRow teamRow)
        : ARowBoundView<MemberTable, TeamRow, MemberRow>(childTable, teamRow) {

        public MemberRow Add(int index, string name) => this.ChildTable.AddRow(teamRow.Match.UID, index, name);

        public MemberRow Get(int index, string name) => this.ChildTable.GetRow(teamRow.Match.UID, index, name);

        public bool Has(int index, string name) => this.ChildTable.HasRow(teamRow.Match.UID, index, name);
    }
}
