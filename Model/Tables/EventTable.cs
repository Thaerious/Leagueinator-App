using System.Data;

namespace Model.Tables {
    internal static class EventTable {
        public static readonly string TABLE_NAME = "event";

        public static class COL {
            public static readonly string ID = "uid";
            public static readonly string EVENT_NAME = "event_name";
            public static readonly string ROUND = "round";
            public static readonly string LANE = "lane";
            public static readonly string TEAM = "team";
            public static readonly string TIE = "tie";
            public static readonly string BOWLS = "bowls";
            public static readonly string ENDS = "ends";
        }

        public static DataTable MakeTable() {
            DataTable table = new DataTable(TABLE_NAME);

            table.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.ID,
                Unique = true,
                AutoIncrement = true
            });

            table.Columns.Add(new DataColumn {
                DataType = typeof(string),
                ColumnName = COL.EVENT_NAME
            });

            table.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.ROUND
            });

            table.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.LANE
            });

            table.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.TEAM
            });

            table.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.TIE
            });

            table.Columns.Add(new DataColumn() {
                DataType = typeof(int),
                ColumnName = COL.BOWLS
            });

            table.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.ENDS
            });

            return table;
        }
    }
}
