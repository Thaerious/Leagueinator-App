using System.Data;

namespace Model.Tables {
    public class EventTable : DataTable {
        public static readonly string TABLE_NAME = "event";

        public static class COL {
            public static readonly string UID = "uid";
            public static readonly string EVENT_UID = "event_dir";  // matches event directory table uid
            public static readonly string ROUND = "round";          // round by index
            public static readonly string LANE = "lane";            // lane (match) by index
            public static readonly string TEAM_IDX = "team";        // team in lane by index    
            public static readonly string TIE = "tie";
            public static readonly string BOWLS = "bowls";
            public static readonly string ENDS = "ends";
        }

        public EventTable() : base(TABLE_NAME) {
            MakeTable(this);
        }

        public DataRow AddRow(int eventUID, int round, int lane, int teamIdx) {
            var row = this.NewRow();
            row[COL.EVENT_UID] = eventUID;
            row[COL.ROUND] = round;
            row[COL.LANE] = lane;
            row[COL.TEAM_IDX] = teamIdx;
            row[COL.TIE] = 0;
            row[COL.BOWLS] = 0;
            row[COL.ENDS] = 0;
            this.Rows.Add(row);
            return row;
        }

        public DataRow GetRow(int eventUID, int round, int lane, int teamIdx) {
            var rows = this.AsEnumerable()
                        .Where(row => row.Field<int>(COL.EVENT_UID) == eventUID)
                        .Where(row => row.Field<int>(COL.ROUND) == round)
                        .Where(row => row.Field<int>(COL.LANE) == lane)
                        .Where(row => row.Field<int>(COL.TEAM_IDX) == teamIdx)
                        .ToList()
                        ;

            if (rows.Count == 0) throw new KeyNotFoundException();
            return rows[0];
        }

        public static EventTable MakeTable(EventTable? table = null) {
            table ??= new();

            table.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.UID,
                Unique = true,
                AutoIncrement = true
            });

            table.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.EVENT_UID
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
                ColumnName = COL.TIE,
                DefaultValue = 0
            });

            table.Columns.Add(new DataColumn() {
                DataType = typeof(int),
                ColumnName = COL.BOWLS,
                DefaultValue = 0
            });

            table.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.ENDS,
                DefaultValue = 0
            });

            return table;
        }
    }
}
