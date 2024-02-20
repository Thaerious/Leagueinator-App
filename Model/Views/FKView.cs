using System.Data;

namespace Model.Views {
    public class FKView<V> : DataView {
        public DataColumn FKCol { get; }
        public V? FKVal { get; }

        /// <summary>
        /// Creates a view where all values in column equals value.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="column"></param>
        /// <param name="value"></param>
        public FKView(DataTable table, DataColumn column, V value) : base(table) {
            this.FKCol = column;
            this.FKVal = value; ;

            if (value is string) {
                this.RowFilter = $"{column.ColumnName} = '{value}'";
            }
            else {
                this.RowFilter = $"{column.ColumnName} = {value}";
            }
        }
    }
}
