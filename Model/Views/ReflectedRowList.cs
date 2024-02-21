using Leagueinator.Utility;
using Model.Tables;
using System.Collections;
using System.Data;
using System.Reflection;

namespace Model.Views {
    /// <summary>
    /// The R and T (row and table) generic types represent the child table and row.
    /// The is typically the table that contains the list.
    /// The fkName and fkValue are from the child table.
    /// The fkValue should also exist in the reference table.
    /// 
    /// [Child Table] o-> [Reference Table]
    /// 
    /// </summary>
    /// <typeparam name="R">The type of row stored in this list.</typeparam>
    /// <typeparam name="T">The type of table that the row belongs to.</typeparam>
    /// <typeparam name="F">The type of foreign key.</typeparam>
    public class ReflectedRowList<R, T, F> : IEnumerable<R>, IEnumerable
    where R : CustomRow
    where T : CustomTable {
        internal F ForeignKeyValue { get; }
        internal T ChildTable { get; }
        internal string ForeignKeyName { get; }

        /// <param name="fkName">The name of the column in the child table that has the foreign key constraint.</param>
        /// <param name="fkValue">The value shared between the child and reference tables.</param>
        /// <param name="childTable">The child table that the view is targeting.</param>
        public ReflectedRowList(string fkName, F fkValue, T childTable) {
            if (typeof(F) != typeof(string) && typeof(F) != typeof(int)) {
                throw new ArgumentException("Generic type of foreign key must be either string or int.");
            }

            ArgumentNullException.ThrowIfNull(fkValue);

            this.ForeignKeyValue = fkValue;
            this.ChildTable = childTable;
            this.ForeignKeyName = fkName;
        }

        public ReflectedRowList(ForeignKeyConstraint fkConstraint, F fkValue)
            : this(fkConstraint.Columns[0].ColumnName, fkValue, childTable: (T)fkConstraint.Table!) { }

        public List<C> Cast<C>() {
            var list = new List<C>();
            foreach (var row in this) {
                C c = (C)(dynamic)row;
                list.Add(c);
            }
            return list;
        }

        /// <summary>
        /// Retrieve rows from the child table that match the foreign key.
        /// </summary>
        internal virtual R[] Rows {
            get {
                string query = $"{this.ForeignKeyName} = {this.ForeignKeyValue}";
                return this.ChildTable.Select(query)
                    .Select(dataRow => ReflectedRowList<R, T, int>.ConstructRow(this.ChildTable.League, dataRow))
                    .ToArray();
            }
        }

        private static R ConstructRow(League league, DataRow dataRow) {
            Type baseType = typeof(R);
            object[] args = [league, dataRow];
            Type[] argTypes = args.Select(a => a.GetType()).ToArray();

            ConstructorInfo? ctor = baseType.GetConstructor(argTypes)
                                 ?? throw new InvalidOperationException($"No matching constructor found for type {baseType}.");

            return (R)ctor.Invoke(args);
        }

        public int Count {
            get => this.Rows.Length;
        }

        public R this[int index] {
            get {
                return this.Rows[index];
            }
        }


        /// <summary>
        /// Invokes the AddRow method for the referred ChildTable.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public R Add(params object[] args) {
            List<object> argList = new List<object>(args);
            argList.Insert(0, this.ForeignKeyValue!);

            Type tableType = typeof(T);
            List<Type> argTypes = argList.Select(arg => arg.GetType()).ToList();

            Console.WriteLine($"{tableType.Name}.AddRow({argTypes.DelString()})");

            try {
                MethodInfo? method
                    = tableType.GetMethod("AddRow", [.. argTypes])
                    ?? throw new InvalidOperationException($"No matching AddRow({argTypes.DelString()}) method found for type '{tableType}'.");

                R? r = (R?)method.Invoke(this.ChildTable, [.. argList]);
                return r ?? throw new InvalidOperationException($"AddRow method for type '{tableType}' returned NULL.");
            }
            catch (Exception ex) {
                if (ex.InnerException != null) throw ex.InnerException;
                throw;
            }
        }

        public void CopyTo(R[] array, int arrayIndex) {
            foreach (R row in this.Rows) {
                array[arrayIndex++] = row;
            }
        }

        public void Clear() {
            foreach (R row in this.Rows) row.DataRow.Delete();
        }

        public bool Remove(R row) {
            if (!this.Contains(row)) return false;
            row.DataRow.Delete();
            return true;
        }

        public bool Contains(R row) {
            foreach (R r in this.Rows) {
                if (r == row) return true;
            }
            return false;
        }

        public bool Has<TYPE>(string column, TYPE value) {
            return this.ChildTable.AsEnumerable()
                .Where(row => row[this.ForeignKeyName].Equals(this.ForeignKeyValue))
                .Where(row => row[column].Equals(value))
                .Any();
        }

        public DataRow Get<TYPE>(string column, TYPE value) {
            return this.ChildTable.AsEnumerable()
                .Where(row => row[this.ForeignKeyName].Equals(this.ForeignKeyValue))
                .Where(row => row[column].Equals(value))
                .First();
        }

        public IEnumerator<R> GetEnumerator() {
            R[] array = new R[this.Count];
            this.CopyTo(array, 0);
            return new CustomRowListEnumerator<R>(array);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }
    }

    public class CustomRowListEnumerator<R> : IEnumerator<R> {
        private R[] items;
        private int pos = -1;

        public CustomRowListEnumerator(R[] items) {
            this.items = items;
        }

        public R Current {
            get {
                try {
                    return this.items[this.pos];
                }
                catch (IndexOutOfRangeException) {
                    throw new InvalidOperationException();
                }
            }
        }

        object? IEnumerator.Current {
            get {
                try {
                    return this.items[this.pos];
                }
                catch (IndexOutOfRangeException) {
                    throw new InvalidOperationException();
                }
            }
        }

        public void Dispose() { }

        public bool MoveNext() {
            return (++this.pos < this.items.Length);
        }

        public void Reset() {
            this.pos = -1;
        }
    }
}
