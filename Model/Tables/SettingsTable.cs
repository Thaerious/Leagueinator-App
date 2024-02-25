using System.Data;

namespace Model.Tables {
    public class SettingsTable() : LeagueTable<CustomRow>("settings") {
        public static class COL {
            public static readonly string EVENT = "event_uid";
            public static readonly string KEY = "key";
            public static readonly string VALUE = "value";
        }

        public DataRow SetValue(int eventUID, string key, string value) {
            var row = this.GetRow(eventUID, key);

            row[COL.EVENT] = eventUID;
            row[COL.KEY] = key;
            row[COL.VALUE] = value;

            return row;
        }

        public DataRow GetRow(int eventUID, string key) {
            var rows = this.AsEnumerable()
                           .Where(row => row.Field<int>(COL.EVENT) == eventUID)
                           .Where(row => row.Field<string>(COL.KEY) == key)
                           .ToList();

            if (rows.Count == 0) return this.NewRow();
            return rows[0];
        }

        public string GetValue(int eventUID, string key) {
            var rows = this.AsEnumerable()
                           .Where(row => row.Field<int>(COL.EVENT) == eventUID)
                           .Where(row => row.Field<string>(COL.KEY) == key)
                           .ToList();

            if (rows.Count == 0) return "";
            return rows[0].Field<string>(COL.VALUE) ?? "";
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
