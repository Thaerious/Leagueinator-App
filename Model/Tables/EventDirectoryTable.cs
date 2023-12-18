using Leagueinator.Utility;
using System.Data;

namespace Model.Tables {
    public class EventDirectoryTable : DataTable {
        public static readonly string TABLE_NAME = "event_directory";

        public static class COL {
            public static readonly string ID = "uid";
            public static readonly string NAME = "name";
            public static readonly string DATE = "date";
        }

        public EventDirectoryTable() : base(TABLE_NAME) {
            MakeTable(this);
        }

        public static DataTable MakeTable(EventDirectoryTable? table = null) {
            table ??= new ();

            table.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.ID,
                Unique = true,
                AutoIncrement = true
            });

            table.Columns.Add(new DataColumn {
                DataType = typeof(string),
                ColumnName = COL.NAME,
                Unique = true,
                AutoIncrement = false
            });

            table.Columns.Add(new DataColumn {
                DataType = typeof(string),
                ColumnName = COL.DATE,
                Unique = false,
                AutoIncrement = false
            });

            return table;
        }
    }
}
