using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Leagueinator.Utility {
    public class MirrorMap<T1, T2> :
            IDictionary<T1, T2>
            where T1 : notnull
            where T2 : notnull {

        private Dictionary<T1, T2> _map = new();
        private Dictionary<T2, T1> _mirror = new();

        public T2 this[T1 key] {
            get {
                return this._map[key];
            }
            set {
                if (key == null) throw new ArgumentNullException(nameof(key));
                if (value == null) throw new ArgumentNullException(nameof(value));
                this._map[key] = value;
                this._mirror[value] = key;
            }
        }

        public ICollection<T1> Keys => this._map.Keys;

        public ICollection<T2> Values => this._map.Values;

        public int Count => this._map.Count;

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(T1 key, T2 value) {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (value == null) throw new ArgumentNullException(nameof(value));
            this._map.Add(key, value);
            this._mirror.Add(value, key);
        }

        public void Add(KeyValuePair<T1, T2> item) {
            this.Add(item.Key, item.Value);
        }

        public void Clear() {
            this._map.Clear();
            this._mirror.Clear();
        }
        public bool Contains(KeyValuePair<T1, T2> item) {
            if (!this._map.ContainsKey(item.Key)) return false;
            if (!this._mirror.ContainsKey(item.Value)) return false;
            if (this._map[item.Key]!.Equals(item.Value)) return true;
            return false;
        }

        public bool ContainsKey(T1 key) => this._map.ContainsKey(key);

        public void CopyTo(KeyValuePair<T1, T2>[] array, int arrayIndex) {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<T1, T2>> GetEnumerator() => this._map.GetEnumerator();

        public bool Remove(T1 key) {
            if (!this._map.ContainsKey(key)) return false;
            this._mirror.Remove(this._map[key]);
            this._map.Remove(key);
            return true;
        }

        public bool Remove(KeyValuePair<T1, T2> item) {
            if (!this._map.ContainsKey(item.Key)) return false;
            this._mirror.Remove(this._map[item.Key]);
            this._map.Remove(item.Key);
            return true;
        }

        public T1 LookupKey(T2 value) {
            return this._mirror[value];
        }

        public MirrorMap<T2, T1> Mirror() {
            return new MirrorMap<T2, T1>() {
                _map = this._mirror,
                _mirror = this._map
            };
        }

        public bool TryGetValue(T1 key, [MaybeNullWhen(false)] out T2 value) => this._map.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator() => this._map.GetEnumerator();
    }
}
