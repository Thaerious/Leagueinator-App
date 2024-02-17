using Model.Tables;
using System.Data;
using System.Reflection;

namespace Model.Scoring.Plus {
    public class PlusRounds(EventRow eventRow) : CustomTable(eventRow.league, "plus_score_rounds") {
        public readonly EventRow eventRow = eventRow;

        public static class COL {
            public static readonly string WIN = "win";
            public static readonly string LOSS = "loss";
            public static readonly string FOR = "for";
            public static readonly string PLUS_FOR = "for+";
            public static readonly string AGAINST = "against";
            public static readonly string PLUS_AGAINST = "against+";
            public static readonly string RANK = "rank";
            public static readonly string UID = "uid";
            public static readonly string ROUND = "round";
            public static readonly string LANE = "lane";
            public static readonly string TEAM = "team";
            public static readonly string TIE = "tie";
            public static readonly string BOWLS = "bowls";
            public static readonly string ENDS = "ends";
        }

        // The maximum score allowed, excess goes into plus.
        public float Ratio = 1.5f;

        public DataRow GetRow(int uid) {
            var rows = this.AsEnumerable()
                           .Where(row => row.Field<int>(COL.UID) == uid)
                           .ToList();

            if (rows.Count == 0) throw new KeyNotFoundException($"{COL.UID} = {uid}");
            return rows[0];
        }

        public DataView GetRowsByTeam(int team) {
            return new DataView(this) {
                RowFilter = $"{COL.TEAM} = {team}"
            };
        }

        private void Fill() {
            foreach (MatchRow match in this.eventRow.Matches()) {
                this.FillMatch(match);
                this.FillAgainst(match);
                this.AssignRanks(match);
            }
        }

        private void FillMatch(MatchRow matchRow) {
            foreach (TeamRow teamRow in matchRow.Teams()) {
                DataRow row = this.NewRow();
                this.Rows.Add(row);
                teamRow.CopyTo(row);

                int maxScore = (int)(matchRow.Ends * this.Ratio);
                row[COL.FOR] = Math.Min(maxScore, teamRow.Bowls);
                row[COL.PLUS_FOR] = teamRow.Bowls - Math.Min(maxScore, teamRow.Bowls);
            }
        }

        private void FillAgainst(MatchRow match) {


            //foreach (DataRowView us in view) {
            //    foreach (DataRowView them in view) {
            //        if (us == them) continue;
            //        int against = us.Row.Field<int?>(COL.AGAINST) ?? 0;
            //        int plusAgainst = us.Row.Field<int?>(COL.PLUS_AGAINST) ?? 0;

            //        us.Row[COL.AGAINST] = against + (int)them[COL.FOR];
            //        us.Row[COL.PLUS_AGAINST] = plusAgainst + (int)them[COL.PLUS_FOR];
            //    }

            //    int bowlsAgainst = (int)us.Row[COL.AGAINST] + (int)us.Row[COL.PLUS_AGAINST];
            //    if ((int)us[COL.BOWLS] > bowlsAgainst) {
            //        us[COL.WIN] = 1;
            //        us[COL.LOSS] = 0;
            //    }
            //    else if ((int)us[COL.BOWLS] < bowlsAgainst) {
            //        us[COL.WIN] = 0;
            //        us[COL.LOSS] = 1;
            //    }
            //    else if ((int)us[COL.TIE] > 0) {
            //        us[COL.WIN] = 1;
            //        us[COL.LOSS] = 0;
            //    }
            //    else {
            //        us[COL.WIN] = 0;
            //        us[COL.LOSS] = 1;
            //    }
            //}
        }
        private void AssignRanks(MatchRow match) {
            var sortedRows = this.AsEnumerable()
                .Where(row => row.Field<int>(COL.ROUND) == match.Round)
                .OrderBy(row => row.Field<int>(COL.WIN))
                .OrderBy(row => row.Field<int>(COL.FOR))
                .ThenBy(row => row.Field<int>(COL.PLUS_FOR))
                .ThenBy(row => row.Field<int>(COL.AGAINST))
                .ThenBy(row => row.Field<int>(COL.PLUS_AGAINST));

            int rank = 1;
            foreach (var row in sortedRows.Reverse()) {
                this.Select($"uid = '{row["uid"]}'")[0]["rank"] = rank++;
            }
        }

        public override void BuildColumns() {
            foreach (FieldInfo field in typeof(COL).GetFields()) {
                this.Columns.Add(new DataColumn {
                    DataType = field.FieldType,
                    ColumnName = field.Name
                });
            }

            this.Fill();
        }
    }
}
