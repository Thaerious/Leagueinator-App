using Leagueinator.Model.Tables;
using Leagueinator.Model.Views;

namespace Model.Source.Tables.Round {
    public class RoundBoundIdles(IdleTable childTable, RoundRow roundRow)
        : ARowBoundView<IdleTable, RoundRow, IdleRow>(childTable, roundRow) {

        public IdleRow Add(string name) => this.ChildTable.AddRow(roundRow.UID, name);

        public IdleRow Get(string name) => this.ChildTable.GetRow(roundRow.UID, name);

        public bool Has(string name) => this.ChildTable.HasRow(roundRow.UID, name);
    }
}
