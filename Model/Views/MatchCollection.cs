using Model.Tables;
using System.Data;

namespace Model.Views {
    public class MatchCollection(MatchTable matchTable, int roundUID) 
        : ReflectedRowList<MatchRow, MatchTable>(roundUID, matchTable) {

        internal override string ForeignKeyName { get => MatchTable.COL.ROUND; }

        public bool Contains(int lane) {
            string query = $"{MatchTable.COL.ROUND} = {this.foreignKeyValue} AND"
                         + $"{MatchTable.COL.LANE} = {lane}";

            DataRow[] result = this.sourceTable.Select(query);
            return result.Length > 0;
        }     
    }
}
