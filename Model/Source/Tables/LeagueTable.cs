using Leagueinator.Model.Tables;
using System.Collections;
using System.Data;
using System.Diagnostics;

namespace Leagueinator.Model {

    public abstract class TableBase(string? tableName) : DataTable(tableName) {
        internal void InvokeRowUpdated(CustomRow row, string columnName, object? oldValue, object? newValue) {
            Debug.WriteLine($"TableBase.InvokeRowUpdated({columnName}, {newValue}) : {this.GetHashCode()}");
            CustomRowUpdateEventArgs args = new(row, columnName, oldValue, newValue);
            this.UpdateCustomRow.Invoke(this, args);
        }

        public TableBase ImportTable(TableBase source) {
            foreach (DataRow row in source.Rows) {
                this.ImportRow(row);
            }

            return this;
        }

        abstract internal void BuildColumns();

        public event UpdateCustomRowEventHandler UpdateCustomRow = delegate { };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">The type of CustomRow this table handles</typeparam>
    public abstract class LeagueTable<T> : TableBase, IEnumerable<T> where T : CustomRow {

        /// <summary>
        ///  Implement New, Get, Has, for use with a RowBoundView
        ///  The object arrays are set as such [foreignKeyValues, ...Req].
        /// </summary>
        internal Func<DataRow, T> NewInstance { get; set; }     // New row from a data row, does not add data to the table.

        public League League => (League)this.DataSet!;

        public T this[int index] {
            get {
                return this.NewInstance(this.Rows[index]);
            }
        }

        internal LeagueTable(string tableName) : base(tableName) {
            NewInstance = args => throw new NotImplementedException();
            base.RowDeleting += this.ForwardRowDeleting;
            base.RowChanged += this.ForwardRowChanged;
        }

        private void ForwardRowDeleting(object sender, DataRowChangeEventArgs e) {
            T customRow = this.NewInstance(e.Row);
            CustomRowAddEventArgs<T> args = new(customRow, e.Action);
            this.CustomRowDelete.Invoke(this, args);
        }

        private void ForwardRowChanged(object sender, DataRowChangeEventArgs e) {
            if (e.Action == DataRowAction.Add) {
                T customRow = this.NewInstance(e.Row);
                CustomRowAddEventArgs<T> args = new(customRow, e.Action);
                this.CustomRowAdd.Invoke(this, args);
            }
        }

        public IEnumerator<EventRow> GetEnumerator() {
            foreach (DataRow row in this.Rows) {
                yield return new EventRow(row);
            }
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator() {
            foreach (DataRow dataRow in this.Rows) {
                yield return NewInstance(dataRow);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }

        public event CustomRowEventHandler<T> CustomRowDelete = delegate { };
        public event CustomRowEventHandler<T> CustomRowAdd = delegate { };        
    }
}
