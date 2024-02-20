using Model.Tables;

namespace Model.Views {
    public class TeamCollection(TeamTable teamTable, int matchUID) 
        : ReflectedRowList<TeamRow, TeamTable>(matchUID, teamTable) {

        internal override string ForeignKeyName { get => MatchTable.COL.UID; }
    }
}
