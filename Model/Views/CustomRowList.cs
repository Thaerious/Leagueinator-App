using Model.Tables;
using System.Collections;
using System.Data;
using System.Reflection;

namespace Model.Views {
    public abstract class CustomRowList<R, T> : IEnumerable<R>, IEnumerable where R : CustomRow {        
        internal readonly int parentUID;
        internal readonly T table;

        protected CustomRowList(int parentUID, T table) {
            this.parentUID = parentUID;
            this.table = table;
        }

        public int Count {
            get => Rows.Length;
        }

        public R this[int index] {
            get {
                return this.Rows[index];
            }
        }
        internal abstract R[] Rows { get; }

        public void Add(params object[] args) {
            List<object> argList = new List<object>(args);
            argList.Insert(0, this.parentUID);

            Type tableType = typeof(T);
            List<Type> argTypes = args.Select(arg => arg.GetType()).ToList();

            MethodInfo? method 
                = tableType.GetMethod("Add", argTypes.ToArray()) 
                ?? throw new InvalidOperationException($"No matching Add method found for type {tableType}.");

            method.Invoke(argList.ToArray(), argTypes.ToArray());
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
            return (IEnumerator<R>)array.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }
    }
}
