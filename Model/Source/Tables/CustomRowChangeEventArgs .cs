using System.Data;
using System.Diagnostics;

namespace Leagueinator.Model.Tables {

    public record Change(string Column, object? OldValue, object? NewValue) {}

    public class CustomRowUpdateEventArgs : EventArgs {
        internal CustomRowUpdateEventArgs(CustomRow row, string columnName, object? oldValue, object? newValue) {
            Row = row;
            List<Change> changes = [];
            this.Change = new(columnName, oldValue, newValue);
        }

        public CustomRow Row { get; }

        public Change Change { get; }


        /// <summary>
        /// Gets the action that has occurred on a <see cref='System.Data.DataRow'/>.
        /// </summary>
        public DataRowAction Action { get; }
    }
}
