using Leagueinator.Model.Views;
using System.Data;

namespace Leagueinator.Model.Tables {

    public class MatchRow(DataRow dataRow) : CustomRow(dataRow) {

        public RowBoundView<TeamRow> Teams {
            get {
                return new(this.League.TeamTable, [TeamTable.COL.MATCH], [this.UID]);
            }
        }

        public int UID {
            get => (int)this[MatchTable.COL.UID];
        }

        public static implicit operator int(MatchRow matchRow) => matchRow.UID;

        public RoundRow Round {
            get => this.League.RoundTable.GetRow((int)this[MatchTable.COL.ROUND]);
        }

        public int Lane {
            get => (int)this[MatchTable.COL.LANE];
            set => this[MatchTable.COL.LANE] = value;
        }

        public int Ends {
            get => (int)this[MatchTable.COL.ENDS];
            set => this[MatchTable.COL.ENDS] = value;
        }
    }

    public class MatchTable : LeagueTable<MatchRow> {
        public MatchTable() : base("matches"){
            this.NewInstance = dataRow => new MatchRow(dataRow);
            GetInstance = args => this.GetRow((int)args[0], (int)args[1]);
            HasInstance = args => this.HasRow((int)args[0], (int)args[1]);
            AddInstance = args => {
                if ((args as object[]).Length == 3) return this.AddRow((int)args[0], (int)args[1], (int)args[2]);
                return this.AddRow((int)args[0], (int)args[1]);
            };
        }

        public static class COL {
            public static readonly string UID = "uid";
            public static readonly string ROUND = "round";
            public static readonly string LANE = "lane";
            public static readonly string ENDS = "ends";
        }

        public MatchRow AddRow(int round, int lane, int ends) {
            var row = this.NewRow();
            row[COL.ROUND] = round;
            row[COL.LANE] = lane;
            row[COL.ENDS] = ends;
            this.Rows.Add(row);
            return new(row);
        }

        public MatchRow GetRow(int matchUID) {
            var rows = this.AsEnumerable<MatchRow>()
                           .Where(row => row.UID == matchUID)
                           .ToList();

            if (rows.Count == 0) throw new KeyNotFoundException($"{COL.UID} == {matchUID}");
            return rows[0];
        }

        public MatchRow GetRow(int round, int lane) {
            var rows = this.AsEnumerable<MatchRow>()
                        .Where(row => row.Round == round)
                        .Where(row => row.Lane == lane)
                        .ToList();

            if (rows.Count == 0) throw new KeyNotFoundException();
            return rows[0];
        }

        public bool HasRow(int round, int lane) {
            return this.AsEnumerable<MatchRow>()
                       .Where(row => row.Round == round)
                       .Where(row => row.Lane == lane)
                       .Any();
        }

        public MatchRow AddRow(int round, int lane) {
            var row = this.NewRow();

            row[COL.ROUND] = round;
            row[COL.LANE] = lane;

            this.League.EnforceConstraints = false;
            this.Rows.Add(row);
            this.League.EnforceConstraints = true;
            return new(row);
        }

        public override void BuildColumns() {
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

            // TODO re-enable, it mucks with rearranging lanes.
            //this.Constraints.Add(
            //    new UniqueConstraint("UniqueConstraint", [
            //    this.Columns[COL.ROUND]!,
            //    this.Columns[COL.LANE]!
            //]));
        }
    }
}
