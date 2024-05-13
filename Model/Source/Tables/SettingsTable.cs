using Leagueinator.Utility;
using System.Data;
using System.Diagnostics;

namespace Leagueinator.Model.Tables {
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

    public class SettingsTable : LeagueTable<SettingRow> {
        public SettingsTable() : base("settings"){
            this.NewInstance = dataRow => new SettingRow(dataRow);
            GetInstance = args => this.GetRow((int)args[0], (string)args[1]);
            HasInstance = args => this.HasRow((int)args[0], (string)args[1]);
            AddInstance = args => {
                string value = "";
                if (args.Length > 2) value = (string)args[2];
                return this.AddRow((int)args[0], (string)args[1], value);
            };
        }

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
