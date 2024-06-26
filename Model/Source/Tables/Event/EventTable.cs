using System.Data;

namespace Leagueinator.Model.Tables {
    public class EventTable : LeagueTable<EventRow> {
        internal EventTable() : base(tableName: "events") {
            NewInstance = dataRow => new EventRow(dataRow);
        }

        internal static class COL {
            public static readonly string UID = "uid";
            public static readonly string NAME = "name";
            public static readonly string DATE = "date";
            public static readonly string LANE_COUNT = "lanes";
            public static readonly string ENDS_DEFAULT = "ends";
            public static readonly string EVENT_FORMAT = "format";
        }

        internal EventRow AddRow(string eventName, string? date = null) {   
            ArgumentNullException.ThrowIfNull(eventName, nameof(eventName));

            date ??= DateTime.Today.ToString("yyyy-MM-dd");
            var row = NewRow();

            row[COL.NAME] = eventName;
            row[COL.DATE] = date;
            Rows.Add(row);

            return new EventRow(row);
        }

        internal EventRow GetRow(int uid) {
            DataRow[] foundRows = Select($"{COL.UID} = '{uid}'");
            if (foundRows.Length == 0) throw new KeyNotFoundException($"{COL.UID} == {uid}");
            return new EventRow(foundRows[0]);
        }

        internal EventRow GetRow(string eventName) {
            DataRow[] foundRows = Select($"{COL.NAME} = '{eventName}'");
            if (foundRows.Length == 0) throw new KeyNotFoundException($"{COL.NAME} == {eventName}");
            return new EventRow(foundRows[0]);
        }

        internal bool HasRow(string eventName) {
            DataRow[] foundRows = Select($"{COL.NAME} = '{eventName}'");
            return foundRows.Length > 0;
        }

        public EventRow GetLast() {
            return new(Rows[^1]);
        }

        internal override void BuildColumns() {
            Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.UID,
                Unique = true,
                AutoIncrement = true
            });

            Columns.Add(new DataColumn {
                DataType = typeof(string),
                ColumnName = COL.NAME,
                Unique = true
            });

            Columns.Add(new DataColumn {
                DataType = typeof(string),
                ColumnName = COL.DATE,
            });

            Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.LANE_COUNT,
                DefaultValue = 8
            });

            Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.ENDS_DEFAULT,
                DefaultValue = 10
            });

            Columns.Add(new DataColumn {
                DataType = typeof(EventFormat),
                ColumnName = COL.EVENT_FORMAT,
                DefaultValue = EventFormat.AssignedLadder
            });

            PrimaryKey = [Columns[COL.UID]!];
        }
    }
}
