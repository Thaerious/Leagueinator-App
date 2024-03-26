namespace Leagueinator.Printer.Query {
    public class HashTable<T> {
        private readonly Dictionary<string, List<T>> dictionary = new();
        public int Count {
            get {
                int count = 0;
                foreach (string key in this.dictionary.Keys) {
                    count += this.dictionary[key].Count;
                }
                return count;
            }
        }

        public List<T> ToList() {
            List<T> list = [];
            foreach (string key in this.dictionary.Keys) {
                list.AddRange(this.dictionary[key]);
            }
            return list;
        }

        public void Add(string key, T value) {
            if (!this.dictionary.ContainsKey(key)) {
                this.dictionary[key] = new List<T>();
            }
            this.dictionary[key].Add(value);
        }

        public void Remove(T value) {
            foreach (string key in this.dictionary.Keys) {
                this.dictionary[key].Remove(value);
            }
        }

        public List<T> Get(string key) {
            if (!this.dictionary.ContainsKey(key)) return [];
            return new(this.dictionary[key]);
        }

        public bool Has(string key) {
            return this.dictionary.ContainsKey(key);
        }
    }
}
