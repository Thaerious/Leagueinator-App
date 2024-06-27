using Leagueinator.Model.Tables;
using System.Collections;
using System.Data;
using System.Diagnostics;

namespace Leagueinator.Model {

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">The type of CustomRow this table handles</typeparam>
    public abstract class LeagueTable<T> : DataTable, IEnumerable<T> where T : CustomRow {

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
            this.RowDeleting.Invoke(this, args);
        }

        private void ForwardRowChanged(object sender, DataRowChangeEventArgs e) {
            Debug.WriteLine($"{e.Action}, {e.Action == DataRowAction.Change}");

            if (e.Action == DataRowAction.Add) {
                T customRow = this.NewInstance(e.Row);
                CustomRowAddEventArgs<T> args = new(customRow, e.Action);
                this.RowAdded.Invoke(this, args);
            }

            if (e.Action == DataRowAction.Change) {
                Debug.WriteLine("Invoking Row Updated");
                T customRow = this.NewInstance(e.Row);
                CustomRowUpdateEventArgs<T> args = new(customRow, e);
                this.RowUpdated.Invoke(this, args);
                Debug.WriteLine("Invoked Row Updated");
            }
        }

        public LeagueTable<T> ImportTable(LeagueTable<T> source) {
            foreach (DataRow row in source.Rows) {
                this.ImportRow(row);
            }

            return this;
        }

        public IEnumerator<EventRow> GetEnumerator() {
            foreach (DataRow row in this.Rows) {
                yield return new EventRow(row);
            }
        }

        abstract internal void BuildColumns();

        IEnumerator<T> IEnumerable<T>.GetEnumerator() {
            foreach (DataRow dataRow in this.Rows) {
                yield return NewInstance(dataRow);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }

        new public event TableChangeEventHandler<T> RowDeleting = delegate { };
        public event TableChangeEventHandler<T> RowAdded = delegate { };
        public event CustomRowUpdateEventHandler<T> RowUpdated = delegate { };
    }
}
