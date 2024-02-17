using Model.Views;
using System.Data;

namespace Model.Tables {

    public class RoundRow : CustomRow {
        public readonly IdlePlayerCollection IdlePlayers;
        public readonly MatchCollection Matches;

        public RoundRow(League league, DataRow row) : base(league, row) {
            this.IdlePlayers = new(league.IdleTable, this.Event, this);
            this.Matches = new(league.MatchTable, this);
        }

        public int UID {
            get => (int)this.DataRow[RoundTable.COL.UID];
        }

        public EventRow Event {
            get => this.League.EventTable.GetRow((int)this.DataRow[RoundTable.COL.EVENT]);
        }

        public static implicit operator int(RoundRow roundRow) => roundRow.UID;
    }

    public class RoundTable(League league) : CustomTable(league, "rounds") {
        public static class COL {
            public static readonly string UID = "uid";
            public static readonly string EVENT = "event_uid";
        }

        public RoundRow AddRow(int eventUID) {
            var row = this.NewRow();
            row[COL.EVENT] = eventUID;
            this.Rows.Add(row);
            return new(League, row);
        }

        public override void BuildColumns() {
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

        internal RoundRow GetRow(int roundUID) {
            DataRow[] foundRows = this.Select($"{COL.UID} = {roundUID}");
            if (foundRows.Length == 0) throw new KeyNotFoundException($"{COL.UID} == {roundUID}");
            return new(this.League, foundRows[0]);
        }
    }
}

