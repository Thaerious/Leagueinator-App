using System.Data;

namespace Model.Tables {

    public class PlayerRow(League league, DataRow row) : CustomRow(league, row) {

        public TeamRow Team {
            get => this.League.TeamTable.GetRow((int)this.DataRow[PlayerTable.COL.TEAM]);
        }

        public string Name {
            get => (string)this.DataRow[PlayerTable.COL.NAME];
            set => this.DataRow[PlayerTable.COL.NAME] = value;
        }

        public static implicit operator string(PlayerRow playerRow) => playerRow.Name;
    }

    public class PlayerTable(League league) : CustomTable(league, "players") {

        public static class COL {
            public static readonly string TEAM = "team";
            public static readonly string NAME = "name";
        }

        public PlayerRow AddRow(int team, string name) {
            var row = this.NewRow();
            row[COL.TEAM] = team;
            row[COL.NAME] = name;
            this.Rows.Add(row);
            return new(this.League, row);
        }

        public override void BuildColumns() {
            this.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.TEAM,
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
                this.Columns[COL.TEAM]!,
                this.Columns[COL.NAME]!
            ]));
        }
    }
}
