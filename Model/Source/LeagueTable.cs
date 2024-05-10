using Leagueinator.Model.Tables;
using System.Collections;
using System.Data;

namespace Leagueinator.Model {
    public abstract class LeagueTable<R> : DataTable, IEnumerable<R> where R : CustomRow {
        public Func<DataRow, R> NewInstance;

        public League League {
            get => (League)this.DataSet!;
        }

        public LeagueTable(string tableName, Func<DataRow, R> newInstance) : base(tableName) { 
            this.NewInstance = newInstance;
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
