using Model.Tables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Views {
    public class TeamCollection(TeamTable teamTable, int matchUID) 
        : CustomRowList<TeamRow, TeamTable>(matchUID, teamTable) { 

        internal override TeamRow[] Rows {
            get {
                string query = $"{MatchTable.COL.UID} = {this.parentUID}";
                return this.table.Select(query)
                    .Select(dataRow => new TeamRow(this.table.League, dataRow))
                    .ToArray();
            }
        }
    }
}
