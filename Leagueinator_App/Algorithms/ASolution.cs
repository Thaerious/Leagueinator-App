using System.Collections;

namespace Leagueinator.App.Algorithms {

    public abstract class ASolution<T> : IEnumerable<T> {
        public T this[int i] {
            get => this.values[i];
            set => this.values[i] = value;
        }

        public ASolution(int size) {
            this.values = new T[size];
        }

        public int Size => this.values.Length;

        public abstract ASolution<T> Clone();

        public abstract int Evaluate();

        public abstract bool IsValid();

        public abstract void Mutate();

        public IEnumerator<T> GetEnumerator() {
            int index = 0;
            while (index < this.Size) {
                yield return this[index++];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            int index = 0;
            while (index < this.Size) {
                yield return this[index++];
            }
        }

        private readonly T[] values;
    }
}
