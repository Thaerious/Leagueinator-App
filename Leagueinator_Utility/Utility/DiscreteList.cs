using Leagueinator.Utility.Seek;
using Newtonsoft.Json;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;

namespace Leagueinator.Utility.ObservableDiscreteCollection {
    [Serializable]
    public class DiscreteList<V> : IEnumerable<V>{
        public delegate void CollectionChangedHnd(DiscreteList<V> source, Args args);
        public event CollectionChangedHnd CollectionChanged = delegate { };
        [JsonProperty] public readonly int MaxSize;

        public class Args {
            public readonly int Key;
            public readonly V NewValue;
            public readonly V OldValue;
            public readonly CollectionChangedAction Action;

            public Args(int key, V newValue, V oldValue, CollectionChangedAction action) {
                this.Key = key;
                this.NewValue = newValue;
                this.OldValue = oldValue;
                this.Action = action;
            }
        }

        /// <summary>
        /// Create a new collection instantiating all objects based on args.
        /// </summary>
        /// <param name="size"></param>
        /// <param name="args"></param>
        public DiscreteList(int size, params object[] args) {
            this.MaxSize = size;

            Type[] types = args.Select(item => item.GetType()).ToArray();
            var constructor = typeof(V).GetConstructor(types) ?? throw new MethodAccessException("Constructor not found");

            for (int i = 0; i < size; i++) {
                this.inner[i] = (V)constructor.Invoke(args);
            }
        }

        public V this[int k] {
            get => this.inner[k]!;
            set => this.Set(k, value);
        }

        private void Set(int key, V value) {
            if (key < 0 || key >= this.MaxSize) throw new IndexOutOfRangeException();
            var args = new Args(key, value, this.inner[key], CollectionChangedAction.Replace);
            this.inner[key] = value;
            CollectionChanged?.Invoke(this, args);
        }

        public void Fill(object[] args) {
            Type[] types = args.Select(item => item.GetType()).ToArray();
            var constructor = typeof(V).GetConstructor(types) ?? throw new MethodAccessException("Constructor not found");

            for (int i = 0; i < this.MaxSize; i++) {
                this.Set(i, (V)constructor.Invoke(args));
            }
        }

        public void ReplaceValue(V replace, V with) {
            int key = ReverseLookup(replace);
            this[key] = with;
        }

        public int ReverseLookup(V value) {
            foreach (var kv in this.inner) {
                if (kv.Value is null) continue;
                if (kv.Value.Equals(value)) return kv.Key;
            }
            return -1;
        }

        public bool Contains(V value) {
            return this.Values.Contains(value);
        }

        public IEnumerator<V> GetEnumerator() {
            return this.inner.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.inner.Values.GetEnumerator();
        }

        [Newtonsoft.Json.JsonIgnore] public int Count => this.inner.Count;
        [DoSeek][Newtonsoft.Json.JsonIgnore] public List<V> Values => this.inner.Values.ToList();
        [Newtonsoft.Json.JsonIgnore] public List<int> Keys => this.inner.Keys.ToList();

        [JsonProperty] private readonly Dictionary<int, V> inner = new();
    }
}
