using System.ComponentModel;
using System.Data;
using System.Runtime.Serialization;

namespace Leagueinator.Model {
    [Serializable]
    public class CustomRow(DataRow dataRow) : INotifyPropertyChanged {
        public League League => (League)this.DataRow.Table.DataSet!;

        public DataRow DataRow { get; } = dataRow;

        public static implicit operator DataRow(CustomRow customRow) => customRow.DataRow;

        public event PropertyChangedEventHandler? PropertyChanged = delegate { };

        public void Delete() => this.DataRow.Delete();

        public void Remove() => this.DataRow.Table.Rows.Remove(this.DataRow);

        public object this[string columnName] {
            get => this.DataRow[columnName];
            set {
                this.DataRow[columnName] = value;
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
    }
}
