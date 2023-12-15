using Model.Tables;
using System.Data;
using Leagueinator.Utility;
using System.Collections.ObjectModel;

namespace Model {
    public class League : DataSet {

        public DataTable EventTable => this.Tables[Model.Tables.EventTable.TABLE_NAME]!;
        public DataTable TeamTable => this.Tables[Model.Tables.TeamTable.TABLE_NAME]!;
        public DataTable EventDirectoryTable => this.Tables[Model.Tables.EventDirectoryTable.TABLE_NAME]!;
        public DataTable RoundDirectoryTable => this.Tables[Model.Tables.RoundDirectoryTable.TABLE_NAME]!;

        public League() {
            Tables.Add(Model.Tables.EventTable.MakeTable());
            Tables.Add(Model.Tables.TeamTable.MakeTable());
            Tables.Add(Model.Tables.EventDirectoryTable.MakeTable());
            Tables.Add(Model.Tables.RoundDirectoryTable.MakeTable());
        }

        public ReadOnlyDictionary<string, LeagueEvent> LeagueEvents {
            get {
                var dictionary = EventDirectoryTable
                .ColumnValues<string>(Model.Tables.EventDirectoryTable.NAME_COL)
                    .NotNull()
                    .ToDictionary(
                        eventName => eventName,
                        eventName => new LeagueEvent(this, eventName)
                    );

                return new ReadOnlyDictionary<string, LeagueEvent>(dictionary);
            }
        }

        public LeagueEvent AddLeagueEvent(string eventName) {
            var row = EventDirectoryTable.NewRow();

            row[Model.Tables.EventDirectoryTable.NAME_COL] = eventName;
            row[Model.Tables.EventDirectoryTable.DATE_COL] = DateTime.Today.ToString("yyyy-MM-dd");

            this.Tables[Model.Tables.EventDirectoryTable.TABLE_NAME]!.Rows.Add(row);

            return new LeagueEvent(this, eventName) {
                RowFilter = $"event_name = '{eventName}'"
            };
        }
    }
}
