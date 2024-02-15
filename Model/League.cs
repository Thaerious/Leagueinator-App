using Model.Tables;
using System.Data;
using Leagueinator.Utility;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Model {
    public class League : DataSet {

        public RoundTable RoundTable { get; } = new();
        public TeamTable TeamTable { get; } = new();
        public IdleTable IdleTable { get; } = new();
        public EventDirectoryTable EventDirectoryTable { get; } = new();
        public EventSettingsTable EventSettings { get; } = new();

        public League() {
            Tables.Add(RoundTable);
            Tables.Add(TeamTable);
            Tables.Add(EventDirectoryTable);
            Tables.Add(IdleTable);
            Tables.Add(EventSettings);            
        }

        public List<LeagueEvent> LeagueEvents {
            get {
                return EventDirectoryTable
                .ColumnValues<int>(EventDirectoryTable.COL.UID)
                .NotNull()
                .Select(uid => new LeagueEvent(this, uid))
                .ToList();
            }
        }

        public LeagueEvent NewLeagueEvent(string eventName, string? date = null) {
            date ??= DateTime.Today.ToString("yyyy-MM-dd");
            var row = this.EventDirectoryTable.AddRow(eventName, date);
            return new LeagueEvent(this, (int)row[EventDirectoryTable.COL.UID]);
        }

        public LeagueEvent GetLeagueEvent(int uid) {
            return new LeagueEvent(this, uid);
        }

        public string PrettyPrint() {
            return RoundTable.PrettyPrint() + "\n" +
                TeamTable.PrettyPrint() + "\n" +
                IdleTable.PrettyPrint() + "\n" +
                EventDirectoryTable.PrettyPrint() + "\n" +
                EventSettings.PrettyPrint();
        }
    }
}
