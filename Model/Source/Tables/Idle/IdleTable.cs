﻿using System.Data;

namespace Leagueinator.Model.Tables {

    public class IdleTable : LeagueTable<IdleRow> {
        public static class COL {
            public static readonly string ROUND = "round";
            public static readonly string PLAYER = "player";
        }

        public IdleRow AddRow(int round, string playerName) {
            var row = this.NewRow();

            row[COL.ROUND] = round;
            row[COL.PLAYER] = playerName;

            this.League.EnforceConstraints = false;
            this.Rows.Add(row);
            this.League.EnforceConstraints = true;
            return new(row);
        }

        public bool HasRow(int round, string player) {
            return this.AsEnumerable<IdleRow>()
                       .Where(row => row.Round.UID == round)
                       .Where(row => row.Player == player)
                       .Any();
        }

        public IdleRow GetRow(int round, string player) {
            var rows = this.AsEnumerable<IdleRow>()
                       .Where(row => row.Round.UID == round)
                       .Where(row => row.Player == player)
                       .ToList();

            if (rows.Count == 0) throw new KeyNotFoundException();
            return rows[0];
        }

        public void RemoveRows(int round, string player) {

            var rowsToDelete = this.AsEnumerable<IdleRow>()
                                   .Where(row => row.Round.UID == round)
                                   .Where(row => row.Player == player)
                                   .ToList();

            foreach (DataRow row in rowsToDelete) {
                this.Rows.Remove(row);
            }
        }

        public IdleTable() : base("idle_players") {
            this.NewInstance = dataRow => new IdleRow(dataRow);

            // When a player is added to the idle table, remove it from any team tables.
            this.RowChanging += (object sender, DataRowChangeEventArgs e) => {
                string name = (string)e.Row[COL.PLAYER];

                // Ensure the name is in the players table
                if (!this.League.PlayerTable.HasRow(name)) {
                    this.League.PlayerTable.AddRow(name);
                }

                // Remove the name from the teams table
                int roundUID = (int)e.Row[COL.ROUND];
                RoundRow roundRow = this.League.RoundTable.GetRow(roundUID);

                foreach (MatchRow matchRow in roundRow.Matches) {
                    foreach (TeamRow teamRow in matchRow.Teams) {
                        foreach (MemberRow memberRow in teamRow.Members) {
                            if (memberRow.Player == name) {
                                memberRow.Remove();
                            }
                        }
                    }
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
        }
    }
}
