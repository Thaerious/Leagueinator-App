using Leagueinator.Model.Views;
using System.Collections.ObjectModel;
using System.Data;

namespace Leagueinator.Model.Tables {
    public class EventRow : CustomRow {
        public readonly RowBoundView<RoundRow> Rounds;
        public readonly RowBoundView<SettingRow> Settings;

        public EventRow(DataRow dataRow) : base(dataRow) {
            this.Rounds = new(this.League.RoundTable, [RoundTable.COL.EVENT], [this.UID]);
            this.Settings = new(this.League.SettingsTable, [SettingsTable.COL.EVENT], [this.UID]);
        }

        /// <summary>
        /// Retreive a colloection of team rows for all rounds in this event.
        /// </summary>
        public IEnumerable<TeamRow> Teams {
            get {
                return this.Rounds.SelectMany(matchRow => matchRow.Teams);
            }
        }

        public int UID {
            get => (int)this[EventTable.COL.UID];
        }

        public static implicit operator int(EventRow eventRow) => eventRow.UID;

        public string Name {
            get => (string)this[EventTable.COL.NAME];
            set => this[EventTable.COL.NAME] = value;
        }

        public string Date {
            get => (string)this[EventTable.COL.DATE];
            set => this[EventTable.COL.DATE] = value;
        }
        
        /// <summary>
        /// Retrieve a list of each team with a unique set of players.
        /// </summary>
        /// <returns></returns>
        public ReadOnlyDictionary<Team, IReadOnlyList<MatchResults>> MatchResults() {
            Dictionary<Team, List<MatchResults>> allTeams = [];

            foreach (TeamRow teamRow in this.Teams) {
                Team team = new(teamRow);
                if (team.Players.Count <= 0) continue;
                if (!allTeams.ContainsKey(team)) allTeams[team] = [];
                allTeams[team].Add(new(teamRow));
            }

            // Convert the dictionary to have IReadOnlyList<MatchResults> as the values
            var readOnlyTeams = new Dictionary<Team, IReadOnlyList<MatchResults>>();
            foreach (var pair in allTeams) {
                readOnlyTeams[pair.Key] = pair.Value.AsReadOnly();
            }

            return new ReadOnlyDictionary<Team, IReadOnlyList<MatchResults>>(readOnlyTeams);
        }

        public List<MatchSummary> MatchSummaries() {
            List<MatchSummary> summaries = [];

            foreach (var pair in this.MatchResults()) {
                summaries.Add(new MatchSummary(pair.Key, pair.Value));
            }

            summaries.Sort();
            summaries.Reverse();
            return summaries;
        }
    }

    public class EventTable : LeagueTable<EventRow> {
        public EventTable() : base("events"){
            this.NewInstance = dataRow => new EventRow(dataRow);
            GetInstance = args => this.GetRow((string)args[0]);
            HasInstance = args => this.HasRow((string)args[0]);
            AddInstance = args => this.AddRow((string)args[0]);
        }

        public static class COL {
            public static readonly string UID = "uid";
            public static readonly string NAME = "name";
            public static readonly string DATE = "date";
        }

        public EventRow AddRow(string eventName, string? date = null) {
            date ??= DateTime.Today.ToString("yyyy-MM-dd");
            var row = this.NewRow();

            row[COL.NAME] = eventName;
            row[COL.DATE] = date;
            this.Rows.Add(row);

            return new EventRow(row);
        }

        public EventRow GetRow(string eventName) {
            DataRow[] foundRows = this.Select($"{COL.NAME} = '{eventName}'");
            if (foundRows.Length == 0) throw new KeyNotFoundException($"{COL.NAME} == {eventName}");
            return new EventRow(foundRows[0]);
        }

        public EventRow GetRow(int eventUID) {
            DataRow[] foundRows = this.Select($"{COL.UID} = {eventUID}");
            if (foundRows.Length == 0) throw new KeyNotFoundException($"{COL.UID} == {eventUID}");
            return new EventRow(foundRows[0]);
        }

        public bool HasRow(string eventName) {
            DataRow[] foundRows = this.Select($"{COL.NAME} = '{eventName}'");
            return foundRows.Length > 0;
        }

        public EventRow GetLast() {
            return new(this.Rows[^1]);
        }

        public override void BuildColumns() {
            this.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.UID,
                Unique = true,
                AutoIncrement = true
            });

            this.Columns.Add(new DataColumn {
                DataType = typeof(string),
                ColumnName = COL.NAME,
            });

            this.Columns.Add(new DataColumn {
                DataType = typeof(string),
                ColumnName = COL.DATE,
            });

            this.PrimaryKey = [this.Columns[COL.UID]!];
        }
    }
}
