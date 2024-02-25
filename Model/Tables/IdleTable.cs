﻿using System.Data;

namespace Model.Tables {

    public class IdleRow(DataRow dataRow) : CustomRow(dataRow) {

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

    public class IdleTable : LeagueTable<IdleRow> {
        public static class COL {
            public static readonly string ROUND = "round";
            public static readonly string PLAYER = "player";
        }

        public IdleRow AddRow(int round, string name) {
            if (!this.League.PlayersTable.Has(PlayersTable.COL.NAME, name)) {
                this.League.PlayersTable.AddRow(name);
            }

            var row = this.NewRow();

            row[COL.ROUND] = round;
            row[COL.PLAYER] = name;

            this.Rows.Add(row);
            return new(row);
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

        public ForeignKeyConstraint? FKRound { private set; get; }

        public IdleTable() : base("idle_players") {
            this.RowChanging += (object sender, DataRowChangeEventArgs e) => {
                string name = (string)e.Row[COL.PLAYER];
                if (!this.League.PlayersTable.Has(PlayersTable.COL.NAME, name)) {
                    this.League.PlayersTable.AddRow(name);
                }
            };
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
                ColumnName = COL.PLAYER,
                Unique = false,
                AutoIncrement = false
            });

            this.Constraints.Add(
                new UniqueConstraint("UniqueConstraint", [
                this.Columns[COL.ROUND]!,
                this.Columns[COL.PLAYER]!
            ]));

            this.FKRound = new ForeignKeyConstraint(
                "FK_Idle_Round",
                this.League.RoundsTable.Columns[RoundTable.COL.UID]!, // Parent column
                this.Columns[COL.ROUND]!                              // Child column
            ) {
                UpdateRule = Rule.Cascade,
                DeleteRule = Rule.Cascade
            };

            this.Constraints.Add(this.FKRound);
        }
    }
}
