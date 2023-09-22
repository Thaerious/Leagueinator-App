using Leagueinator.App.Algorithms;

namespace Leagueinator.Algorithms {
    public class GreedyWalk {
        private int generation = 0;
        public int MaxGen = 100;

        public int Generation {
            get => this.generation;
        }

        /// <summary>
        /// Generate a new (cloned) solution and apply the algorithm to it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="member"></param>
        /// <param name="cb"></param>
        /// <returns></returns>
        public ASolution<T> Run<T>(ASolution<T> member, Action<ASolution<T>>? cb = null) {
            int bestScore = member.Evaluate();
            ASolution<T> current = member.Clone();
            ASolution<T> best = current;

            cb?.Invoke(best);
            while (bestScore > 0 && this.generation++ < this.MaxGen) {
                current = current.Clone();
                current.Mutate();
                int eval = current.Evaluate();
                if (eval <= bestScore) {
                    bestScore = eval;
                    best = current.Clone();
                }
                else {
                    current = best;
                }
                cb?.Invoke(best);
            }

            return best;
        }
    }
}
