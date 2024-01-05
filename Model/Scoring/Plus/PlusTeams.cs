using Leagueinator.Utility;
using Model.Tables;
using System.Data;
using System.Diagnostics;

namespace Model.Scoring.Plus {
    public class PlusTeams : DataTable {
        public static class COL {
            public static readonly string TEAM_INDEX = "team_index"; // 'team' in score table
            public static readonly string NAME = "name";
        }

        public PlusTeams() {
            this.TableName = "Plus Teams";
            PlusTeams.BuildColumns(this);
        }

        public void AddRow(int teamIndex, string name) {
            var row = this.NewRow();

            row[COL.TEAM_INDEX] = teamIndex;
            row[COL.NAME] = name;

            this.Rows.Add(row);
        }

        public DataView TeamView(int teamIndex) {
            return new DataView(this) {
                RowFilter = $"{COL.TEAM_INDEX} = {teamIndex}"
            };
        }

        /// <summary>
        /// Retrieve the next available team index.
        /// </summary>
        /// <returns></returns>
        public int NextIndex {
            get {
                if (this.Rows.Count == 0) return 0;
                return (int)this.Compute($"MAX({COL.TEAM_INDEX})", "") + 1;
            }
        }

        /// <summary>
        /// True if a team exists exactly matching names (length and contents).
        /// </summary>
        /// <param name="names"></param>
        /// <returns>The uid of the team, else -1</returns>
        public int LookupTeam(List<string> names) {
            names.Sort();

            for (int i = 0; i <= this.NextIndex; i++) {
                var team = this.TeamView(i).ToList<string>(COL.NAME);
                team.Sort();
                if (team.SequenceEqual(names)) return i;
            }
            return -1;
        }

        public int AddTeamIf(List<string> names) {
            int index = this.LookupTeam(names);
            if (index != -1) return index;

            index = this.NextIndex;
            foreach (string name in names) {
                this.AddRow(index, name);
            }

            return index;
        }

        public static void BuildColumns(DataTable table) {
            table.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.TEAM_INDEX
            });

            table.Columns.Add(new DataColumn {
                DataType = typeof(string),
                ColumnName = COL.NAME
            });
        }

        public List<int> AllIDs() {
            List<int> list = new();
            foreach (DataRow row in this.Rows) {
                int i = (int)row[COL.TEAM_INDEX];
                if (!list.Contains(i)) list.Add(i); 
            }
            return list;
        }
    }
}
