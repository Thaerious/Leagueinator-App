using Leagueinator.Utility;
using Model.Tables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leagueinator.App.Scoring.Plus {
    public class PlusTeams : DataTable {
        public static class COL {
            public static readonly string TEAM_INDEX = "team_index"; // 'team' in score table
            public static readonly string NAME = "name";
        }

        public PlusTeams() {
            PlusTeams.BuildColumns(this);
        }

        public void AddRow(int teamIndex, string name) {
            var row = this.NewRow();

            row[COL.TEAM_INDEX] = teamIndex;
            row[COL.NAME] = name;

            this.Rows.Add(row);
        }

        public DataView GetView(int teamIndex) {
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
                return (int)this.Compute($"MAX({COL.TEAM_INDEX})", "");
            }
        }

        /// <summary>
        /// True if a team exists exactly matching names (length and contents).
        /// </summary>
        /// <param name="names"></param>
        /// <returns>The uid of the team, else -1</returns>
        public int FindTeam(List<string> names) {
            names.Sort();

            for (int i = 0; i <= this.NextIndex; i++) {
                var team = this.GetView(i).ToList<string>(COL.NAME);
                team.Sort();
                if (team.SequenceEqual(names)) return i;
            }
            return -1;
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
    }
}
