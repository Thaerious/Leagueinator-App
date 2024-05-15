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
