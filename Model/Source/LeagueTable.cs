﻿using Leagueinator.Model.Tables;
using System.Collections;
using System.Data;

namespace Leagueinator.Model {
    public abstract class LeagueTable<R> : DataTable, IEnumerable<R> where R : CustomRow {

        /// <summary>
        ///  Implement New, Get, Has, for use with a RowBoundView
        ///  The object arrays are set as such [foreignKeyValues, ...Req].
        /// </summary>

        internal Func<DataRow, R> NewInstance { get; set; }     // New row from a data row, does not add data to the table.

        public League League {
            get => (League)this.DataSet!;
        }

        public R this[int index] {
            get {
                return this.NewInstance(this.Rows[index]);
            }
        }

        internal LeagueTable(string tableName) : base(tableName) {
            NewInstance = args => throw new NotImplementedException();
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

        abstract internal void BuildColumns();

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
