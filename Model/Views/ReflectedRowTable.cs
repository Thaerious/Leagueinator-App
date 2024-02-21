using System.Data;

namespace Model.Views {

    /// <summary>
    /// Create a dictionary from a ChildTable.
    /// The ChildTable must have extended properties set on two columns:
    ///  - column.ExtendedProperties.Add("dict", "key");
    ///  - column.ExtendedProperties.Add("dict", "value");
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class ReflectedRowTable<K, V> {
        internal readonly DataView view;
        private readonly DataColumn fkCol;
        private readonly object fkVal;
        internal readonly DataTable souceTable;
        internal readonly DataColumn keyCol; // key column
        internal readonly DataColumn valCol; // value column


        public ReflectedRowTable(DataTable sourceTable, DataColumn fkCol, object fkVal) {
            this.view = new DataView(sourceTable);
            this.fkCol = fkCol;
            this.fkVal = fkVal;

            if (fkVal is string) {
                this.view.RowFilter = $"{fkCol.ColumnName} = '{fkVal}'";
            }
            else {
                this.view.RowFilter = $"{fkCol.ColumnName} = {fkVal}";
            }

            this.souceTable = sourceTable;

            (this.keyCol, this.valCol) = this.IdentifyKVColumns();
        }

        private Tuple<DataColumn, DataColumn> IdentifyKVColumns() {
            DataColumn? _keyCol = null;
            DataColumn? _valCol = null;

            foreach (DataColumn column in this.souceTable.Columns) {
                if (!column.ExtendedProperties.ContainsKey("dict")) continue;
                if (column.ExtendedProperties["dict"] as string == "key") _keyCol = column;
                if (column.ExtendedProperties["dict"] as string == "value") _valCol = column;
            }

            if (_keyCol is null) throw new InvalidOperationException("Missing extended property dict:key.");
            if (_valCol is null) throw new InvalidOperationException("Missing extended property dict:key.");

            return new(_keyCol, _valCol);
        }

        public V? this[K key] {
            get => this.Get(key);
            set => this.Set(key, value);
        }

        private V? Get(K key) {
            if (!this.HasKey(key)) return default;

            return (V?)this.view
                .ToTable()
                .AsEnumerable()
                .Where(row => row[this.keyCol.ColumnName].Equals(key))
                .First()[this.valCol.ColumnName];
        }

        public bool HasKey(K key) {
            return this.view
                .ToTable()
                .AsEnumerable()
                .Where(row => row[this.keyCol.ColumnName].Equals(key))
                .Any();
        }

        public bool Delete(K key) {
            foreach (DataRowView rowView in this.view) {
                if (rowView[this.keyCol.ColumnName].Equals(key)) {
                    rowView.Delete();
                    return true;
                }
            }

            return false;
        }

        private void Set(K key, V? value) {
            if (value == null) {
                this.Delete(key);
                return;
            }

            if (this.HasKey(key)) {
                foreach (DataRowView rowView in this.view) {
                    if (rowView[this.keyCol.ColumnName].Equals(key)) {
                        rowView[this.valCol.ColumnName] = value;
                        return;
                    }
                }
            }
            else {
                if (this.view.Table is null) throw new NullReferenceException("Table is null in view.");
                DataRow row = this.view.Table.NewRow();
                row[this.fkCol] = this.fkVal;
                row[this.keyCol] = key;
                row[this.valCol] = value;
                this.view.Table.Rows.Add(row);
            }
        }
    }
}
