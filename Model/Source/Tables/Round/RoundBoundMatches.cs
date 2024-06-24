using Leagueinator.Model.Tables;
using Leagueinator.Model.Views;

namespace Model.Source.Tables.Round {
    public class RoundBoundMatches(RoundRow roundRow)
        : ARowBoundView<MatchTable, RoundRow, MatchRow>(roundRow.League.MatchTable, roundRow) {

        public MatchRow Add(int lane, int ends) => this.ChildTable.AddRow(roundRow.UID, lane, ends);

        public MatchRow Get(int lane) => this.ChildTable.GetRow(roundRow.UID, lane);

        public bool Has(int lane) => this.ChildTable.HasRow(roundRow.UID, lane);
    }
}
