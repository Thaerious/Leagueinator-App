using Model.Views;
using System.Data;
using static Model.Tables.MembersTable.COL;

namespace Model.Tables {

    public class TeamRow : CustomRow {

        public readonly RowBoundView<MemberRow> Members;

        public TeamRow(DataRow dataRow) : base(dataRow) {
            this.Members = new(this.League.MembersTable, [MATCH, INDEX], [this.Match.UID, this.Index]);
        }

        public MatchRow Match {
            get => this.League.MatchTable.GetRow((int)this.DataRow[TeamTable.COL.MATCH]);
        }

        public int Index {
            get => (int)this.DataRow[TeamTable.COL.INDEX];
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

    public class TeamTable() : LeagueTable<TeamRow>("teams") {

        public static class COL {
            public static readonly string MATCH = "match_uid";
            public static readonly string INDEX = "index";
            public static readonly string BOWLS = "bowls";
            public static readonly string TIE = "tie";
        }

        private int NextIndex(int match) {
            return this.AsEnumerable()
                .Select(row => new TeamRow(row))
                .Where((TeamRow row) => row.Match == match)
                .Select(row => row.Index)
                .DefaultIfEmpty(0)
                .Max() + 1;
        }

        public TeamRow AddRow(int match) {
            var row = this.NewRow();
            row[COL.MATCH] = match;
            row[COL.INDEX] = this.NextIndex(match);
            this.Rows.Add(row);
            return new(row);
        }

        public TeamRow GetRow(int match, int index) {
            return this.AsEnumerable()
                       .Select(row => new TeamRow(row))
                       .Where(row => row.Match == match)
                       .Where(row => row.Index == index)
                       .First();
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

            this.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.INDEX,
                Unique = false,
                AutoIncrement = false
            });

            this.Constraints.Add(new UniqueConstraint(
                [this.Columns[COL.MATCH]!, this.Columns[COL.INDEX]!]
                , true
            ));
        }
    }
}
