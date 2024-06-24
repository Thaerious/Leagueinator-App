using Leagueinator.Model.Tables;
using Leagueinator.Model.Views;

namespace Model.Source.Tables.Round {
    public class MatchBoundTeams(TeamTable childTable, MatchRow matchRow)
        : ARowBoundView<TeamTable, MatchRow, TeamRow>(childTable, matchRow) {

        public TeamRow Add(int index) => this.ChildTable.AddRow(matchRow.UID, index);

        public TeamRow Get(int index) => this.ChildTable.GetRow(matchRow.UID, index);

        public bool Has(int index) => this.ChildTable.HasRow(matchRow.UID, index);
    }
}
