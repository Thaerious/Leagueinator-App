using Model.Tables;
using System.Data;

namespace Model.Views {
    public class PlayerCollection(MembersTable playerTable, int teamUID)
        : ReflectedRowList<MemberRow, MembersTable, int>(MembersTable.COL.TEAM, teamUID, playerTable) {

        public bool Contains(string name) {
            string query = $"{MembersTable.COL.TEAM} = {this.ForeignKeyValue} AND"
                         + $"{MembersTable.COL.PLAYER} = '{name}'";

            DataRow[] result = this.ChildTable.Select(query);
            return result.Length > 0;
        }
    }
}
