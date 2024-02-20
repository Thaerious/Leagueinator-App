using Model.Views;
using System.Data;

namespace Model.Tables {

    public class TeamRow : CustomRow {

        public readonly ReflectedRowList<MemberRow, MembersTable, int> Members;

        public TeamRow(League league, DataRow row) : base(league, row) {
            InvalidTableException.CheckTable<TeamTable>(row);
            this.Members = new(league.MembersTable.FKTeam, this);
        }

        public int UID {
            get => (int)this.DataRow[TeamTable.COL.UID];
        }

        public static implicit operator int(TeamRow teamRow) => teamRow.UID;

        public MatchRow Match {
            get => this.League.MatchTable.GetRow((int)this.DataRow[TeamTable.COL.MATCH]);
        }

        public int Bowls {
            get => (int)this.DataRow[TeamTable.COL.BOWLS];
            set => this.DataRow[TeamTable.COL.BOWLS] = value;
        }

        public int Tie {
            get => (int)this.DataRow[TeamTable.COL.TIE];
            set => this.DataRow[TeamTable.COL.TIE] = value;
        }
    }

    public class TeamTable(League league) : CustomTable(league, "teams") {

        public static class COL {
            public static readonly string UID = "uid";
            public static readonly string MATCH = "match_uid";
            public static readonly string BOWLS = "bowls";
            public static readonly string TIE = "tie";
        }

        public TeamRow AddRow(int match) {
            var row = this.NewRow();
            row[COL.MATCH] = match;
            this.Rows.Add(row);
            return new(this.League, row);
        }

        public TeamRow GetRow(int eventUID) {
            var rows = this.AsEnumerable()
                           .Where(row => row.Field<int>(COL.UID) == eventUID)
                           .ToList();

            if (rows.Count == 0) throw new KeyNotFoundException($"{COL.UID} == {eventUID}");
            return new(League, rows[0]);
        }

        public ForeignKeyConstraint FKMatch { private set; get; }

        public override void BuildColumns() {
            this.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.UID,
                Unique = true,
                AutoIncrement = true
            });

            this.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.MATCH,
                Unique = false,
                AutoIncrement = false
            });

            this.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.BOWLS,
                Unique = false,
                AutoIncrement = false,
                DefaultValue = 0
            });

            this.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.TIE,
                Unique = false,
                AutoIncrement = false,
                DefaultValue = 0
            });

            this.FKMatch = new ForeignKeyConstraint(
                "FK_Match_Team",
                this.League.MatchTable.Columns[MatchTable.COL.UID]!, // Parent column
                this.Columns[COL.MATCH]!                             // Child column
            ) {
                UpdateRule = Rule.Cascade,
                DeleteRule = Rule.Cascade
            };

            this.Constraints.Add(this.FKMatch);
        }
    }
}
