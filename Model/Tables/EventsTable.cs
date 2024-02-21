using Model.Views;
using System.Data;

namespace Model.Tables {

    public class EventRow : CustomRow {
        public readonly ReflectedRowList<RoundRow, RoundTable, int> Rounds;
        public readonly ReflectedRowTable<string, string> Settings;

        public EventRow(League league, DataRow row) : base(league, row) {
            ArgumentNullException.ThrowIfNull(this.League.RoundsTable.FKEvent);
            this.Rounds = new(this.League.RoundsTable.FKEvent, this.UID);

            var column
                = this.League.SettingsTable.Columns[SettingsTable.COL.EVENT]
                ?? throw new NullReferenceException("Column is null");

            this.Settings = new(this.League.SettingsTable, column, this.UID);
        }

        public int UID {
            get => (int)this.DataRow[EventsTable.COL.UID];
        }

        public static implicit operator int(EventRow eventRow) => eventRow.UID;

        public string Name {
            get => (string)this.DataRow[EventsTable.COL.NAME];
            set => this.DataRow[EventsTable.COL.NAME] = value;
        }

        public string Date {
            get => (string)this.DataRow[EventsTable.COL.DATE];
            set => this.DataRow[EventsTable.COL.DATE] = value;
        }
    }

    public class EventsTable(League league) : CustomTable(league, "events") {

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
            return new EventRow(this.League, row);
        }

        public EventRow GetRow(string eventName) {
            DataRow[] foundRows = this.Select($"{COL.NAME} = '{eventName}'");
            if (foundRows.Length == 0) throw new KeyNotFoundException($"{COL.NAME} == {eventName}");
            return new(this.League, foundRows[0]);
        }

        public EventRow GetRow(int eventUID) {
            DataRow[] foundRows = this.Select($"{COL.UID} = {eventUID}");
            if (foundRows.Length == 0) throw new KeyNotFoundException($"{COL.UID} == {eventUID}");
            return new(this.League, foundRows[0]);
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
