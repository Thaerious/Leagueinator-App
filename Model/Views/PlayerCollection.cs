using Model.Tables;
using System.Collections;
using System.Data;

namespace Model.Views {
    public class PlayerCollection(PlayerTable playerTable, int teamUID) 
        : CustomRowList<PlayerRow, PlayerTable>(teamUID, playerTable) {

        internal override PlayerRow[] Rows {
            get {
                string query = $"{PlayerTable.COL.TEAM} = {this.parentUID}";
                return this.table.Select(query)
                    .Select(dataRow => new PlayerRow(this.table.League, dataRow))
                    .ToArray();
            }
        }

        public bool Contains(string name) {
            string query = $"{PlayerTable.COL.TEAM} = {this.parentUID} AND"
                         + $"{PlayerTable.COL.NAME} = '{name}'";

            DataRow[] result = this.table.Select(query);
            return result.Length > 0;
        }
    }
}
