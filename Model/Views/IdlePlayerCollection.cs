using Model.Tables;
using System.Collections;
using System.Data;

namespace Model.Views {
    public class IdlePlayerCollection(IdleTable idleTable, int roundUID) 
        : ReflectedRowList<IdleRow, IdleTable>(roundUID, idleTable) {

        internal override string ForeignKeyName { get => IdleTable.COL.ROUND; }

        public bool Has(string playerName) {
            string query = $"{IdleTable.COL.ROUND} = {this.foreignKeyValue} AND "
                         + $"{IdleTable.COL.NAME} = '{playerName}'";

            DataRow[] result = this.sourceTable.Select(query);
            return result.Length > 0;
        }

        public IdleRow Get(string playerName) {
            string query = $"{IdleTable.COL.ROUND} = {this.foreignKeyValue} AND "
                         + $"{IdleTable.COL.NAME} = '{playerName}'";

            DataRow[] result = this.sourceTable.Select(query);
            if (result.Length < 1) throw new IndexOutOfRangeException($"Value '{playerName}' not found.");
            return new(this.sourceTable.League, result[0]);
        }
    }
}
