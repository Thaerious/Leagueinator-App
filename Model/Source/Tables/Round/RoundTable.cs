using System.Data;

namespace Leagueinator.Model.Tables {

    public class RoundTable : LeagueTable<RoundRow> {
        internal RoundTable() : base("rounds") {
            this.NewInstance = dataRow => new RoundRow(dataRow);
        }

        internal static class COL {
            public static readonly string UID = "uid";
            public static readonly string EVENT = "event_uid";
        }

        internal RoundRow GetRow(int roundUID) {
            DataRow[] foundRows = this.Select($"{COL.UID} = {roundUID}");
            if (foundRows.Length == 0) throw new KeyNotFoundException($"{COL.UID} == {roundUID}");
            return new(foundRows[0]);
        }

        internal bool HasRow(int roundUID) {
            DataRow[] foundRows = this.Select($"{COL.UID} = {roundUID}");
            return foundRows.Length > 0;
        }

        internal RoundRow AddRow(int eventUID) {
            var row = this.NewRow();
            row[COL.EVENT] = eventUID;
            this.Rows.Add(row);
            return new(row);
        }

        internal override void BuildColumns() {
            this.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.UID,
                Unique = true,
                AutoIncrement = true
            });

            this.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.EVENT,
                Unique = false
            });
        }
    }
}

