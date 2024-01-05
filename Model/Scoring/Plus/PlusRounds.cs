using Model;
using Model.Tables;
using System.Data;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Match = Model.Match;

namespace Model.Scoring.Plus {
    public class PlusRounds : DataTable {
        public static class COL {
            public static readonly string WIN = "win";
            public static readonly string LOSS = "loss";
            public static readonly string FOR = "for";
            public static readonly string PLUS_FOR = "for+";
            public static readonly string AGAINST = "against";
            public static readonly string PLUS_AGAINST = "against+";
            public static readonly string RANK = "rank";

            public static readonly string UID = EventTable.COL.UID;
            public static readonly string ROUND = EventTable.COL.ROUND;
            public static readonly string LANE = EventTable.COL.LANE;
            public static readonly string TEAM = EventTable.COL.TEAM_IDX;
            public static readonly string TIE = EventTable.COL.TIE;
            public static readonly string BOWLS = EventTable.COL.BOWLS;
            public static readonly string ENDS = EventTable.COL.ENDS;
        }

        // The maximum score allowed, excess goes into plus.
        public float Ratio = 1.5f;

        public PlusRounds(LeagueEvent leagueEvent) {
            this.TableName = "Plus Rounds";
            this.LeagueEvent = leagueEvent;
            this.Build();
        }

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

        private void Build() {
            this.MergeWith(dataCol => dataCol.ColumnName, this.LeagueEvent.League.EventTable);
            this.BuildColumns();
            this.Columns.Remove(EventTable.COL.EVENT_UID);
            this.Fill();
        }

        private void Fill() {
            foreach (Round round in this.LeagueEvent.Rounds) {
                foreach (Match match in round.Matches) {
                    this.FillMatch(match);
                    this.FillAgainst(match);
                }
                this.AssignRanks(round);
            }
        }

        private void FillMatch(Match match) {
            foreach (Team team in match.Teams) {
                DataRow row = this.NewRow();
                this.Rows.Add(row);

                foreach (DataColumn col in this.LeagueEvent.Table!.Columns) {
                    if (this.Columns[col.ColumnName] == null) continue;
                    row[col.ColumnName] = team.EventTableRow[col.ColumnName];
                }

                int maxScore = (int)(team.Ends * this.Ratio);
                row[COL.FOR] = Math.Min(maxScore, team.Bowls);
                row[COL.PLUS_FOR] = team.Bowls - Math.Min(maxScore, team.Bowls);
            }
        }

        private void FillAgainst(Match match) {
            DataView view = new(this) {
                RowFilter = match.RowFilter
            };

            foreach (DataRowView us in view) {
                foreach (DataRowView them in view) {
                    if (us == them) continue;
                    int against = us.Row.Field<int?>(COL.AGAINST) ?? 0;
                    int plusAgainst = us.Row.Field<int?>(COL.PLUS_AGAINST) ?? 0;

                    us.Row[COL.AGAINST] = against + (int)them[COL.FOR];
                    us.Row[COL.PLUS_AGAINST] = plusAgainst + (int)them[COL.PLUS_FOR];
                }

                int bowlsAgainst = (int)us.Row[COL.AGAINST] + (int)us.Row[COL.PLUS_AGAINST];
                if ((int)us[COL.BOWLS] > bowlsAgainst) {
                    us[COL.WIN] = 1;
                    us[COL.LOSS] = 0;
                }
                else if ((int)us[COL.BOWLS] < bowlsAgainst) {
                    us[COL.WIN] = 0;
                    us[COL.LOSS] = 1;
                }
                else if ((int)us[COL.TIE] > 0) {
                    us[COL.WIN] = 1;
                    us[COL.LOSS] = 0;
                }
                else {
                    us[COL.WIN] = 0;
                    us[COL.LOSS] = 1;
                }
            }
        }
        private void AssignRanks(Round round) {
            var sortedRows = this.AsEnumerable()
                .Where(row => row.Field<int>(COL.ROUND) == round.RoundIndex)
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

        private void BuildColumns() {
            this.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.WIN,
                DefaultValue = 0,
            });

            this.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.LOSS,
                DefaultValue = 0,
            });

            this.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.FOR,
                DefaultValue = 0,
            });

            this.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.PLUS_FOR,
                DefaultValue = 0,
            });

            this.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.AGAINST,
                DefaultValue = 0,
            });

            this.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.PLUS_AGAINST,
                DefaultValue = 0,
            });

            this.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.RANK
            });
        }

        public LeagueEvent LeagueEvent { get; }
    }
}
