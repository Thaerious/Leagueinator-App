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

        public League() {
            Tables.Add(EventTable);
            Tables.Add(TeamTable);
            Tables.Add(EventDirectoryTable);
        }

        public List<LeagueEvent> LeagueEvents {
            get {
                return EventDirectoryTable
                .ColumnValues<string>(EventDirectoryTable.COL.EVENT_NAME)
                .NotNull()
                .Select(name => new LeagueEvent(this, name))
                .ToList();
            }
        }

        public LeagueEvent NewLeagueEvent(string eventName, string? date = null) {
            date ??= DateTime.Today.ToString("yyyy-MM-dd");
            this.EventDirectoryTable.AddRow(eventName, date);
            return new LeagueEvent(this, eventName);
        }

        public LeagueEvent GetLeagueEvent(string eventName) {
            return new LeagueEvent(this, eventName);
        }

        public string PrettyPrint() {
            return EventTable.PrettyPrint() + "\n" +
                TeamTable.PrettyPrint() + "\n" +
                EventDirectoryTable.PrettyPrint();
        }
    }
}
