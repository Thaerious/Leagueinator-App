using System.ComponentModel;
using System.Data;

namespace Leagueinator.Model {
    [Serializable]
    public class CustomRow(DataRow dataRow) : INotifyPropertyChanged {
        public League League => (League)this.DataRow.Table.DataSet!;

        public DataRow DataRow { get; } = dataRow;

        public static implicit operator DataRow(CustomRow customRow) => customRow.DataRow;

        public event PropertyChangedEventHandler? PropertyChanged = delegate { };

        public void Remove() => this.DataRow.Table.Rows.Remove(this.DataRow);

        public object this[string columnName] {
            get => this.DataRow[columnName];
            set {
                this.DataRow[columnName] = value;

                // Invoke the property change event for WPF
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(columnName));
            }
        }

        public override bool Equals(object? that) {
            if (that is null) return false;
            if (that is not CustomRow row) return false;
            return row.DataRow == this.DataRow;
        }

        public override int GetHashCode() {
            return this.DataRow.GetHashCode();
        }

        internal void InvokeRowUpdated(string propertyName, object? oldValue, object newValue) {
            TableBase tableBase = (TableBase)this.DataRow.Table;
            tableBase.InvokeRowUpdated(this, propertyName, oldValue, newValue);
        }
    }
}
