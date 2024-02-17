using Model.Tables;
using System.Collections;
using System.Data;

namespace Model.Views {
    public class RoundCollection(RoundTable roundTable, int eventUID) 
        : CustomRowList<RoundRow, RoundTable>(eventUID, roundTable) {

        internal override RoundRow[] Rows {
            get {
                string query = $"{RoundTable.COL.EVENT} = {this.parentUID}";
                return this.table.Select(query)
                    .Select(dataRow => new RoundRow(this.table.League, dataRow))
                    .ToArray();
            }
        }         
    }
}
