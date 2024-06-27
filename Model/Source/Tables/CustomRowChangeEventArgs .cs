using System.Data;
using System.Diagnostics;

namespace Leagueinator.Model.Tables {

    public record Change(string Column, object OldValue, object NewValue) {}

    public class CustomRowUpdateEventArgs<T> : EventArgs where T : CustomRow {
        internal CustomRowUpdateEventArgs(T row, DataRowChangeEventArgs source) {
            Row = row;
            List<Change> changes = [];

            foreach (DataColumn column in source.Row.Table.Columns) {                
                var oldValue = source.Row[column];
                var newValue = source.Row[column];
                Debug.WriteLine($"{ column.ColumnName} {oldValue} {newValue}");
                if (!oldValue.Equals(newValue)) {
                    changes.Add(new(column.ColumnName, oldValue, newValue));
                }
            }

            Changes = changes;
        }

        public T Row { get; }

        public IReadOnlyList<Change> Changes { get; }


        /// <summary>
        /// Gets the action that has occurred on a <see cref='System.Data.DataRow'/>.
        /// </summary>
        public DataRowAction Action { get; }
    }
}
