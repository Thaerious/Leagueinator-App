using Leagueinator.Utility;
using System.Data;

namespace Model.Tables {
    public class EventDirectoryTable : DataTable {
        public static readonly string TABLE_NAME = "event_directory";

        public static class COL {
            public static readonly string UID = "uid";
            public static readonly string EVENT_NAME = "name";
            public static readonly string DATE = "date";
            public static readonly string ROUND_COUNT = "round_count";
        }

        public EventDirectoryTable() : base(TABLE_NAME) {
            MakeTable(this);
        }

        public DataRow AddRow(string eventName, string date) {
            var row = this.NewRow();

            row[COL.EVENT_NAME] = eventName;
            row[COL.DATE] = date;

            this.Rows.Add(row);
            return row;
        }

        public DataRow GetRow(int eventUID) {
            var rows = this.AsEnumerable()
                           .Where(row => row.Field<int>(COL.UID) == eventUID)
                           .ToList();

            if (rows.Count == 0) throw new KeyNotFoundException($"{COL.UID} == {eventUID}");
            return rows[0];
        }

        public bool HasRow(int eventUID) {
            var rows = this.AsEnumerable()
                           .Where(row => row.Field<int>(COL.UID) == eventUID)
                           .ToList();

            if (rows.Count == 0) return false;
            return true;
        }

        public static DataTable MakeTable(EventDirectoryTable? table = null) {
            table ??= new ();

            table.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.UID,
                Unique = true,
                AutoIncrement = true
            });

            table.Columns.Add(new DataColumn {
                DataType = typeof(string),
                ColumnName = COL.EVENT_NAME,
                Unique = false,
                AutoIncrement = false
            });

            table.Columns.Add(new DataColumn {
                DataType = typeof(string),
                ColumnName = COL.DATE,
                Unique = false,
                AutoIncrement = false
            });

            table.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.ROUND_COUNT,
                Unique = false,
                AutoIncrement = false,
                DefaultValue = 0
            });

            table.PrimaryKey = [table.Columns[COL.UID]!];

            return table;
        }
    }
}
