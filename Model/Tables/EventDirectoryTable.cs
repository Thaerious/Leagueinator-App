using Leagueinator.Utility;
using System.Data;

namespace Model.Tables {
    internal static class EventDirectoryTable {
        public static readonly string TABLE_NAME = "event_directory";
        public static readonly string ID_COL = "uid";
        public static readonly string NAME_COL = "name";
        public static readonly string DATE_COL = "date";

        public static DataTable MakeTable() {
            DataTable table = new DataTable(TABLE_NAME);

            table.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = ID_COL,
                Unique = true,
                AutoIncrement = true
            });

            table.Columns.Add(new DataColumn {
                DataType = typeof(string),
                ColumnName = NAME_COL,
                Unique = true,
                AutoIncrement = false
            });

            table.Columns.Add(new DataColumn {
                DataType = typeof(string),
                ColumnName = DATE_COL,
                Unique = false,
                AutoIncrement = false
            });

            return table;
        }
    }
}
