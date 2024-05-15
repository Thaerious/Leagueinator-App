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
        public LeagueTable<ROW> ChildTable { get; }

        public delegate void NewRoundRowEventHandler(object sender, ROW row);

        public event NewRoundRowEventHandler NewBoundRow = delegate { };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="childTable">The child table that references the parent table.</param>
        /// <param name="fkCol">The column in the child table that refers to the reference row.</param>
        /// <param name="fkVal">The value for fkCol table that identifies the rows int the child table (usually the primary key for reference table).</param>
        /// <exception cref="NullReferenceException"></exception>
        public RowBoundView(LeagueTable<ROW> childTable, string[] fkCol, object[] fkVal)
            : this(childTable, fkCol.Select(colName => childTable.Columns[colName]!).ToArray(), fkVal) { }

        public RowBoundView(LeagueTable<ROW> childTable, DataColumn[] fkCol, object[] fkVal) : base(childTable) {
            this.ChildTable = childTable;
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
            get => this.ChildTable.NewInstance(base[index].Row);
        }

        /// <summary>
        /// Adds a new TeamRow to Table B using the AddRow method (must be defined in the custom tables).
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public virtual ROW Add(params object[] args) {
            return this.ChildTable.AddInstance([..this.ForeignKeyValue, ..args]);
        }

        /// <summary>
        /// Retrieves a row from Table B using the GetRow method (must be defined in the custom tables).
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public virtual ROW Get(params object[] args) {
            return this.ChildTable.GetInstance([..this.ForeignKeyValue, ..args]); 
        }

        /// <summary>
        /// Determines if a row exits in Table B using the HasRow method (must be defined in the custom tables).
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public virtual bool Has(params object[] args) {
            return this.ChildTable.HasInstance([..this.ForeignKeyValue, ..args]);
        }

        /// <summary>
        /// Retrieve a row if it exists, otherwise create the row.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public virtual ROW GetIf(params object[] args) {
            if (!this.Has(args)) this.Add(args);
            return this.Get(args);
        }

        /// <summary>
        /// Enumerator for the rows from Table B.
        /// </summary>
        /// <returns></returns>
        public new IEnumerator<ROW> GetEnumerator() {
            for (int i = 0; i < this.Count; i++) {
                if (this[i] is not null) yield return this[i]!;
            }
        }
    }
}
