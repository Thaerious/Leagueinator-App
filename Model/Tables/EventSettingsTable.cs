using System.Data;

namespace Model.Tables {
    public class EventSettingsTable : DataTable {
        public static readonly string TABLE_NAME = "event_settings";

        public static class COL {
            public static readonly string UID = "uid";
            public static readonly string DIR_UID = "dir_uid";
            public static readonly string KEY = "key";
            public static readonly string VALUE = "value";
        }

        public EventSettingsTable() : base(TABLE_NAME) {
            MakeTable(this);
        }

        public DataRow SetValue(int eventUID, string key, string value) {
            var row = this.GetRow(eventUID, key);

            row[COL.DIR_UID] = eventUID;
            row[COL.KEY] = key;
            row[COL.VALUE] = value;

            if (!this.Rows.Contains(row[COL.UID])) this.Rows.Add(row);
            return row;
        }

        public DataRow GetRow(int eventUID, string key) {
            var rows = this.AsEnumerable()
                           .Where(row => row.Field<int>(COL.DIR_UID) == eventUID)
                           .Where(row => row.Field<string>(COL.KEY) == key)
                           .ToList();

            if (rows.Count == 0) return this.NewRow();
            return rows[0];
        }

        public string GetValue(int eventUID, string key) {
            var rows = this.AsEnumerable()
                           .Where(row => row.Field<int>(COL.DIR_UID) == eventUID)
                           .Where(row => row.Field<string>(COL.KEY) == key)
                           .ToList();

            if (rows.Count == 0) return "";
            return rows[0].Field<string>(COL.VALUE) ?? "";
        }

        public static EventSettingsTable MakeTable(EventSettingsTable? table = null) {
            table ??= new();

            table.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.UID,
                Unique = true,
                AutoIncrement = true
            });

            table.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.DIR_UID
            });

            table.Columns.Add(new DataColumn {
                DataType = typeof(string),
                ColumnName = COL.KEY
            });

            table.Columns.Add(new DataColumn {
                DataType = typeof(string),
                ColumnName = COL.VALUE
            });

            table.PrimaryKey = [table.Columns[COL.UID]!];

            return table;
        }
    }
}
