using System.Data;

namespace Leagueinator.Model.Tables {

    public class PlayerTable : LeagueTable<PlayerRow> {
        public PlayerTable() : base("players") {
            this.NewInstance = dataRow => new PlayerRow(dataRow);
        }

        public static class COL {
            public static readonly string NAME = "name";
        }

        public bool HasRow(string name) {
            return this.Select($"{COL.NAME} = '{name}'").Length != 0;
        }

        public bool AddRowIf(string name) {
            if (this.HasRow(name)) return false;
            var row = this.NewRow();
            row[COL.NAME] = name;
            this.Rows.Add(row);
            return true;
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
