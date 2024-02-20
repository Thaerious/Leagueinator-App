using Model.Tables;

namespace Model.Views {
    public class RoundCollection(RoundTable roundTable, int eventUID) 
        : ReflectedRowList<RoundRow, RoundTable>(eventUID, roundTable) {

        internal override string ForeignKeyName { get => RoundTable.COL.EVENT; }
    }
}
