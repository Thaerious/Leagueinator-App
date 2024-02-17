using Model.Views;
using System.Data;

namespace Model.Tables {

    public class MatchRow : CustomRow {
        public readonly TeamCollection Teams;

        public MatchRow(League league, DataRow row) : base(league, row) {
            this.Teams = new(league.TeamTable, this);
        }

        public int UID {
            get => (int)this.DataRow[MatchTable.COL.UID];
        }

        public static implicit operator int(MatchRow matchRow) => matchRow.UID;

        public RoundRow Round {
            get => this.League.RoundTable.GetRow((int)this.DataRow[MatchTable.COL.ROUND]);
        }

        public int Lane {
            get => (int)this.DataRow[MatchTable.COL.LANE];
            set => this.DataRow[MatchTable.COL.LANE] = value;
        }

        public int Ends {
            get => (int)this.DataRow[MatchTable.COL.ENDS];
            set => this.DataRow[MatchTable.COL.ENDS] = value;
        }
    }

    public class MatchTable(League league) : CustomTable(league, "matches") {

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
            return new(this.League, row);
        }

        public MatchRow GetRow(int matchUID) {
            var rows = this.AsEnumerable()
                           .Where(row => row.Field<int>(COL.UID) == matchUID)
                           .ToList();

            if (rows.Count == 0) throw new KeyNotFoundException($"{COL.UID} == {matchUID}");
            return new(this.League, rows[0]);
        }

        public DataRow GetRow(int eventUID, int round, int lane) {
            var rows = this.AsEnumerable()
                        .Where(row => row.Field<int>(COL.ROUND) == round)
                        .Where(row => row.Field<int>(COL.LANE) == lane)
                        .ToList()
                        ;

            if (rows.Count == 0) throw new KeyNotFoundException();
            return rows[0];
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
                ColumnName = COL.ENDS
            });

            this.Constraints.Add(
                new UniqueConstraint("UniqueConstraint", [
                this.Columns[COL.ROUND]!,
                this.Columns[COL.LANE]!
            ]));
        }
    }
}
