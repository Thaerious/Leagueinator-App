using Leagueinator.Utility;
using System.Data;

namespace Model.Tables {
    internal class TeamTable{
        public static readonly string TABLE_NAME = "team";
        public static readonly string ID_COL = "uid";
        public static readonly string TEAM_ID_COL = "team_id";
        public static readonly string NAME_COL = "name";

        public static DataTable MakeTable() {
            DataTable table = new DataTable(TABLE_NAME);

            table.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = TEAM_ID_COL,
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

        ///// <summary>
        ///// If the particular team has not been added, add a new team to the table
        ///// </summary>
        ///// <param name="names"></param>
        ///// <returns>The id of the team</returns>
        //public int TryAddTeam(params string[] names) {
        //    int prevId = this.GetID(names);
        //    if (prevId != -1) return prevId;

        //    object result = this.Compute($"MAX({TEAM_ID_COL})", string.Empty);
        //    int maxValue = (result != DBNull.Value) ? Convert.ToInt32(result) : 0;
        //    int nextID = maxValue + 1;

        //    foreach (string name in names) {
        //        var row = this.NewRow();
        //        row[TEAM_ID_COL] = nextID;
        //        row[NAME_COL] = name;
        //        this.Rows.Add(row);
        //    }

        //    return nextID;
        //}

        ///// <summary>
        ///// Retrieve all names with matching id.
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns>An array of names, an empty array if id is not found</returns>
        //public string[] GetNames(int id) {            
        //    DataRow[] rows = this.Select($"{TEAM_ID_COL} = {id}");
        //    return rows.Select(row => row.Field<string>(NAME_COL)).NotNull().ToArray();
        //}

        ///// <summary>
        ///// Determine id of the team that contains all the listed players.
        ///// </summary>
        ///// <param name="names"></param>
        ///// <returns>The team id or -1 if not found</returns>
        //public int GetID(params string[] names) {
        //    Array.Sort(names);
        //    List<int> idList = new();

        //    // retrieve all matches for the first player name
        //    if (names.Length == 0) return -1;
        //    DataRow[] firstNameRows = this.Select($"name = '{names[0].ToLower()}' ");
        //    foreach (DataRow row in firstNameRows) {
        //        idList.Add(row.Field<int>(TEAM_ID_COL));
        //    }

        //    foreach (int id in idList.ToArray()) {
        //        DataRow[] idRows = this.Select($"{TEAM_ID_COL} = {id} ");
        //        // only consider matches that have the same number of players as the list of names
        //        if (idRows.Length != names.Length) continue;

        //        string[] found = idRows.Select(idRow => idRow.Field<string>(NAME_COL)).NotNull().ToArray();
        //        Array.Sort(found);
        //        if (found.SequenceEqual(names)) return id;
        //    }

        //    return -1;
        //}

        ///// <summary>
        ///// Retrieve an array containing all unique ids.
        ///// Will only retrieve each id once.
        ///// </summary>
        ///// <returns>Array of ids</returns>
        //public int[] AllIDs() {
        //    SortedSet<int> ids = new();
        //    foreach (DataRow row in this.AsEnumerable()) {
        //        int id = (row.Field<int>(TEAM_ID_COL));
        //        if (!ids.Contains(id)) ids.Add(id);
        //    }
        //    return ids.ToArray();
        //}
    }
}
