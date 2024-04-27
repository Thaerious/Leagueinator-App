using System.Data;

namespace Leagueinator.Model.Tables {
    public class PlayerRow(DataRow dataRow) : CustomRow(dataRow) {

        public string Name {
            get => (string)this.DataRow[PlayerTable.COL.NAME];
            set => this.DataRow[PlayerTable.COL.NAME] = value;
        }

        public static implicit operator string(PlayerRow playerRow) => playerRow.Name;
    }

    public class PlayerTable() : LeagueTable<PlayerRow>("players") {

        public static class COL {
            public static readonly string NAME = "name";
        }

        public bool HasRow(string name) {
            return this.Select($"{COL.NAME} = '{name}'").Length != 0;
        }

        public PlayerRow AddRow(string name) {
            var row = this.NewRow();
            row[COL.NAME] = name;
            this.Rows.Add(row);
            return new(row);
        }

        public PlayerRow GetRow(string name) {
            DataRow[] foundRows = this.Select($"{COL.NAME} = '{name}'");
            if (foundRows.Length == 0) throw new KeyNotFoundException($"{COL.NAME} == {name}");
            return new(foundRows[0]);
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
