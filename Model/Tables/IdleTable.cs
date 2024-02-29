using System.Data;

namespace Model.Tables {

    public class IdleRow(DataRow dataRow) : CustomRow(dataRow) {

        public EventRow Event {
            get => this.League.EventTable.GetRow((int)this.DataRow[IdleTable.COL.ROUND]);
        }

        public PlayerRow Player {
            get => this.League.PlayerTable.GetRow((string)this.DataRow[IdleTable.COL.PLAYER]);
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

        public IdleRow AddRow(int round, string playerName) {
            var row = this.NewRow();

            row[COL.ROUND] = round;
            row[COL.PLAYER] = playerName;

            this.League.EnforceConstraints = false;
            this.Rows.Add(row);
            this.League.EnforceConstraints = true;
            return new(row);
        }

        public List<IdleRow> GetRows(int round, string playerName) {
            return this.AsEnumerable()
                       .Where(row => row.Field<int>(COL.ROUND) == round)
                       .Where(row => row.Field<string>(COL.PLAYER) == playerName)
                       .Select(row => new IdleRow(row))
                       .ToList();
        }

        public void RemoveRows(int round, string playerName) {

            var rowsToDelete = this.AsEnumerable()
                               .Where(row => row.Field<int>(COL.ROUND) == round)
                               .Where(row => row.Field<string>(COL.PLAYER) == playerName)
                               .ToList()
                               ;

            foreach (DataRow row in rowsToDelete) {
                this.Rows.Remove(row);
            }
        }
        
        public IdleTable() : base("idle_players") {
            this.RowChanging += (object sender, DataRowChangeEventArgs e) => {
                // Add name to players table if it is not already there.
                string name = (string)e.Row[COL.PLAYER];
                if (!this.League.PlayerTable.Has(PlayerTable.COL.NAME, name)) {
                    this.League.PlayerTable.AddRow(name);
                }

                // Remove the name from the teams table
                int roundUID = (int)e.Row[COL.ROUND];
                RoundRow roundRow = this.League.RoundTable.GetRow(roundUID);

                foreach (MatchRow matchRow in roundRow.Matches) {
                    foreach (TeamRow teamRow in matchRow.Teams) {
                        foreach (MemberRow memberRow in teamRow.Members) {
                            if (memberRow.Player == name) memberRow.Delete();
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
