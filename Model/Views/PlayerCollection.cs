using Model.Tables;
using System.Data;

namespace Model.Views {
    public class PlayerCollection(PlayerTable playerTable, int teamUID) 
        : ReflectedRowList<PlayerRow, PlayerTable>(teamUID, playerTable) {

        internal override string ForeignKeyName { get => PlayerTable.COL.TEAM; }

        public bool Contains(string name) {
            string query = $"{PlayerTable.COL.TEAM} = {this.foreignKeyValue} AND"
                         + $"{PlayerTable.COL.NAME} = '{name}'";

            DataRow[] result = this.sourceTable.Select(query);
            return result.Length > 0;
        }
    }
}
