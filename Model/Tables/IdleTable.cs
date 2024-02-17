using System.Data;
using System.Diagnostics;

namespace Model.Tables {

    public class IdleRow(League league, DataRow row) : CustomRow(league, row) {

        public EventRow Event {
            get => this.League.EventTable.GetRow((int)this.DataRow[IdleTable.COL.ROUND]);
        }

        public int Round {
            get => (int)this.DataRow[IdleTable.COL.ROUND];
            set => this.DataRow[IdleTable.COL.ROUND] = value;
        }

        public string Name {
            get => (string)this.DataRow[IdleTable.COL.NAME];
            set => this.DataRow[IdleTable.COL.NAME] = value;
        }
    }

    public class IdleTable(League league) : CustomTable(league, "idle_players") {
        public static class COL {
            public static readonly string ROUND = "round";
            public static readonly string NAME = "name";
        }

        public DataRow AddRow(int eventUID, int round, string playerName) {
            var row = this.NewRow();

            row[COL.ROUND] = round;
            row[COL.NAME] = playerName;

            this.Rows.Add(row);
            return row;
        }

        public DataRow? GetRow(int eventDirUID, int round, string playerName) {
            var rows = this.AsEnumerable()
                           .Where(row => row.Field<int>(COL.ROUND) == round)
                           .Where(row => row.Field<string>(COL.NAME) == playerName)
                           .ToList();

            if (rows.Count == 0) return null;
            return rows[0];
        }

        public void RemoveRows(int eventUID, int round, string playerName) {

            var rowsToDelete = this.AsEnumerable()
                               .Where(row => row.Field<int>(COL.ROUND) == round)
                               .Where(row => row.Field<string>(COL.NAME) == playerName)
                               .ToList()
                               ;

            foreach (DataRow row in rowsToDelete) {
                this.Rows.Remove(row);
            }
        }

        public override void BuildColumns() {
            this.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.ROUND,
                Unique = false,
                AutoIncrement = false
            });

            this.Columns.Add(new DataColumn {
                DataType = typeof(string),
                ColumnName = COL.NAME,
                Unique = false,
                AutoIncrement = false
            });

            this.Constraints.Add(
                new UniqueConstraint("UniqueConstraint", [
                this.Columns[COL.ROUND]!,
                this.Columns[COL.NAME]!
            ]));
        }
    }
}

