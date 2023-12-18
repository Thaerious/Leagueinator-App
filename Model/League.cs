using Model.Tables;
using System.Data;
using Leagueinator.Utility;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Model {
    public class League : DataSet {

        public EventTable EventTable { get; } = new();
        public TeamTable TeamTable { get; } = new();
        public EventDirectoryTable EventDirectoryTable { get; } = new();
        public RoundDirectoryTable RoundDirectoryTable { get; } = new();

        public League() {
            Tables.Add(EventTable);
            Tables.Add(TeamTable);
            Tables.Add(EventDirectoryTable);
            Tables.Add(RoundDirectoryTable);
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

        public string PrettyPrint() {
            return EventTable.PrettyPrint() + "\n" +
                TeamTable.PrettyPrint() + "\n" +
                RoundDirectoryTable.PrettyPrint() + "\n" +
                EventDirectoryTable.PrettyPrint();
        }
    }
}
