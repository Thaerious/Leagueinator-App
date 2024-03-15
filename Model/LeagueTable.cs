using System.Data;

namespace Model.Tables {
    public abstract class LeagueTable<R> : DataTable where R : CustomRow {
        public League League {
            get => (League)this.DataSet!;
        }

        public LeagueTable(string tableName) : base(tableName) { }

        abstract public void BuildColumns();
    }
}
