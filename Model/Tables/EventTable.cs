using System.Data;

namespace Model.Tables {
    internal static class EventTable {
        public static readonly string TABLE_NAME = "event";
        public static readonly string ID_COL = "uid";

        public static DataTable MakeTable() {
            DataTable table = new DataTable(TABLE_NAME);

            table.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = "uid",
                Unique = true,
                AutoIncrement = true
            });

            table.Columns.Add(new DataColumn {
                DataType = typeof(string),
                ColumnName = "event_name"
            });

            table.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = "round"
            });

            table.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = "lane"
            });

            table.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = "team"
            });

            table.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = "tie"
            });

            table.Columns.Add(new DataColumn() {
                DataType = typeof(int),
                ColumnName = "bowls"
            });

            table.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = "ends"
            });

            return table;
        }
    }
}
