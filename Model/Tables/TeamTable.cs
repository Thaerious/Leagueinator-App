using Leagueinator.Utility;
using System.Data;

namespace Model.Tables {
    internal class TeamTable{
        public static readonly string TABLE_NAME = "team";

        public static class COL {
            public static readonly string ID = "uid";
            public static readonly string EVENT_NAME = "event_name";
            public static readonly string TEAM_IDX = "team_index";
            public static readonly string PLAYER_NAME = "player_name";
        }

        public static DataTable MakeTable() {
            DataTable table = new DataTable(TABLE_NAME);

            table.Columns.Add(new DataColumn {
                DataType = typeof(string),
                ColumnName = COL.EVENT_NAME,
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
