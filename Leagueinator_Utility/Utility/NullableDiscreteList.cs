using Leagueinator.Utility.Seek;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Leagueinator.Utility.ObservableDiscreteCollection {
    public enum CollectionChangedAction {
        Remove, Add, Replace
    }

    [Serializable]
    public class NullableDiscreteList<V> {
        public delegate void CollectionChangedHnd(NullableDiscreteList<V> source, Args args);
        public event CollectionChangedHnd CollectionChanged = delegate { };
        [JsonProperty] public readonly int MaxSize;

        public class Args {
            public readonly int Key;
            public readonly V? NewValue;
            public readonly V? OldValue;
            public readonly CollectionChangedAction Action;

            public Args(int key, V? newValue, V? oldValue, CollectionChangedAction action) {
                this.Key = key;
                this.NewValue = newValue;
                this.OldValue = oldValue;
                this.Action = action;
            }
        }

        public NullableDiscreteList(int size) {
            this.MaxSize = size;
        }

        public V? this[int k] {
            get => this.inner.ContainsKey(k) ? this.inner[k] : default;
            set {
                if (this.inner.ContainsKey(k)) {
                    if (value == null) this.Remove(k);
                    else this.Replace(k, value);
                }
                else if (value != null) {
                    this.Set(k, value);
                }
            }
        }

        private void Set(int key, V value) {
            if (key < 0 || key >= this.MaxSize) throw new IndexOutOfRangeException($"index {key} is out of bounds {0} < {this.MaxSize}");
            var args = new Args(key, value, default, CollectionChangedAction.Add);
            this.inner[key] = value;
            CollectionChanged?.Invoke(this, args);
        }

        private void Remove(int key) {
            if (key < 0 || key >= this.MaxSize) throw new IndexOutOfRangeException();
            var args = new Args(key, default, this.inner[key], CollectionChangedAction.Remove);
            _ = this.inner.Remove(key);
            CollectionChanged?.Invoke(this, args);
        }

        private void Replace(int key, V? value) {
            if (key < 0 || key >= this.MaxSize) throw new IndexOutOfRangeException();
            var args = new Args(key, value, this.inner[key], CollectionChangedAction.Replace);
            this.inner[key] = value;
            CollectionChanged?.Invoke(this, args);
        }

        public void Clear() {
            foreach (int k in this.Keys) {
                this.Remove(k);
            }
        }

        public void RemoveValue(V value) {
            if (!this.inner.ContainsValue(value)) return;

            KeyValuePair<int, V?> keyValuePair = this.inner.First(item => {
                return item.Value != null && item.Value.Equals(value);
            });
            this.Remove(keyValuePair.Key);
        }

        public void ReplaceValue(V replace, V with) {
            int key = ReverseLookup(replace);
            this[key] = with;
        }

        public int ReverseLookup(V value) {
            foreach(var kv in this.inner) {
                if (kv.Value is null) continue;
                if (kv.Value.Equals(value)) return kv.Key;
            }
            return -1;
        }

        public bool Contains(V value) {
            return this.Values.Contains(value);
        }

        /// <summary>
        /// Return true if the vavlue at index is not null.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool Has(int index) {
            return this.inner.ContainsKey(index);
        }

        public V?[] toArray() => this.inner.Values.ToArray();

        [Newtonsoft.Json.JsonIgnore] public int Count => this.inner.Count;
        [DoSeek][Newtonsoft.Json.JsonIgnore] public List<V?> Values => this.inner.Values.ToList();
        [Newtonsoft.Json.JsonIgnore] public List<int> Keys => this.inner.Keys.ToList();

        [JsonProperty] private readonly Dictionary<int, V?> inner = new();
    }
}
