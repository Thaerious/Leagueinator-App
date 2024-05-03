using Leagueinator.Model.Views;
using System.Collections.ObjectModel;
using System.Data;

namespace Leagueinator.Model.Tables {

    public class EventRow : CustomRow {
        public readonly RowBoundView<RoundRow> Rounds;
        public readonly ReflectedRowTable Settings;

        public EventRow(DataRow dataRow) : base(dataRow) {
            this.Rounds = new(this.League.RoundTable, [RoundTable.COL.EVENT], [this.UID]);

            var column
                = this.League.SettingsTable.Columns[SettingsTable.COL.EVENT]
                ?? throw new NullReferenceException("Column is null");

            this.Settings = new(this.League.SettingsTable, column, this.UID);
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
        public ReadOnlyDictionary<Team, IReadOnlyList<MatchView>> AllTeams() {
            Dictionary<Team, List<MatchView>> allTeams = [];

            foreach (TeamRow teamRow in this.Teams) {
                Team team = new(teamRow);
                if (team.Players.Count <= 0) continue;
                if (!allTeams.ContainsKey(team)) allTeams[team] = [];
                allTeams[team].Add(new(teamRow));
            }

            // Convert the dictionary to have IReadOnlyList<MatchView> as the values
            var readOnlyTeams = new Dictionary<Team, IReadOnlyList<MatchView>>();
            foreach (var pair in allTeams) {
                readOnlyTeams[pair.Key] = pair.Value.AsReadOnly();
            }

            return new ReadOnlyDictionary<Team, IReadOnlyList<MatchView>>(readOnlyTeams);
        }
    }

    public class EventTable() : LeagueTable<EventRow>("events") {

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

        public EventRow GetLast() {
            return new(this.Rows[this.Rows.Count - 1]);
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
                Unique = true,
            });

            this.Columns.Add(new DataColumn {
                DataType = typeof(string),
                ColumnName = COL.DATE,
            });

            this.PrimaryKey = [this.Columns[COL.UID]!];
        }
    }
}
