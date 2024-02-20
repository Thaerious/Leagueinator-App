using System.Data;

namespace Model.Tables {
    public class PlayerRow(League league, DataRow row) : CustomRow(league, row) {

        public string Name {
            get => (string)this.DataRow[PlayersTable.COL.NAME];
            set => this.DataRow[PlayersTable.COL.NAME] = value;
        }

        public static implicit operator string(PlayerRow playerRow) => playerRow.Name;
    }

    public class PlayersTable(League league) : CustomTable(league, "players") {

        public static class COL {
            public static readonly string NAME = "name";
        }

        public PlayerRow AddRow(string name) {
            var row = this.NewRow();
            row[COL.NAME] = name;
            this.Rows.Add(row);
            return new(this.League, row);
        }

        public PlayerRow GetRow(string name) {
            DataRow[] foundRows = this.Select($"{COL.NAME} = '{name}'");
            if (foundRows.Length == 0) throw new KeyNotFoundException($"{COL.NAME} == {name}");
            return new(this.League, foundRows[0]);
        }

        public override void BuildColumns() {
            this.Columns.Add(new DataColumn {
                DataType = typeof(string),
                ColumnName = COL.NAME,
                Unique = true
            });

            this.PrimaryKey = [this.Columns[COL.NAME]!];
        }
    }
}
