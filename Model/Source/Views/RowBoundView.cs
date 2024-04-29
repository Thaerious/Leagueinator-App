using Leagueinator.Model.Tables;
using System.Data;
using System.Reflection;
using System.Runtime.ExceptionServices;
using Leagueinator.Utility;

namespace Leagueinator.Model.Views {
    /// <summary>
    /// A view of all the rows from Table B that has a specific foreign key for
    /// Table A.  This is a data view for Table B.
    /// 
    /// +---------+      +---------+
    /// | Table A |      | Table B |
    /// +---------+      +---------+
    /// | PK      | <--< | FK      |
    /// +---------+      +---------+
    /// 
    /// </summary>
    /// <typeparam name="ROW">Table B row type.</typeparam>
    public class RowBoundView<ROW> : DataView, IEnumerable<ROW> where ROW : CustomRow {
        public object[] ForeignKeyValue { get; }

        public DataColumn[] ForeignKeyColumn { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="childTable">The child table that references the parent table.</param>
        /// <param name="fkCol">The column in the child table that refers to the reference row.</param>
        /// <param name="fkVal">The value for fkCol table that identifies the rows int the child table (usually the primary key for reference table).</param>
        /// <exception cref="NullReferenceException"></exception>
        public RowBoundView(LeagueTable<ROW> childTable, string[] fkCol, object[] fkVal)
            : this(childTable, fkCol.Select(colName => childTable.Columns[colName]!).ToArray(), fkVal) {}

        public RowBoundView(LeagueTable<ROW> childTable, DataColumn[] fkCol, object[] fkVal) : base(childTable) {
            this.RowFilter = TableExtensions.BuildRowFilter(fkCol, fkVal);
            this.ForeignKeyColumn = fkCol;
            this.ForeignKeyValue = fkVal;
        }

        /// <summary>
        /// Retrieve a specific row from Table B by index as a custom row of type 'ROW'.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public new ROW? this[int index] {
            get {
                ConstructorInfo ctor
                    = typeof(ROW).GetConstructor([typeof(DataRow)])
                    ?? throw new InvalidOperationException($"No matching ctor(DataRow) method found for type '{typeof(ROW)}'.");

                return (ROW)ctor.Invoke([base[index].Row]);
            }
        }

        /// <summary>
        /// Adds a new Row to Table B using the AddRow method (must be defined in the custom tables).
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public ROW Add(params object[] args) {
            List<object> argList = [.. (IEnumerable<object>)[.. this.ForeignKeyValue], .. args];

            Type tableType = this.Table!.GetType();
            List<Type> argTypes = argList.Select(arg => arg.GetType()).ToList();

            try {
                MethodInfo? method
                    = tableType.GetMethod("AddRow", [.. argTypes])
                    ?? throw new InvalidOperationException($"No matching AddRow({argTypes.DelString()}) method found for type '{tableType}'.");

                ROW? r = (ROW?)method.Invoke(this.Table, [.. argList]);
                return r ?? throw new InvalidOperationException($"AddRow method for type '{tableType}' returned NULL.");
            }
            catch (Exception ex) {
                var innerException = ex.InnerException ?? ex;
                ExceptionDispatchInfo.Capture(innerException).Throw();
                throw;
            }
        }

        /// <summary>
        /// Retrieves a row from Table B using the GetRow method (must be defined in the custom tables).
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public ROW? Get(params object[] args) {
            List<object> argList = [.. (IEnumerable<object>)[.. this.ForeignKeyValue], .. args];

            Type tableType = this.Table!.GetType();
            List<Type> argTypes = argList.Select(arg => arg.GetType()).ToList();

            try {
                MethodInfo? method
                    = tableType.GetMethod("GetRow", [.. argTypes])
                    ?? throw new InvalidOperationException($"No matching GetRow({argTypes.DelString()}) method found for type '{tableType}'.");

                return (ROW?)method.Invoke(this.Table, [.. argList]);
            }
            catch (Exception ex) {
                var innerException = ex.InnerException ?? ex;
                ExceptionDispatchInfo.Capture(innerException).Throw();
                throw;
            }
        }

        /// <summary>
        /// Determines if a row exits in Table B using the HasRow method (must be defined in the custom tables).
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public bool Has(params object[] args) {
            List<object> argList = [.. (IEnumerable<object>)[.. this.ForeignKeyValue], .. args];

            Type tableType = this.Table!.GetType();
            List<Type> argTypes = argList.Select(arg => arg.GetType()).ToList();

            try {
                MethodInfo? method
                    = tableType.GetMethod("HasRow", [.. argTypes])
                    ?? throw new InvalidOperationException($"No matching HasRow({argTypes.DelString()}) method found for type '{tableType}'.");

                bool? r = (bool?)method.Invoke(this.Table, [.. argList]);
                return r ?? throw new InvalidOperationException($"HasRow method for type '{tableType}' returned NULL.");
            }
            catch (Exception ex) {
                var innerException = ex.InnerException ?? ex;
                ExceptionDispatchInfo.Capture(innerException).Throw();
                throw;
            }
        }

        /// <summary>
        /// Enumerator for the rows from Table B.
        /// </summary>
        /// <returns></returns>
        public new IEnumerator<ROW> GetEnumerator() {
            for (int i = 0; i < this.Count; i++) {
                yield return this[i];
            }
        }
    }
}
