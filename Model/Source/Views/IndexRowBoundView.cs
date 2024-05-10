
namespace Leagueinator.Model.Views {
    /// <summary>
    /// A row bound view where a single variable can identify the row.
    /// The child table methods get and has accept a single variable of Type T, besides
    /// the parent table identifier.
    /// </summary>
    /// 
    public class IndexRowBoundView<ROW, T>(LeagueTable<ROW> childTable, string[] fkCol, object[] fkVal) 
        : RowBoundView<ROW>(childTable, fkCol, fkVal) where ROW : CustomRow {

        public virtual ROW? this[T t] {
            get {
                if (t == null) throw new IndexOutOfRangeException();
                return this.Get([t]);
            }
        }
    }
}
