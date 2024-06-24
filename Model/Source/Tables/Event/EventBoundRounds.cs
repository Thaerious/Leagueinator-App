using Leagueinator.Model.Views;

namespace Leagueinator.Model.Tables {
    public class EventBoundRounds(RoundTable childTable, EventRow eventRow)
        : ARowBoundView<RoundTable, EventRow, RoundRow>(childTable, eventRow) {

        public RoundRow Add() => this.ChildTable.AddRow(eventRow.UID);
    }
}
