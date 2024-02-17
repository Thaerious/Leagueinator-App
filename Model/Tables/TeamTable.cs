using Model.Views;
using System.Data;

namespace Model.Tables {

    public class TeamRow : CustomRow {

        public readonly PlayerCollection Players;

        public TeamRow(League league, DataRow row) : base(league, row) {
            this.Players = new(league.PlayerTable, this);
        }

        public int UID {
            get => (int)this.DataRow[EventsTable.COL.UID];
        }

        public MatchRow Match {
            get => this.League.MatchTable.GetRow((int)this.DataRow[TeamTable.COL.MATCH]);
        }

        public int Bowls {
            get => (int)this.DataRow[TeamTable.COL.BOWLS];
            set => this.DataRow[TeamTable.COL.BOWLS] = value;
        }

        public int Tie {
            get => (int)this.DataRow[TeamTable.COL.TIE];
            set => this.DataRow[TeamTable.COL.TIE] = value;
        }
    }

    public class TeamTable(League league) : CustomTable(league, "teams") {

        public static class COL {
            public static readonly string UID = "uid";
            public static readonly string MATCH = "match_uid";
            public static readonly string BOWLS = "bowls";
            public static readonly string TIE = "tie";
        }

        public TeamRow AddRow(int match, int bowls = 0, int tie = 0) {
            var row = this.NewRow();
            row[COL.MATCH] = match;
            row[COL.BOWLS] = bowls;
            row[COL.TIE] = tie;
            this.Rows.Add(row);
            return new(this.League, row);
        }

        public TeamRow GetRow(int eventUID) {
            var rows = this.AsEnumerable()
                           .Where(row => row.Field<int>(COL.UID) == eventUID)
                           .ToList();

            if (rows.Count == 0) throw new KeyNotFoundException($"{COL.UID} == {eventUID}");
            return new(League, rows[0]);
        }

        //public void RemoveRows(int eventTableUID, string playerName) {
        //    var rowsToDelete = this.AsEnumerable()
        //                       .Where(row => row.Field<int>(COL.ROUND_UID) == eventTableUID)
        //                       .Where(row => row.Field<string>(COL.PLAYER_NAME) == playerName)
        //                       .ToList()
        //                       ;

        //    foreach (DataRow row in rowsToDelete) {
        //        this.Rows.Remove(row);
        //    }
        //}

        public override void BuildColumns() {
            this.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.UID,
                Unique = true,
                AutoIncrement = true
            });

            this.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.MATCH,
                Unique = false,
                AutoIncrement = false
            });

            this.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.BOWLS,
                Unique = false,
                AutoIncrement = false
            });

            this.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.TIE,
                Unique = false,
                AutoIncrement = false
            });
        }
    }
}
