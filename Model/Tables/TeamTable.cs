using Leagueinator.Utility;
using System.Data;

namespace Model.Tables {
    public class TeamTable : DataTable {
        public static readonly string TABLE_NAME = "team";

        public static class COL {
            public static readonly string UID = "uid";
            public static readonly string EVENT_NAME = "event_name";
            public static readonly string ROUND = "round";
            public static readonly string TEAM_IDX = "team_index";
            public static readonly string PLAYER_NAME = "player_name";
        }

        public TeamTable() : base(TABLE_NAME) {
            MakeTable(this);
        }

        public DataRow AddRow(string eventName, int round, int teamIdx, string playerName) {
            var row = this.NewRow();
            row[COL.EVENT_NAME] = eventName;
            row[COL.ROUND] = round;
            row[COL.TEAM_IDX] = teamIdx;
            row[COL.PLAYER_NAME] = playerName;
            this.Rows.Add(row);
            return row;
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
                DataType = typeof(string),
                ColumnName = COL.EVENT_NAME,
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
                DataType = typeof(int),
                ColumnName = COL.TEAM_IDX,
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
