using System.Data;

namespace Model.Tables {
    public class CustomRow(DataRow dataRow) {
        public League League {
            get => (League)this.DataRow.Table.DataSet!;
        }
        public DataRow DataRow = dataRow;

        public static implicit operator DataRow?(CustomRow customRow) {
            return customRow.DataRow;
        }

        public void Delete() => this.DataRow.Delete();
    }

    public class InvalidTableException : Exception {
        public InvalidTableException(string? message) : base(message) { }

        public static void CheckTable<T>(DataRow row) {
            if (typeof(T) != row.Table.GetType()) {
                throw new InvalidTableException(
                    $"Incorrect table in DataRow, expected {typeof(T)}, found {row.Table.GetType()}"
                );
            }
        }
    }
}
