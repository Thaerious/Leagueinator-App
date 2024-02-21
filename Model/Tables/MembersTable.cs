using System.Data;

namespace Model.Tables {

    public class MemberRow(League league, DataRow row) : CustomRow(league, row) {

        public TeamRow Team {
            get => this.League.TeamTable.GetRow((int)this.DataRow[MembersTable.COL.TEAM]);
        }

        public string Name {
            get => (string)this.DataRow[MembersTable.COL.PLAYER];
            set => this.DataRow[MembersTable.COL.PLAYER] = value;
        }

        public static implicit operator string(MemberRow playerRow) => playerRow.Name;
    }

    public class MembersTable(League league) : CustomTable(league, "members") {

        public static class COL {
            public static readonly string TEAM = "team";
            public static readonly string PLAYER = "player";
        }

        public MemberRow AddRow(int team, string name) {
            if (!this.League.PlayersTable.Has(PlayersTable.COL.NAME, name)) {
                this.League.PlayersTable.AddRow(name);
            }

            var row = this.NewRow();
            row[COL.TEAM] = team;
            row[COL.PLAYER] = name;
            this.Rows.Add(row);
            return new(this.League, row);
        }

        public ForeignKeyConstraint FKTeam { private set; get; }
        public ForeignKeyConstraint FKPlayer { private set; get; }

        public override void BuildColumns() {
            this.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.TEAM,
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
                this.Columns[COL.TEAM]!,
                this.Columns[COL.PLAYER]!
            ]));

            this.FKTeam = new ForeignKeyConstraint(
                "FK_Member_Team",
                this.League.TeamTable.Columns[TeamTable.COL.UID]!, // Parent column
                this.Columns[COL.TEAM]!                              // Child column
            ) {
                UpdateRule = Rule.Cascade,
                DeleteRule = Rule.Cascade
            };

            this.FKPlayer = new ForeignKeyConstraint(
                "FK_Member_Player",
                this.League.PlayersTable.Columns[PlayersTable.COL.NAME]!, // Parent column
                this.Columns[COL.PLAYER]!                               // Child column
            ) {
                UpdateRule = Rule.Cascade,
                DeleteRule = Rule.Cascade
            };

            this.Constraints.Add(this.FKPlayer);
            this.Constraints.Add(this.FKTeam);
        }
    }
}
