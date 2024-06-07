using Leagueinator.Model.Views;
using System.Collections.ObjectModel;
using System.Data;

namespace Leagueinator.Model.Tables {

    public enum EventFormat { AssignedLadder }

    public class EventRow : CustomRow {
        public readonly RowBoundView<RoundRow> Rounds;

        public EventRow(DataRow dataRow) : base(dataRow) {
            this.Rounds = new(this.League.RoundTable, [RoundTable.COL.EVENT], [this.UID]);
        }

        public int EndsDefault {
            get => (int)this[EventTable.COL.ENDS_DEFAULT];
            set => this[EventTable.COL.ENDS_DEFAULT] = value;
        }

        public int LaneCount {
            get => (int)this[EventTable.COL.LANE_COUNT];
            set => this[EventTable.COL.LANE_COUNT] = value;
        }

        public EventFormat EventFormat {
            get => (EventFormat)this[EventTable.COL.EVENT_FORMAT];
            set => this[EventTable.COL.EVENT_FORMAT] = value;
        }

        public int UID {
            get => (int)this[EventTable.COL.UID];
        }

        public static implicit operator int(EventRow eventRow) => eventRow.UID;

        public string Name {
            get => (string)this[EventTable.COL.NAME];
            set => this[EventTable.COL.NAME] = value;
        }

        public string Date {
            get => (string)this[EventTable.COL.DATE];
            set => this[EventTable.COL.DATE] = value;
        }
    }

    public class EventTable : LeagueTable<EventRow> {
        public EventTable() : base("events"){
            this.NewInstance = dataRow => new EventRow(dataRow);
            GetInstance = args => this.GetRow((string)args[0]);
            HasInstance = args => this.HasRow((string)args[0]);
            AddInstance = args => this.AddRow((string)args[0]);
        }

        public static class COL {
            public static readonly string UID = "uid";
            public static readonly string NAME = "name";
            public static readonly string DATE = "date";
            public static readonly string LANE_COUNT = "lanes";
            public static readonly string ENDS_DEFAULT = "ends";
            public static readonly string EVENT_FORMAT = "format";
        }

        public EventRow AddRow(string eventName, string? date = null) {
            date ??= DateTime.Today.ToString("yyyy-MM-dd");
            var row = this.NewRow();

            row[COL.NAME] = eventName;
            row[COL.DATE] = date;
            this.Rows.Add(row);

            return new EventRow(row);
        }

        public EventRow GetRow(string eventName) {
            DataRow[] foundRows = this.Select($"{COL.NAME} = '{eventName}'");
            if (foundRows.Length == 0) throw new KeyNotFoundException($"{COL.NAME} == {eventName}");
            return new EventRow(foundRows[0]);
        }

        public EventRow GetRow(int eventUID) {
            DataRow[] foundRows = this.Select($"{COL.UID} = {eventUID}");
            if (foundRows.Length == 0) throw new KeyNotFoundException($"{COL.UID} == {eventUID}");
            return new EventRow(foundRows[0]);
        }

        public bool HasRow(string eventName) {
            DataRow[] foundRows = this.Select($"{COL.NAME} = '{eventName}'");
            return foundRows.Length > 0;
        }

        public EventRow GetLast() {
            return new(this.Rows[^1]);
        }

        public override void BuildColumns() {
            this.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.UID,
                Unique = true,
                AutoIncrement = true
            });

            this.Columns.Add(new DataColumn {
                DataType = typeof(string),
                ColumnName = COL.NAME,
            });

            this.Columns.Add(new DataColumn {
                DataType = typeof(string),
                ColumnName = COL.DATE,
            });

            this.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.LANE_COUNT,
                DefaultValue = 8
            });

            this.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.ENDS_DEFAULT,
                DefaultValue = 10
            });

            this.Columns.Add(new DataColumn {
                DataType = typeof(EventFormat),
                ColumnName = COL.EVENT_FORMAT,
                DefaultValue = EventFormat.AssignedLadder
            });

            this.PrimaryKey = [this.Columns[COL.UID]!];
        }
    }
}
