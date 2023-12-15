using Leagueinator.Utility;
using System.Data;

namespace Model.Tables {
    internal class RoundDirectoryTable {
        public static readonly string TABLE_NAME = "round_directory";
        public static readonly string ID_COL = "uid";
        public static readonly string EVENT_ID_COL = "event_id";
        public static readonly string ROUND_COUNT_COL = "round_count";

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
                ColumnName = EVENT_ID_COL,
                Unique = false,
                AutoIncrement = false
            });

            table.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = ROUND_COUNT_COL,
                Unique = false,
                AutoIncrement = false
            });

            return table;
        }
    }
}
