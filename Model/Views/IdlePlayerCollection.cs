using Model.Tables;
using System.Collections;
using System.Data;

namespace Model.Views {
    public class IdlePlayerCollection(IdleTable idleTable, int roundUID) 
        : CustomRowList<IdleRow, IdleTable>(roundUID, idleTable) {

        internal override IdleRow[] Rows {
            get {
                string query = $"{IdleTable.COL.ROUND} = {this.parentUID}";
                return this.table.Select(query)
                    .Select(dataRow => new IdleRow(this.table.League, dataRow))
                    .ToArray();
            }
        }

        public bool Contains(string playerName) {
            string query = $"{IdleTable.COL.ROUND} = {this.parentUID} AND "
                         + $"{IdleTable.COL.NAME} = '{playerName}'";

            DataRow[] result = this.table.Select(query);
            return result.Length > 0;
        }
    }
}
