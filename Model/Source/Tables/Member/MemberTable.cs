using System.Data;

namespace Leagueinator.Model.Tables {
    public class MemberTable : LeagueTable<MemberRow> {
        internal MemberTable() : base("members") {
            this.NewInstance = dataRow => new MemberRow(dataRow);

            this.RowChanging += (object sender, DataRowChangeEventArgs e) => {
                MemberRow memberRow = new(e.Row);
                string previousName = memberRow.Player;

                if (memberRow.Round.IdlePlayers.Has(previousName)) {
                    memberRow.Round.IdlePlayers.Get(previousName).Remove();
                }

                memberRow.Round.RemoveNameFromTeams(previousName);
            };
        }

        internal static class COL {
            public static readonly string MATCH = "match";
            public static readonly string INDEX = "index";
            public static readonly string PLAYER = "player";
        }

        internal MemberRow AddRow(int match, int index, string name) {
            var row = this.NewRow();
            row[COL.MATCH] = match;
            row[COL.INDEX] = index;
            row[COL.PLAYER] = name;

            //this.League.EnforceConstraints = false;
            this.Rows.Add(row);
            //this.League.EnforceConstraints = true;
            return new(row);
        }

        internal bool HasRow(int match, int index, string player) {
            return this.AsEnumerable<MemberRow>()
                       .Where(row => row.Match.UID == match)
                       .Where(row => row.Index == index)
                       .Where(row => row.Player == player)
                       .Any();
        }

        internal MemberRow GetRow(int match, int index, string player) {
            var rows = this.AsEnumerable<MemberRow>()
                           .Where(row => row.Match.UID == match)
                           .Where(row => row.Index == index)
                           .Where(row => row.Player == player)
                           .ToList();

            if (rows.Count == 0) throw new KeyNotFoundException($"{match} {index} {player}");
            return rows[0];
        }

        internal override void BuildColumns() {
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

            this.Constraints.Add(new UniqueConstraint(
                [
                    this.Columns[COL.MATCH]!,
                    this.Columns[COL.INDEX]!,
                    this.Columns[COL.PLAYER]!
                ]
                , true
            ));
        }
    }
}
