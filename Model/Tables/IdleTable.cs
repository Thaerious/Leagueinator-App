using Model.Views;
using System.Data;
using System.Diagnostics;

namespace Model.Tables {

    public class IdleRow(League league, DataRow row) : CustomRow(league, row) {

        public EventRow Event {
            get => this.League.EventTable.GetRow((int)this.DataRow[IdleTable.COL.ROUND]);
        }

        public PlayerRow Player {
            get => this.League.PlayersTable.GetRow((string)this.DataRow[IdleTable.COL.PLAYER]);
        }

        public int Round {
            get => (int)this.DataRow[IdleTable.COL.ROUND];
            set => this.DataRow[IdleTable.COL.ROUND] = value;
        }
    }

    public class IdleTable(League league) : CustomTable(league, "idle_players") {
        public static class COL {
            public static readonly string ROUND = "round";
            public static readonly string PLAYER = "player";
        }

        public IdleRow AddRow(int round, string playerName) {
            var row = this.NewRow();

            row[COL.ROUND] = round;
            row[COL.PLAYER] = playerName;

            this.Rows.Add(row);
            return new(this.League, row);
        }

        public DataRow? GetRow(int round, string playerName) {
            var rows = this.AsEnumerable()
                           .Where(row => row.Field<int>(COL.ROUND) == round)
                           .Where(row => row.Field<string>(COL.PLAYER) == playerName)
                           .ToList();

            if (rows.Count == 0) return null;
            return rows[0];
        }

        public void RemoveRows(int eventUID, int round, string playerName) {

            var rowsToDelete = this.AsEnumerable()
                               .Where(row => row.Field<int>(COL.ROUND) == round)
                               .Where(row => row.Field<string>(COL.PLAYER) == playerName)
                               .ToList()
                               ;

            foreach (DataRow row in rowsToDelete) {
                this.Rows.Remove(row);
            }
        }

        public ForeignKeyConstraint FKRound => (ForeignKeyConstraint)this.Constraints["FK_Idle_Round"]!;
        public ForeignKeyConstraint FKPlayer => (ForeignKeyConstraint)this.Constraints["FK_Idle_Player"]!;

        public override void BuildColumns() {
            this.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.ROUND,
                Unique = false,
                AutoIncrement = false
            });

            this.Columns.Add(new DataColumn {
                DataType = typeof(string),
                ColumnName = COL.PLAYER,
                Unique = false,
                AutoIncrement = false
            });

            this.Constraints.Add(
                new UniqueConstraint("UniqueConstraint", [
                this.Columns[COL.ROUND]!,
                this.Columns[COL.PLAYER]!
            ]));

            this.Constraints.Add(new ForeignKeyConstraint(
                "FK_Idle_Round",
                this.League.RoundsTable.Columns[RoundTable.COL.UID]!, // Parent column
                this.Columns[COL.ROUND]!                              // Child column
            ) {
                UpdateRule = Rule.Cascade,
                DeleteRule = Rule.Cascade
            });

            this.Constraints.Add(new ForeignKeyConstraint(
                "FK_Idle_Player",
                this.League.PlayersTable.Columns[PlayersTable.COL.NAME]!, // Parent column
                this.Columns[COL.PLAYER]!                                 // Child column
            ) {
                UpdateRule = Rule.Cascade,
                DeleteRule = Rule.Cascade
            });
        }
    }
}
