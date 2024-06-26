using System.Data;

namespace Leagueinator.Model.Tables {
    public class CustomRowChangeEventArgs<T> : EventArgs where T : CustomRow   {
        public CustomRowChangeEventArgs(T row, DataRowAction action) {
            Row = row;
            Action = action;
        }

        /// <summary>
        /// Gets the row upon which an action has occurred.
        /// </summary>
        public T Row { get; }

        /// <summary>
        /// Gets the action that has occurred on a <see cref='System.Data.DataRow'/>.
        /// </summary>
        public DataRowAction Action { get; }
    }
}
