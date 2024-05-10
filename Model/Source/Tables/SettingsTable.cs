using Leagueinator.Model.Views;
using System.Data;

namespace Leagueinator.Model.Tables {

    public class SettingsRowBoundView(LeagueTable<SettingRow> childTable, string[] fkCol, object[] fkVal)
        : IndexRowBoundView<SettingRow, string>(childTable, fkCol, fkVal) {

        public override SettingRow this[string key] {
            get {
                if (key == null) throw new IndexOutOfRangeException();
                if (!this.Has([key])) return this.Add(key, "");
                return this.Get([key])!;
            }
        }
    }

    public class SettingRow(DataRow dataRow) : CustomRow(dataRow) {
        public EventRow Event {
            get => this.League.EventTable.GetRow((int)this[SettingsTable.COL.EVENT]);
        }

        public string Key {
            get => (string)this[SettingsTable.COL.KEY];
        }

        public string Value {
            get => (string)this[SettingsTable.COL.VALUE];
            set => this[SettingsTable.COL.VALUE] = value;
        }
    }

    public class SettingsTable() : LeagueTable<SettingRow>("settings", dataRow => new SettingRow(dataRow)) {
        public static class COL {
            public static readonly string EVENT = "event_uid";
            public static readonly string KEY = "key";
            public static readonly string VALUE = "value";
        }

        public SettingRow AddRow(int eventUID, string key, string value) {
            var row = this.NewRow();

            row[COL.EVENT] = eventUID;
            row[COL.KEY] = key;
            row[COL.VALUE] = value;

            this.Rows.Add(row);
            return new(row);
        }

        public SettingRow GetRow(int eventUID, string key) {
            var rows = this.AsEnumerable<SettingRow>()
                           .Where(row => row.Event == eventUID)
                           .Where(row => row.Key == key)
                           .ToList();

            if (rows.Count == 0) throw new KeyNotFoundException();
            return new(rows[0]);
        }

        public bool HasRow(int eventUID, string key) {
            return this.AsEnumerable<SettingRow>()
                       .Where(row => row.Event == eventUID)
                       .Where(row => row.Key == key)
                       .Any();
        }

        public override void BuildColumns() {
            this.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.EVENT
            });

            this.Columns.Add(new DataColumn {
                DataType = typeof(string),
                ColumnName = COL.KEY
            });

            this.Columns.Add(new DataColumn {
                DataType = typeof(string),
                ColumnName = COL.VALUE
            });

            this.Columns[COL.KEY]!.ExtendedProperties.Add("dict", "key");
            this.Columns[COL.VALUE]!.ExtendedProperties.Add("dict", "value");
        }
    }
}
