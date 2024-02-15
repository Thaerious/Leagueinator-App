using Model;
using Model.Tables;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using Match = Model.Match;

namespace Leagueinator.App.Scoring.Plus {
    public class PlusSummary : DataTable {

        // The maximum score allowed, excess goes into plus.
        private readonly PlusRounds PlusRounds;

        public PlusSummary(PlusRounds plusRounds) {
            this.PlusRounds = plusRounds;
            this.TableName = "Plus Summary";
            this.Build();
            this.Fill();
            this.AssignRanks();
        }

        public bool HasRow(int team) {
            var rows = this.AsEnumerable()
                           .Where(row => row.Field<int>(RoundTable.COL.TEAM_IDX) == team)
                           .ToList();

            return rows.Count != 0;
        }

        public DataRow GetRow(int team) {
            var rows = this.AsEnumerable()
                           .Where(row => row.Field<int>(RoundTable.COL.TEAM_IDX) == team)
                           .ToList();

            if (rows.Count == 0) throw new KeyNotFoundException($"{RoundTable.COL.TEAM_IDX} = {team}");
            return rows[0];
        }

        private DataRow AddOrGet(int team) {
            if (!this.HasRow(team)) {
                var row = this.NewRow();
                row[RoundTable.COL.TEAM_IDX] = team;
                this.Rows.Add(row);
                return row;
            }

            return this.GetRow(team);
        }

        private void Build() {
            this.MergeWith(dataCol => dataCol.ColumnName, this.PlusRounds);
            this.Columns.Remove(RoundTable.COL.ROUND);
            this.Columns.Remove(RoundTable.COL.LANE);
            this.Columns.Remove(RoundTable.COL.UID);
            this.Columns.Remove(RoundTable.COL.TIE);
        }

        private void Fill() {
            foreach (DataRow roundRow in this.PlusRounds.Rows) {
                int teamIndex = roundRow.Field<int>(RoundTable.COL.TEAM_IDX);
                DataRow summaryRow = this.AddOrGet(teamIndex);

                summaryRow[PlusRounds.COL.WIN] = (int)summaryRow[PlusRounds.COL.WIN] + (int)roundRow[PlusRounds.COL.WIN];
                summaryRow[PlusRounds.COL.LOSS] = (int)summaryRow[PlusRounds.COL.LOSS] + (int)roundRow[PlusRounds.COL.LOSS];
                summaryRow[PlusRounds.COL.FOR] = (int)summaryRow[PlusRounds.COL.FOR] + (int)roundRow[PlusRounds.COL.FOR];
                summaryRow[PlusRounds.COL.PLUS_FOR] = (int)summaryRow[PlusRounds.COL.PLUS_FOR] + (int)roundRow[PlusRounds.COL.PLUS_FOR];
                summaryRow[PlusRounds.COL.AGAINST] = (int)summaryRow[PlusRounds.COL.AGAINST] + (int)roundRow[PlusRounds.COL.AGAINST];
                summaryRow[PlusRounds.COL.PLUS_AGAINST] = (int)summaryRow[PlusRounds.COL.PLUS_AGAINST] + (int)roundRow[PlusRounds.COL.PLUS_AGAINST];
            }
        }

        private void AssignRanks() {
            var sortedRows = this.AsEnumerable()               
                .OrderBy(row => row.Field<int>(PlusRounds.COL.WIN))
                .OrderBy(row => row.Field<int>(PlusRounds.COL.FOR))
                .ThenBy(row => row.Field<int>(PlusRounds.COL.PLUS_FOR))
                .ThenBy(row => row.Field<int>(PlusRounds.COL.AGAINST))
                .ThenBy(row => row.Field<int>(PlusRounds.COL.PLUS_AGAINST));

            int rank = 1;
            foreach (var row in sortedRows.Reverse()) {
                row[PlusRounds.COL.RANK] = rank++;
            }
        }
    }
}
