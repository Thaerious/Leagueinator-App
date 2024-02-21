using System.Data;

namespace Model.Tables {
    public abstract class CustomTable : DataTable {
        public readonly League League;

        public CustomTable(League league, string tableName) : base(tableName) {
            this.League = league;
        }

        abstract public void BuildColumns();
    }
}
