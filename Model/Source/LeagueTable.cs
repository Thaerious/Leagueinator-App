using Leagueinator.Model.Tables;
using System.Collections;
using System.Data;

namespace Leagueinator.Model {
    public abstract class LeagueTable<R> : DataTable, IEnumerable<R> where R : CustomRow {
        
        /// <summary>
        ///  Implement New, Get, Has, to deal with foreign keys.
        /// </summary>

        public Func<DataRow, R> NewInstance { get; protected set; }     // New row from a data row, does not add data to the table.
        public Func<object[], R> GetInstance { get; protected set; }    // Retrieve existing row base on index values.
        public Func<object[], bool> HasInstance { get; protected set; } // Has row based on index values.
        public Func<object[], R> AddInstance { get; protected set; }    // Insert data based on index values.

        public League League {
            get => (League)this.DataSet!;
        }

        public R this[int index] {
            get {
                return this.NewInstance(this.Rows[index]);
            }
        }

        public LeagueTable(string tableName) : base(tableName) {
            NewInstance = args => throw new NotImplementedException();
            GetInstance = args => throw new NotImplementedException();
            HasInstance = args => throw new NotImplementedException();
            AddInstance = args => throw new NotImplementedException();
        }

        public LeagueTable<R> ImportTable(LeagueTable<R> source) {
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

        abstract public void BuildColumns();

        IEnumerator<R> IEnumerable<R>.GetEnumerator() {
            foreach (DataRow dataRow in this.Rows) {
                yield return NewInstance(dataRow);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }
    }
}
