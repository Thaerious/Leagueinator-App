using Leagueinator.Model.Views;

namespace Leagueinator.Model.Tables {
    public class EventBoundRounds(EventRow eventRow)
        : ARowBoundView<RoundTable, EventRow, RoundRow>(eventRow.League.RoundTable, eventRow) {

        public RoundRow Add() => this.ChildTable.AddRow(eventRow.UID);
    }
}
