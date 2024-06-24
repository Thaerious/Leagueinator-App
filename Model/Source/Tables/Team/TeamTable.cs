using System.Data;

namespace Leagueinator.Model.Tables {
    public class TeamTable : LeagueTable<TeamRow> {
        public TeamTable() : base("teams") {
            this.NewInstance = dataRow => new TeamRow(dataRow);
        }

        public static class COL {
            public static readonly string MATCH = "match";
            public static readonly string INDEX = "index";
            public static readonly string BOWLS = "bowls";
            public static readonly string TIE = "tie";
        }

        public int LastIndex(int match) {
            return this.AsEnumerable<TeamRow>()
                .Where(row => row.Match.UID == match)
                .Select(row => row.Index)
                .DefaultIfEmpty(-1)
                .Max();
        }

        public TeamRow AddRow(int match, int index) {
            var row = this.NewRow();
            row[COL.MATCH] = match;
            row[COL.INDEX] = index;
            this.Rows.Add(row);
            return new(row);
        }

        /// <summary>
        /// Add a row, infer the index from the previous indices.
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public TeamRow AddRow(int match) {
            return this.AddRow(match, this.LastIndex(match) + 1);
        }

        public TeamRow GetRow(int match, int index) {
            return this.AsEnumerable<TeamRow>()
                       .Where(row => row.Match.UID == match)
                       .Where(row => row.Index == index)
                       .First();
        }

        public bool HasRow(int match, int index) {
            return this.AsEnumerable<TeamRow>()
                       .Where(row => row.Match.UID == match)
                       .Where(row => row.Index == index)
                       .Any();
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

            this.Constraints.Add(new UniqueConstraint(
                [this.Columns[COL.MATCH]!, this.Columns[COL.INDEX]!]
                , true
            ));
        }
    }
}
