using System.Data;
using System.Xml.Linq;

namespace Model.Tables {

    public class MemberRow(DataRow dataRow) : CustomRow(dataRow) {

        public TeamRow Team {
            get => this.League.TeamTable.GetRow(this.Match, this.Index);
        }

        public int Index {
            get => (int)this.DataRow[MembersTable.COL.INDEX];
        }


        public int Match {
            get => (int)this.DataRow[MembersTable.COL.MATCH];
        }

        public string Name {
            get => (string)this.DataRow[MembersTable.COL.PLAYER];
            set => this.DataRow[MembersTable.COL.PLAYER] = value;
        }

        public static implicit operator string(MemberRow playerRow) => playerRow.Name;
    }

    public class MembersTable : LeagueTable<MemberRow> {

        public static class COL {
            public static readonly string MATCH = "match";
            public static readonly string INDEX = "index";
            public static readonly string PLAYER = "player";
        }

        public MemberRow AddRow(int match, int index, string name) {
            var row = this.NewRow();
            row[COL.MATCH] = match;
            row[COL.INDEX] = index;
            row[COL.PLAYER] = name;
            this.Rows.Add(row);
            return new(row);
        }

        public MembersTable() : base("members") {
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
                ColumnName = COL.MATCH,
                Unique = false,
                AutoIncrement = false
            });

            this.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.INDEX,
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
                this.Columns[COL.MATCH]!,
                this.Columns[COL.INDEX]!,
                this.Columns[COL.PLAYER]!
            ]));
        }
    }
}
