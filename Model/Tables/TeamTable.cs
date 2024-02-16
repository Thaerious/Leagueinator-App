using Leagueinator.Utility;
using System.Data;
using System.Diagnostics;

namespace Model.Tables {
    public class TeamTable : DataTable {
        public static readonly string TABLE_NAME = "team";

        public static class COL {
            public static readonly string UID = "uid";
            public static readonly string ROUND_UID = "round_uid";
            public static readonly string PLAYER_NAME = "player_name";
        }

        public TeamTable() : base(TABLE_NAME) {
            MakeTable(this);
        }

        public DataRow AddRow(int eventTableUID, string playerName) {
            var row = this.NewRow();
            row[COL.ROUND_UID] = eventTableUID;
            row[COL.PLAYER_NAME] = playerName;
            this.Rows.Add(row);
            return row;
        }

        public void RemoveRows(int eventTableUID, string playerName) {
            var rowsToDelete = this.AsEnumerable()
                               .Where(row => row.Field<int>(COL.ROUND_UID) == eventTableUID)
                               .Where(row => row.Field<string>(COL.PLAYER_NAME) == playerName)
                               .ToList()
                               ;

            foreach (DataRow row in rowsToDelete) {
                this.Rows.Remove(row);
            }
        }

        public static TeamTable MakeTable(TeamTable? table = null) {
            table ??= new();

            table.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.UID,
                Unique = true,
                AutoIncrement = true
            });

            table.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.ROUND_UID,
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
