using Leagueinator.Utility;
using System.Data;

namespace Leagueinator_Model.Model.Tables {
    public class TeamTable {
        private static readonly string TABLE_NAME = "team";
        private static readonly string ID_COL = "id";
        private static readonly string NAME_COL = "name";
        private DataTable source;

        public TeamTable(DataTable source) {
            this.source = source;
        }

        public TeamTable(DataSet source) {
            this.source = source.Tables[TABLE_NAME] ?? throw new NullReferenceException($"table '{TABLE_NAME}' not found");
        }

        public static DataTable MakeTeamTable() {
            DataTable table = new DataTable(TABLE_NAME);

            table.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = ID_COL,
                Unique = false,
                AutoIncrement = false
            });

            table.Columns.Add(new DataColumn {
                DataType = typeof(string),
                ColumnName = NAME_COL,
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

            object result = this.source.Compute($"MAX({ID_COL})", string.Empty);
            int maxValue = (result != DBNull.Value) ? Convert.ToInt32(result) : 0;
            int nextID = maxValue + 1;

            foreach (string name in names) {
                var row = source.NewRow();
                row[ID_COL] = nextID;
                row[NAME_COL] = name;
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
            return rows.Select(idRow => idRow.Field<string>(NAME_COL)).NotNull().ToArray();
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
                idList.Add(row.Field<int>(ID_COL));
            }

            foreach (int id in idList.ToArray()) {
                DataRow[] idRows = this.source.Select($"id = {id} ");
                // only consider matches that have the same number of players as the list of names
                if (idRows.Length != names.Length) continue;

                string[] found = idRows.Select(idRow => idRow.Field<string>(NAME_COL)).NotNull().ToArray();
                Array.Sort(found);
                if (found.SequenceEqual(names)) return id;
            }

            return -1;
        }

        /// <summary>
        /// Retrieve an array containing all unique ids.
        /// Will only retrieve each id once.
        /// </summary>
        /// <returns>Array of ids</returns>
        public int[] AllIDs() {
            SortedSet<int> ids = new();
            foreach (DataRow row in this.source.AsEnumerable()) {
                int id = (row.Field<int>(ID_COL));
                if (!ids.Contains(id)) ids.Add(id);
            }
            return ids.ToArray();
        }
    }
}
