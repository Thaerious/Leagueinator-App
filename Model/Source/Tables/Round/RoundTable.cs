using System.Data;

namespace Leagueinator.Model.Tables {

    public class RoundTable : LeagueTable<RoundRow> {
        internal RoundTable() : base(tableName: "rounds") {
            this.NewInstance = dataRow => new RoundRow(dataRow);
        }

        internal static class COL {
            public static readonly string UID = "uid";
            public static readonly string EVENT = "event_uid";
            public static readonly string INDEX = "index";
        }

        internal RoundRow GetRow(int roundUID) {
            DataRow[] foundRows = this.Select($"{COL.UID} = {roundUID}");
            if (foundRows.Length == 0) throw new KeyNotFoundException($"{COL.UID} == {roundUID}");
            return new(foundRows[0]);
        }

        internal RoundRow GetRow(int eventUID, int index) {
            string query = $"{COL.EVENT} = '{eventUID}' AND {COL.INDEX} = '{index}'";
            DataRow[] foundRows = this.Select(query);
            if (foundRows.Length == 0) throw new KeyNotFoundException(query);
            return new(foundRows[0]);
        }

        internal bool HasRow(int roundUID) {
            DataRow[] foundRows = this.Select($"{COL.UID} = {roundUID}");
            return foundRows.Length > 0;
        }

        internal bool HasRow(int eventUID, int index) {
            string query = $"{COL.EVENT} = '{eventUID}' AND {COL.INDEX} = '{index}'";
            DataRow[] foundRows = this.Select(query);
            return foundRows.Length > 0;
        }

        internal RoundRow AddRow(int eventUID) {
            var row = this.NewRow();
            row[COL.EVENT] = eventUID;
            row[COL.INDEX] = this.NextIndex(eventUID);
            this.Rows.Add(row);
            return new(row);
        }

        /// <summary>
        /// Retrieve the next index for a round in the specified event.
        /// </summary>
        /// <param name="eventUID"></param>
        /// <returns></returns>
        private int NextIndex(int eventUID) {
            int nextIndex = 0;
            DataRow[] foundRows = this.Select($"{COL.EVENT} = '{eventUID}'");
            foreach (DataRow dataRow in foundRows) {
                int index = (int)dataRow[COL.INDEX];
                if (index >= nextIndex) nextIndex = index + 1;
            }
            return nextIndex;
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
                ColumnName = COL.INDEX
            });

            this.Columns.Add(new DataColumn {
                DataType = typeof(int),
                ColumnName = COL.EVENT,
                Unique = false
            });
        }
    }
}

