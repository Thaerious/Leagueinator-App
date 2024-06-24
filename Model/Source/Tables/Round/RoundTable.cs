using System.Data;

namespace Leagueinator.Model.Tables {

    public class RoundTable : LeagueTable<RoundRow> {
        public RoundTable() : base("rounds"){
            this.NewInstance = dataRow => new RoundRow(dataRow);
            GetInstance = args => this.GetRow((int)args[0]);
            HasInstance = args => this.HasRow((int)args[0]);
            AddInstance = args => this.AddRow((int)args[0]);
        }

        public static class COL {
            public static readonly string UID = "uid";
            public static readonly string EVENT = "event_uid";
        }

        public RoundRow GetRow(int roundUID) {
            DataRow[] foundRows = this.Select($"{COL.UID} = {roundUID}");
            if (foundRows.Length == 0) throw new KeyNotFoundException($"{COL.UID} == {roundUID}");
            return new(foundRows[0]);
        }

        public bool HasRow(int roundUID) {
            DataRow[] foundRows = this.Select($"{COL.UID} = {roundUID}");
            return foundRows.Length > 0;
        }

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
        }
    }
}

