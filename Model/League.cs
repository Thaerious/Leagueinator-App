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
                .ColumnValues<string>(Model.Tables.EventDirectoryTable.COL.NAME)
                    .NotNull()
                    .ToDictionary(
                        eventName => eventName,
                        eventName => new LeagueEvent(this, eventName)
                    );

                return new ReadOnlyDictionary<string, LeagueEvent>(dictionary);
            }
        }

        public LeagueEvent AddLeagueEvent(string eventName) {            
            AddToRoundDirectory(eventName);
            return AddToEventDirectory(eventName);
        }

        private LeagueEvent AddToEventDirectory(string eventName) {
            var row = this.EventDirectoryTable.NewRow();

            row[Model.Tables.EventDirectoryTable.COL.NAME] = eventName;
            row[Model.Tables.EventDirectoryTable.COL.DATE] = DateTime.Today.ToString("yyyy-MM-dd");

            this.EventDirectoryTable.Rows.Add(row);

            return new LeagueEvent(this, eventName) {
                RowFilter = $"event_name = '{eventName}'"
            };
        }

        private void AddToRoundDirectory(string eventName) {
            var row = RoundDirectoryTable.NewRow();

            row[Model.Tables.RoundDirectoryTable.COL.EVENT_NAME] = eventName;
            row[Model.Tables.RoundDirectoryTable.COL.ROUND_COUNT] = -1;

            this.RoundDirectoryTable.Rows.Add(row);
        }
    }
}
