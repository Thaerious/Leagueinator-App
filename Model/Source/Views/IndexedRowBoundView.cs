
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Leagueinator.Model.Views {
    /// <summary>
    /// A row bound view where a single variable can identify the row.
    /// The child table methods get and has accept a single variable of Type T, besides
    /// the parent table identifier.
    /// </summary>
    /// 
    public class IndexedRowBoundView<ROW, T>(LeagueTable<ROW> childTable, string[] fkCol, object[] fkVal)
        : RowBoundView<ROW>(childTable, fkCol, fkVal) where ROW : CustomRow,
        INotifyPropertyChanged {

        public virtual ROW? this[T t] {
            get {
                if (t == null) throw new IndexOutOfRangeException();
                return this.Get([t]);
            }
        }

        // This method is called by the Set accessor of each property.  
        // The CallerMemberName attribute that is applied to the optional propertyName  
        // parameter causes the property name of the caller to be substituted as an argument.  
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
