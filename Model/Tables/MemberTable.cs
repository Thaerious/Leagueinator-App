﻿using System.Data;

namespace Model.Tables {

    public class MemberRow(DataRow dataRow) : CustomRow(dataRow) {

        public TeamRow Team {
            get => this.League.TeamTable.GetRow(this.Match, this.Index);
        }

        public int Index {
            get => (int)this.DataRow[MemberTable.COL.INDEX];
        }

        public MatchRow Match {
            get => this.League.MatchTable.GetRow((int)this.DataRow[MemberTable.COL.MATCH]);
        }

        public string Player {
            get => (string)this.DataRow[MemberTable.COL.PLAYER];
            set => this.DataRow[MemberTable.COL.PLAYER] = value;
        }

        public static implicit operator string(MemberRow playerRow) => playerRow.Player;
    }

    public class MemberTable : LeagueTable<MemberRow> {

        public static class COL {
            public static readonly string MATCH = "match";
            public static readonly string INDEX = "index";
            public static readonly string PLAYER = "player";
        }

        public MemberRow AddRow(int match, int index, string name) {
            var row = this.NewRow();
            row[COL.MATCH] = match;
            row[COL.INDEX] = index;
            row[COL.PLAYER] = name;

            this.League.EnforceConstraints = false;
            this.Rows.Add(row);
            this.League.EnforceConstraints = true;
            return new(row);
        }

        public MemberTable() : base("members") {
            this.RowChanging += (object sender, DataRowChangeEventArgs e) => {
                MemberRow memberRow = new(e.Row);

                // Check for name in idle table
                int matchUID = (int)e.Row[COL.MATCH];
                MatchRow matchRow = this.League.MatchTable.GetRow(matchUID);
                RoundRow roundRow = matchRow.Round;

                if (this.League.IdleTable.HasRow(roundRow, memberRow.Player)) {
                    throw new ConstraintException(
                        $"Player can not be shared between " +
                        $"table '{this.League.IdleTable.TableName}' and table '{this.League.MemberTable.TableName}' " +
                        $"for a given round."
                    );
                }

                bool hasPlayer = memberRow.Match.Round.Members.Where(row => row.Player == memberRow.Player).Any();

                if (hasPlayer && e.Row.RowState == DataRowState.Detached) {
                    throw new ConstraintException(
                        $"Each round can only have a given player once in table '{this.League.MemberTable.TableName}'"
                    );
                }
            };
        }

        public override void BuildColumns() {
            this.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.MATCH,
                Unique = false,
                AutoIncrement = false
            });

            this.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.INDEX,
                Unique = false,
                AutoIncrement = false
            });

            this.Columns.Add(new DataColumn {
                DataType = typeof(string),
                ColumnName = COL.PLAYER,
                Unique = false,
                AutoIncrement = false
            });

            this.Constraints.Add(
                new UniqueConstraint("UniqueConstraint", [
                this.Columns[COL.MATCH]!,
                this.Columns[COL.INDEX]!,
                this.Columns[COL.PLAYER]!
            ]));

            this.Constraints.Add(new UniqueConstraint(
                [
                    this.Columns[COL.MATCH]!, 
                    this.Columns[COL.PLAYER]!
                ]
                , true
            ));
        }
    }
}
