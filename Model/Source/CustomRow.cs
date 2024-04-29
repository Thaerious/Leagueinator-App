using System.ComponentModel;
using System.Data;

namespace Leagueinator.Model {
    public class CustomRow(DataRow dataRow) : INotifyPropertyChanged {
        public League League => (League)this.DataRow.Table.DataSet!;

        private DataRow DataRow => dataRow;

        public static implicit operator DataRow?(CustomRow customRow) => customRow.DataRow;

        public event PropertyChangedEventHandler? PropertyChanged = delegate { };

        public void Delete() => this.DataRow.Delete();

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
    }
}
