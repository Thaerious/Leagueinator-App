using System.Data;

namespace Leagueinator.Model.Tables {
    public class SummaryTable : ATable {
        public static readonly string TABLE_NAME = "summary";

        public SummaryTable() : this(MakeSummaryTable()) { }

        public SummaryTable(DataTable source) {
            this.Table = source;
        }

        public SummaryTable(DataSet source) {
            this.Table = source.Tables[TABLE_NAME] ?? throw new NullReferenceException($"table '{TABLE_NAME}' not found");
        }

        public static DataTable MakeSummaryTable() {
            DataTable table = new DataTable(TABLE_NAME);

            table.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = "rank"
            });

            table.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = "team"
            });

            table.Columns.Add(new DataColumn() {
                DataType = typeof(int),
                ColumnName = "bowls"
            });

            table.Columns.Add(new DataColumn() {
                DataType = typeof(int),
                ColumnName = "bowls="
            });

            table.Columns.Add(new DataColumn() {
                DataType = typeof(int),
                ColumnName = "bowls+"
            });

            table.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = "win"
            });

            table.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = "ends"
            });

            table.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = "against"
            });

            table.Columns.Add(new DataColumn() {
                DataType = typeof(int),
                ColumnName = "against="
            });

            table.Columns.Add(new DataColumn() {
                DataType = typeof(int),
                ColumnName = "against+"
            });

            table.PrimaryKey = new DataColumn[] { table.Columns["team"]! };

            return table;
        }

        public void AddRound(int teamID, int bowls, int ends, int against, int win) {
            var row = this.Table.Rows.Find(teamID);
            var bowlsEq = Math.Min(bowls, ends * 1.5);
            var bowlsPlus = bowls - Math.Min(bowls, ends * 1.5);
            var againstEq = Math.Min(against, ends * 1.5);
            var againstPlus = against - Math.Min(against, ends * 1.5);

            if (row == null) {
                AddRow(teamID, bowls, ends, against, win);
            }
            else {
                row["ends"] = (int)row["ends"] + ends;
                row["win"] = (int)row["win"] + win;
                row["bowls"] = (int)row["bowls"] + bowls;
                row["bowls+"] = (int)row["bowls+"] + bowlsPlus;
                row["bowls="] = (int)row["bowls="] + bowlsEq;
                row["against"] = (int)row["against"] + against;
                row["against="] = (int)row["against="] + againstEq;
                row["against+"] = (int)row["against+"] + againstPlus;
            }
        }

        internal void AddRow(int teamID, int bowls, int ends, int against, int win) {
            var eRow = this.Table.NewRow();
            eRow["team"] = teamID;
            eRow["bowls"] = bowls;
            eRow["win"] = win;
            eRow["bowls="] = Math.Min(bowls, ends * 1.5);
            eRow["bowls+"] = bowls - Math.Min(bowls, ends * 1.5);
            eRow["ends"] = ends;
            eRow["against"] = against;
            eRow["against="] = Math.Min(against, ends * 1.5);
            eRow["against+"] = against - Math.Min(against, ends * 1.5);
            this.Table.Rows.Add(eRow);
        }

        private List<int> TeamList() {
            SortedSet<int> ids = new();
            foreach (DataRow row in this.Table.AsEnumerable()) {
                int id = (row.Field<int>("team"));
                if (!ids.Contains(id)) ids.Add(id);
            }
            return ids.ToList();
        }

        public void AssignRanks() {
            foreach (int round in TeamList()) {
                var sortedRows = Table.AsEnumerable()
                    .OrderBy(row => row.Field<int>("win"))
                    .ThenBy(row => row.Field<int>("bowls="))
                    .ThenBy(row => row.Field<int>("bowls+"))
                    .ThenBy(row => row.Field<int>("against="))
                    .ThenBy(row => row.Field<int>("against+"));

                int rank = 1;
                foreach (var row in sortedRows.Reverse()) {
                    row["rank"] = rank++;
                }
            }
        }

        public DataRow[] GetRowsById(int uid) {
            return this.Table.Select($"uid = '{uid}'");
        }

        public DataRow[] GetRowsByTeam(int teamUID) {
            return this.Table.Select($"team = '{teamUID}'");
        }
    }
}
