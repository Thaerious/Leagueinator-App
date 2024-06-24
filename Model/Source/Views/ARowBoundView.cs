using Leagueinator.Model.Tables;
using Leagueinator.Utility;
using System.Data;

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
    /// <typeparam name="DEST_ROW">Table B row type.</typeparam>
    public abstract class ARowBoundView<TABLE, SRC_ROW, DEST_ROW> :
                          DataView, IEnumerable<DEST_ROW>
                          where SRC_ROW : CustomRow
                          where DEST_ROW : CustomRow
                          where TABLE : LeagueTable<DEST_ROW> {

        public TABLE ChildTable { get; }

        public SRC_ROW SourceRow { get; }

        public delegate void NewRoundRowEventHandler(object sender, DEST_ROW row);

        public event NewRoundRowEventHandler NewBoundRow = delegate { };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="childTable">The child table that references the parent table.</param>
        /// <param name="fkCol">The column in the child table that refers to the reference row.</param>
        /// <param name="fkVal">The value for fkCol table that identifies the rows int the child table (usually the primary key for reference table).</param>
        /// <exception cref="NullReferenceException"></exception>
        public ARowBoundView(TABLE childTable, SRC_ROW sourceRow) : base(childTable) {
            this.ChildTable = childTable;
            this.SourceRow = sourceRow;
        }

        /// <summary>
        /// Retrieve a specific row from Table B by index as a custom row of type 'DEST_ROW'.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public new DEST_ROW? this[int index] {
            get => this.ChildTable.NewInstance(base[index].Row);
        }

        /// <summary>
        /// Enumerator for the rows from Table B.
        /// </summary>
        /// <returns></returns>
        public new IEnumerator<DEST_ROW> GetEnumerator() {
            for (int i = 0; i < this.Count; i++) {
                if (this[i] is not null) yield return this[i]!;
            }
        }
    }
}
