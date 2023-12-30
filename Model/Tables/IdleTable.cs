using System.Data;
using System.Diagnostics;

namespace Model.Tables {
    public class IdleTable : DataTable {
        public static readonly string TABLE_NAME = "idle";

        public static class COL {
            public static readonly string UID = "uid";
            public static readonly string EVENT_UID = "event_dir_uid";
            public static readonly string ROUND = "round";
            public static readonly string PLAYER_NAME = "player_name";
        }

        public IdleTable() : base(TABLE_NAME) {
            MakeTable(this);
        }

        public DataRow AddRow(int eventUID, int round, string playerName) {
            var row = this.NewRow();

            row[COL.EVENT_UID] = eventUID;
            row[COL.ROUND] = round;
            row[COL.PLAYER_NAME] = playerName;

            this.Rows.Add(row);
            return row;
        }

        public DataRow? GetRow(int eventDirUID, int round, string playerName) {
            var rows = this.AsEnumerable()
                           .Where(row => row.Field<int>(COL.EVENT_UID) == eventDirUID)
                           .Where(row => row.Field<int>(COL.ROUND) == round)
                           .Where(row => row.Field<string>(COL.PLAYER_NAME) == playerName)
                           .ToList();

            if (rows.Count == 0) return null;
            return rows[0];
        }

        public void RemoveRows(int eventUID, int round, string playerName) {

            var rowsToDelete = this.AsEnumerable()
                               .Where(row => row.Field<int>(COL.EVENT_UID) == eventUID)
                               .Where(row => row.Field<int>(COL.ROUND) == round)
                               .Where(row => row.Field<string>(COL.PLAYER_NAME) == playerName)
                               .ToList()
                               ;

            foreach (DataRow row in rowsToDelete) {
                this.Rows.Remove(row);
            }
        }

        public static IdleTable MakeTable(IdleTable? table = null) {
            table ??= new();

            table.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.UID,
                Unique = true,
                AutoIncrement = true
            });

            table.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.EVENT_UID,
                Unique = false,
                AutoIncrement = false
            });

            table.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.ROUND,
                Unique = false,
                AutoIncrement = false
            });

            table.Columns.Add(new DataColumn {
                DataType = typeof(string),
                ColumnName = COL.PLAYER_NAME,
                Unique = false,
                AutoIncrement = false
            });

            return table;
        }
    }
}
