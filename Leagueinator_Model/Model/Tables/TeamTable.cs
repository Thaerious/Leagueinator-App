using Leagueinator.Model;
using Leagueinator.Utility;
using System.Data;
using System.Numerics;

namespace Leagueinator_Model.Model.Tables {
    public class TeamTable {
        private DataTable source;

        public TeamTable(DataTable source) {
            this.source = source;
        }

        public static DataTable MakeTeamTable() {
            DataTable table = new DataTable("team");

            table.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = "id",
                Unique = false,
                AutoIncrement = false
            });

            table.Columns.Add(new DataColumn {
                DataType = typeof(string),
                ColumnName = "name",
                Unique = false,
                AutoIncrement = false
            });

            return table;
        }

        /// <summary>
        /// If the particular team has not been added, add a new team to the table
        /// </summary>
        /// <param name="names"></param>
        /// <returns>The id of the team</returns>
        public int TryAddTeam(params string[] names) {
            int prevId = this.GetID(names);
            if (prevId != -1) return prevId;

            object result = this.source.Compute("MAX(id)", string.Empty);
            int maxValue = (result != DBNull.Value) ? Convert.ToInt32(result) : 0;
            int nextID = maxValue + 1;

            foreach (string name in names) {
                var row = source.NewRow();
                row["id"] = nextID;
                row["name"] = name;
                this.source.Rows.Add(row);
            }

            return nextID;
        }

        /// <summary>
        /// Retrieve all names with matching id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>An array of names, an empty array if id is not found</returns>
        public string[] GetNames(int id) {
            DataRow[] rows = this.source.Select($"id = {id} ");
            return rows.Select(idRow => idRow.Field<string>("name")).NotNull().ToArray();
        }

        /// <summary>
        /// Determine id of the team that contains all the listed players.
        /// </summary>
        /// <param name="names"></param>
        /// <returns>The team id or -1 if not found</returns>
        public int GetID(params string[] names) {
            Array.Sort(names);
            List<int> idList = new();

            // retrieve all matches for the first player name
            if (names.Length == 0) return -1;
            DataRow[] firstNameRows = this.source.Select($"name = '{names[0].ToLower()}' ");
            foreach (DataRow row in firstNameRows) {
                idList.Add(row.Field<int>("id"));
            }

            foreach (int id in idList.ToArray()) {
                DataRow[] idRows = this.source.Select($"id = {id} ");
                // only consider matches that have the same number of players as the list of names
                if (idRows.Length != names.Length) continue;

                string[] found = idRows.Select(idRow => idRow.Field<string>("name")).NotNull().ToArray();
                Array.Sort(found);
                if (found.SequenceEqual(names)) return id;
            }

            return -1;
        }
    }
}
