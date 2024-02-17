using Model.Tables;
using System.Collections;
using System.Data;

namespace Model.Views {
    public class MatchCollection(MatchTable matchTable, int roundUID) 
        : CustomRowList<MatchRow, MatchTable>(roundUID, matchTable) {

        internal override MatchRow[] Rows {
            get {
                string query = $"{MatchTable.COL.ROUND} = {this.parentUID}";

                return this.table.Select(query)
                    .Select(dataRow => new MatchRow(this.table.League, dataRow))
                    .ToArray();
            }
        }

        public bool Contains(int lane) {
            string query = $"{MatchTable.COL.ROUND} = {this.parentUID} AND"
                         + $"{MatchTable.COL.LANE} = {lane}";

            DataRow[] result = this.table.Select(query);
            return result.Length > 0;
        }     
    }
}
