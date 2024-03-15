using Leagueinator.Utility;
using Model.Tables;
using System.Data;
using System.Reflection;
using System.Runtime.ExceptionServices;

namespace Model.Views {
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="R">Child row type.</typeparam>
    /// <typeparam name="T">Foreign key type.</typeparam>
    public class RowBoundView<R> : DataView, IEnumerable<R> where R : CustomRow {
        public object[] ForeignKeyValue { get; }
        public DataColumn[] ForeignKeyColumn { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="childTable">The child table that references the parent table.</param>
        /// <param name="fkCol">The column in the child table that refers to the reference row.</param>
        /// <param name="fkVal">The value for fkCol table that identifies the rows int the child table (usually the primary key for reference table).</param>
        /// <exception cref="NullReferenceException"></exception>
        public RowBoundView(LeagueTable<R> childTable, string[] fkCol, object[] fkVal)
            : this(childTable, fkCol.Select(colName => childTable.Columns[colName]!).ToArray(), fkVal) { }

        public RowBoundView(LeagueTable<R> childTable, string fkCol, object fkVal)
            : this(childTable, [fkCol], [fkVal]) { }

        public RowBoundView(LeagueTable<R> childTable, DataColumn fkCol, object fkVal)
            : this(childTable, [fkCol], [fkVal]) { }

        public RowBoundView(LeagueTable<R> childTable, DataColumn[] fkCol, object[] fkVal) : base(childTable) {
            this.RowFilter = TableExtensions.BuildRowFilter(fkCol, fkVal);
            this.ForeignKeyColumn = fkCol;
            this.ForeignKeyValue = fkVal;
        }

        public new R this[int index] {
            get => this.Get(index);
        }

        public R Get(int index) {
            ConstructorInfo ctor
                = typeof(R).GetConstructor([typeof(DataRow)])
                ?? throw new InvalidOperationException($"No matching ctor(DataRow) method found for type '{typeof(R)}'.");

            return (R)ctor.Invoke([base[index].Row]);
        }

        /// <summary>
        /// Invokes the AddRow method for the referred ChildTable.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public R Add(params object[] args) {
            List<object> argList = [.. (IEnumerable<object>)[.. this.ForeignKeyValue], .. args];

            Type tableType = this.Table!.GetType();
            List<Type> argTypes = argList.Select(arg => arg.GetType()).ToList();

            try {
                MethodInfo? method
                    = tableType.GetMethod("AddRow", [.. argTypes])
                    ?? throw new InvalidOperationException($"No matching AddRow({argTypes.DelString()}) method found for type '{tableType}'.");

                R? r = (R?)method.Invoke(this.Table, [.. argList]);
                return r ?? throw new InvalidOperationException($"AddRow method for type '{tableType}' returned NULL.");
            }
            catch (Exception ex) {
                var innerException = ex.InnerException ?? ex;
                ExceptionDispatchInfo.Capture(innerException).Throw();
                throw;
            }
        }

        public new IEnumerator<R> GetEnumerator() {
            for (int i = 0; i < this.Count; i++) {
                yield return this[i];
            }
        }
    }
}
