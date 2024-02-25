using Model.Views;
using System.Data;

namespace Model.Tables {

    public class RoundRow : CustomRow {
        public readonly RowBoundView<IdleRow> IdlePlayers;
        public readonly RowBoundView<MatchRow> Matches;

        public RoundRow(DataRow dataRow) : base(dataRow) {
            ArgumentNullException.ThrowIfNull(this.League.IdleTable.FKRound);
            ArgumentNullException.ThrowIfNull(this.League.MatchTable.FKRound);

            this.IdlePlayers = new(this.League.IdleTable, IdleTable.COL.ROUND, this.UID);
            this.Matches = new(this.League.MatchTable, MatchTable.COL.ROUND, this.UID);
        }

        public int UID {
            get => (int)this.DataRow[RoundTable.COL.UID];
        }

        public static implicit operator int(RoundRow roundRow) => roundRow.UID;

        public EventRow Event {
            get => this.League.EventTable.GetRow((int)this.DataRow[RoundTable.COL.EVENT]);
        }
    }

    public class RoundTable() : LeagueTable<RoundRow>("rounds") {
        public static class COL {
            public static readonly string UID = "uid";
            public static readonly string EVENT = "event_uid";
        }

        internal ForeignKeyConstraint? FKEvent { private set; get; }

        public RoundRow AddRow(int eventUID) {
            var row = this.NewRow();
            row[COL.EVENT] = eventUID;
            this.Rows.Add(row);
            return new(row);
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

            this.FKEvent = new ForeignKeyConstraint(
                "FK_Event_Round",
                this.League.EventTable.Columns[EventsTable.COL.UID]!, // Parent column
                this.Columns[COL.EVENT]!                              // Child column
            ) {
                UpdateRule = Rule.Cascade,
                DeleteRule = Rule.Cascade
            };

            this.Constraints.Add(this.FKEvent);
        }

        internal RoundRow GetRow(int roundUID) {
            DataRow[] foundRows = this.Select($"{COL.UID} = {roundUID}");
            if (foundRows.Length == 0) throw new KeyNotFoundException($"{COL.UID} == {roundUID}");
            return new(foundRows[0]);
        }
    }
}

