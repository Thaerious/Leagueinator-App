using System.Data;

namespace Model.Tables {
    public class EventTable : DataTable {
        public static readonly string TABLE_NAME = "event";

        public static class COL {
            public static readonly string ID = "uid";
            public static readonly string EVENT_NAME = "event_name";
            public static readonly string ROUND = "round";
            public static readonly string LANE = "lane";
            public static readonly string TEAM_IDX = "team";
            public static readonly string TIE = "tie";
            public static readonly string BOWLS = "bowls";
            public static readonly string ENDS = "ends";
        }

        public EventTable() : base(TABLE_NAME) {
            MakeTable(this);
        }

        public DataRow AddRow(string eventName, int round, int lane, int teamIdx) {
            var row = this.NewRow();
            row[COL.EVENT_NAME] = eventName;
            row[COL.ROUND] = round;
            row[COL.LANE] = lane;
            row[COL.TEAM_IDX] = teamIdx;
            row[COL.TIE] = 0;
            row[COL.BOWLS] = 0;
            row[COL.ENDS] = 0;
            this.Rows.Add(row);
            return row;
        }

        public static EventTable MakeTable(EventTable? table = null) {
            table ??= new();

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
                ColumnName = COL.TEAM_IDX
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
