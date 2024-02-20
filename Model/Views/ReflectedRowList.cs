using Leagueinator.Utility;
using Model.Tables;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Reflection;

namespace Model.Views {
    public abstract class ReflectedRowList<R, T>(int foreignKey, T sourceTable) : IEnumerable<R>, IEnumerable
    where R : CustomRow
    where T : CustomTable {
        internal readonly int foreignKeyValue = foreignKey;
        internal readonly T sourceTable = sourceTable;
        internal abstract string ForeignKeyName { get; }

        public List<C> Cast<C>() {
            var list = new List<C>();
            foreach (var row in this) {
                C c = (C)(dynamic)row;
                list.Add(c);
            }
            return list;
        }

        internal virtual R[] Rows {
            get {
                string query = $"{this.ForeignKeyName} = {this.foreignKeyValue}";
                return this.sourceTable.Select(query)
                    .Select(dataRow => ReflectedRowList<R, T>.ConstructRow(this.sourceTable.League, dataRow))
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
            get => Rows.Length;
        }

        public R this[int index] {
            get {
                return this.Rows[index];
            }
        }


        /// <summary>
        /// Invokes the AddRow method for the referred sourceTable.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public R Add(params object[] args) {
            List<object> argList = new List<object>(args);
            argList.Insert(0, this.foreignKeyValue);

            Type tableType = typeof(T);
            List<Type> argTypes = argList.Select(arg => arg.GetType()).ToList();

            Console.WriteLine($"{tableType.Name}.AddRow({argTypes.DelString()})");

            try {
                MethodInfo? method
                    = tableType.GetMethod("AddRow", [.. argTypes])
                    ?? throw new InvalidOperationException($"No matching AddRow({argTypes.DelString()}) method found for type '{tableType}'.");

                R? r = (R?)method.Invoke(this.sourceTable, argList.ToArray());
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
                    return items[pos];
                }
                catch (IndexOutOfRangeException) {
                    throw new InvalidOperationException();
                }
            }
        }

        object? IEnumerator.Current {
            get {
                try {
                    return items[pos];
                }
                catch (IndexOutOfRangeException) {
                    throw new InvalidOperationException();
                }
            }
        }

        public void Dispose() { }

        public bool MoveNext() {
            return (++pos < items.Length);
        }

        public void Reset() {
            pos = -1;
        }
    }
}
