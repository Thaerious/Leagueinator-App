using System.ComponentModel;
using System.Data;
using System.Diagnostics;

namespace Leagueinator.Model {
    public class CustomRow(DataRow dataRow) : INotifyPropertyChanged {
        public League League => (League)this.DataRow.Table.DataSet!;

        public DataRow DataRow => dataRow;

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

        public class InvalidTableException(string? message) : Exception(message) {
            public static void CheckTable<T>(DataRow row) {
                if (typeof(T) != row.Table.GetType()) {
                    throw new InvalidTableException(
                        $"Incorrect table in DataRow, expected {typeof(T)}, found {row.Table.GetType()}"
                    );
                }
            }
        }

        public override bool Equals(object? that) {
            if (that is null) return false;
            if (that is not CustomRow row) return false;
            return row.DataRow == this.DataRow;
        }
    }
}
