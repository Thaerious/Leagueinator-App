using System.Data;

namespace Leagueinator.Model.Tables {
    public class MatchTable : LeagueTable<MatchRow> {
        internal MatchTable() : base("matches") {
            this.NewInstance = dataRow => new MatchRow(dataRow);
        }

        internal static class COL {
            public static readonly string UID = "uid";
            public static readonly string ROUND = "round";
            public static readonly string LANE = "lane";
            public static readonly string ENDS = "ends";
            public static readonly string FORMAT = "format";
        }

        internal MatchRow AddRow(int round, int lane, int ends) {
            var row = this.NewRow();
            row[COL.ROUND] = round;
            row[COL.LANE] = lane;
            row[COL.ENDS] = ends;
            this.Rows.Add(row);
            return new(row);
        }

        internal MatchRow GetRow(int matchUID) {
            var rows = this.AsEnumerable<MatchRow>()
                           .Where(row => row.UID == matchUID)
                           .ToList();

            if (rows.Count == 0) throw new KeyNotFoundException($"{COL.UID} == {matchUID}");
            return rows[0];
        }

        internal MatchRow GetRow(int round, int lane) {
            var rows = this.AsEnumerable<MatchRow>()
                        .Where(row => row.Round.UID == round)
                        .Where(row => row.Lane == lane)
                        .ToList();

            if (rows.Count == 0) throw new KeyNotFoundException();
            return rows[0];
        }

        internal bool HasRow(int round, int lane) {
            return this.AsEnumerable<MatchRow>()
                       .Where(row => row.Round.UID == round)
                       .Where(row => row.Lane == lane)
                       .Any();
        }

        internal MatchRow AddRow(int round, int lane) {
            var row = this.NewRow();

            row[COL.ROUND] = round;
            row[COL.LANE] = lane;

            //this.League.EnforceConstraints = false;
            this.Rows.Add(row);
            //this.League.EnforceConstraints = true;
            return new(row);
        }

        internal override void BuildColumns() {
            this.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.UID,
                Unique = true,
                AutoIncrement = true
            });

            this.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.ROUND
            });

            this.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.LANE
            });

            this.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.ENDS,
                DefaultValue = 10
            });

            this.Columns.Add(new DataColumn {
                DataType = typeof(MatchFormat),
                ColumnName = COL.FORMAT,
                DefaultValue = MatchFormat.VS4
            });

            this.Constraints.Add(
                new UniqueConstraint("UniqueConstraint", [
                this.Columns[COL.ROUND]!,
                this.Columns[COL.LANE]!
            ]));
        }
    }
}
