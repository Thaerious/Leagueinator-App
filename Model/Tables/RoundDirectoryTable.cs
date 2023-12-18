using System.Data;

namespace Model.Tables {
    public class RoundDirectoryTable : DataTable{
        public static readonly string TABLE_NAME = "round_directory";
        public static class COL {
            public static readonly string ID = "uid";
            public static readonly string EVENT_NAME = "event_name";
            public static readonly string ROUND_COUNT = "round_count";
        }

        public RoundDirectoryTable() : base(TABLE_NAME) {
            MakeTable(this);
        }

        public static RoundDirectoryTable MakeTable(RoundDirectoryTable? table = null) {
            table ??= new();

            table.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.ID,
                Unique = true,
                AutoIncrement = true
            });

            table.Columns.Add(new DataColumn {
                DataType = typeof(string),
                ColumnName = COL.EVENT_NAME,
                Unique = true,
                AutoIncrement = false
            });

            table.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.ROUND_COUNT,
                Unique = false,
                AutoIncrement = false
            });

            return table;
        }
    }
}
