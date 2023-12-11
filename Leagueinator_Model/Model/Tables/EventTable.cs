using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics;
using System.Text;

namespace Leagueinator_Model.Model.Tables {
    public class EventTable : ATable {
        public static readonly string TABLE_NAME = "event";

        public EventTable() : this(MakeEventTable()) { }

        public EventTable(DataTable source) {
            this.Table = source;
        }

        public EventTable(DataSet source) {
            this.Table = source.Tables[TABLE_NAME] ?? throw new NullReferenceException($"table '{TABLE_NAME}' not found");
        }

        public static DataTable MakeEventTable() {
            DataTable table = new DataTable(TABLE_NAME);
            DataColumn column;

            table.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = "uid",
                Unique = true,
                AutoIncrement = true
            });

            table.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = "round"
            });

            table.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = "lane"
            });


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

            return table;
        }

        internal void AddRow(int round, int lane, int teamID, int bowls, int ends, int against) {
            var eRow = this.Table.NewRow();
            eRow["round"] = round;
            eRow["lane"] = lane;
            eRow["team"] = teamID;
            eRow["bowls"] = bowls;
            eRow["bowls="] = Math.Min(bowls, ends * 1.5);
            eRow["bowls+"] = bowls - Math.Min(bowls, ends * 1.5);
            eRow["ends"] = ends;
            eRow["against"] = against;
            eRow["against="] = Math.Min(against, ends * 1.5);
            eRow["against+"] = against - Math.Min(against, ends * 1.5);
            this.Table.Rows.Add(eRow);
        }

        private List<int> RoundList() {
            SortedSet<int> ids = new();
            foreach (DataRow row in this.Table.AsEnumerable()) {
                int id = (row.Field<int>("round"));
                if (!ids.Contains(id)) ids.Add(id);
            }
            return ids.ToList();
        }

        public void AssignRanks() {
            foreach (int round in RoundList()) {
                var sortedRows = Table.AsEnumerable()
                    .Where(row => row.Field<int>("round") == round)
                    .OrderBy(row => row.Field<int>("bowls="))
                    .ThenBy(row => row.Field<int>("bowls+"))
                    .ThenBy(row => row.Field<int>("against="))
                    .ThenBy(row => row.Field<int>("against+"));

                int rank = 1;
                foreach (var row in sortedRows.Reverse()) {
                    this.Table.Select($"uid = '{row["uid"]}'")[0]["rank"] = rank++;
                }
            }
        }

        public DataRow[] GetRowsById(int uid) {
            return this.Table.Select($"uid = '{uid}'");
        }

        public DataRow[] GetRowsByTeam(int teamUID) {
            return this.Table.Select($"round = '{teamUID}'");
        }
    }
}
